using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
#if !NET45
using Silk.NET.WebGPU;
using Silk.NET.Core.Native;

namespace SharpCanvas.WebGPU
{
    public unsafe partial class WebGPUBridge
    {
        private readonly Silk.NET.WebGPU.WebGPU _wgpu;
        private readonly Instance* _instance;

        public WebGPUBridge()
        {
            // Initialize Silk.NET WebGPU
            _wgpu = Silk.NET.WebGPU.WebGPU.GetApi();
            
            // Create Instance
            // Note: In a real app we might want to pass descriptor, but null is usually fine for defaults
            _instance = _wgpu.CreateInstance(null);
            
            if (_instance == null)
            {
                Console.WriteLine("[SharpCanvas] Failed to create WebGPU Instance");
            }
            else
            {
                Console.WriteLine($"[SharpCanvas] WebGPU Instance created: {(long)_instance:X}");
            }
        }

        internal Silk.NET.WebGPU.Queue* _currentQueue;

        public Task<object> requestAdapter()
        {
            Console.WriteLine("[SharpCanvas] WebGPUBridge: requestAdapter called");
            var tcs = new TaskCompletionSource<object>();

            if (_instance == null)
            {
                tcs.SetException(new Exception("WebGPU Instance not initialized"));
                return tcs.Task;
            }

            var options = new RequestAdapterOptions
            {
                PowerPreference = PowerPreference.HighPerformance,
                ForceFallbackAdapter = false
            };

            PfnRequestAdapterCallback callback = new PfnRequestAdapterCallback((status, adapter, message, userdata) =>
            {
                if (status == RequestAdapterStatus.Success)
                {
                    Console.WriteLine($"[SharpCanvas] Adapter obtained: {(long)adapter:X}");
                    tcs.SetResult(new Adapter(this, _wgpu, adapter));
                }
                else
                {
                    string msg = Marshal.PtrToStringAnsi((IntPtr)message) ?? "Unknown Error";
                    Console.WriteLine($"[SharpCanvas] Failed to get adapter: {msg}");
                    tcs.SetException(new Exception($"Failed to get adapter: {msg}"));
                }
            });

            _wgpu.InstanceRequestAdapter(_instance, &options, callback, null);

            return tcs.Task;
        }
    }

    public unsafe class Adapter
    {
        private readonly WebGPUBridge _bridge;
        private readonly Silk.NET.WebGPU.WebGPU _wgpu;
        private readonly Silk.NET.WebGPU.Adapter* _adapter;

        public Adapter(WebGPUBridge bridge, Silk.NET.WebGPU.WebGPU wgpu, Silk.NET.WebGPU.Adapter* adapter)
        {
            _bridge = bridge;
            _wgpu = wgpu;
            _adapter = adapter;
        }

        public Task<object> requestDevice()
        {
            Console.WriteLine("[SharpCanvas] WebGPUBridge: requestDevice called");
            var tcs = new TaskCompletionSource<object>();

            var descriptor = new DeviceDescriptor
            {
            };

            PfnRequestDeviceCallback callback = new PfnRequestDeviceCallback((status, device, message, userdata) =>
            {
                if (status == RequestDeviceStatus.Success)
                {
                    Console.WriteLine($"[SharpCanvas] Device obtained: {(long)device:X}");
                    var q = _wgpu.DeviceGetQueue(device);
                    _bridge._currentQueue = q;
                    tcs.SetResult(new Device(_wgpu, device));
                }
                else
                {
                    string msg = Marshal.PtrToStringAnsi((IntPtr)message) ?? "Unknown Error";
                    Console.WriteLine($"[SharpCanvas] Failed to get device: {msg}");
                    tcs.SetException(new Exception($"Failed to get device: {msg}"));
                }
            });

            _wgpu.AdapterRequestDevice(_adapter, &descriptor, callback, null);

            return tcs.Task;
        }
    }

    public unsafe class Device
    {
        private readonly Silk.NET.WebGPU.WebGPU _wgpu;
        private readonly Silk.NET.WebGPU.Device* _device;
        public Queue queue { get; }

        public Device(Silk.NET.WebGPU.WebGPU wgpu, Silk.NET.WebGPU.Device* device)
        {
            _wgpu = wgpu;
            _device = device;
            
            var q = _wgpu.DeviceGetQueue(_device);
            queue = new Queue(_wgpu, q);
        }
    }

    public unsafe class Queue
    {
        private readonly Silk.NET.WebGPU.WebGPU _wgpu;
        private readonly Silk.NET.WebGPU.Queue* _queue;

        public Queue(Silk.NET.WebGPU.WebGPU wgpu, Silk.NET.WebGPU.Queue* queue)
        {
            _wgpu = wgpu;
            _queue = queue;
        }
    }

    public class Navigator
    {
        public WebGPUBridge gpu { get; }

        public Navigator()
        {
            gpu = new WebGPUBridge();
        }
    }
    
    // Extension to WebGPUBridge
    public unsafe partial class WebGPUBridge
    {
        public void submit(int[] commands)
        {
            if (_currentQueue == null)
            {
                Console.WriteLine("[SharpCanvas] Error: Queue not initialized");
                return;
            }

            Console.WriteLine($"[SharpCanvas] submit called with {(commands != null ? commands.Length : 0)} commands");
            
            if (commands == null || commands.Length == 0) return;

            int i = 0;
            while (i < commands.Length)
            {
                int cmd = commands[i++];
                switch (cmd)
                {
                    case 1: // BEGIN_RENDER_PASS
                        Console.WriteLine("  CMD: BEGIN_RENDER_PASS");
                        break;
                    case 2: // END_RENDER_PASS
                        Console.WriteLine("  CMD: END_RENDER_PASS");
                        break;
                    case 3: // SET_PIPELINE
                        Console.WriteLine("  CMD: SET_PIPELINE");
                        break;
                    case 4: // DRAW
                        int vertexCount = commands[i++];
                        int instanceCount = commands[i++];
                        int firstVertex = commands[i++];
                        int firstInstance = commands[i++];
                        Console.WriteLine($"  CMD: DRAW {vertexCount}, {instanceCount}, {firstVertex}, {firstInstance}");
                        break;
                    default:
                        Console.WriteLine($"  Unknown command: {cmd}");
                        break;
                }
            }
        }
    }
}
#endif

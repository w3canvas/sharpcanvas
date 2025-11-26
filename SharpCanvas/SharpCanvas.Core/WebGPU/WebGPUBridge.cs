using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
#if !NET45
using Silk.NET.WebGPU;
using Silk.NET.Core.Native;

namespace SharpCanvas.WebGPU
{
    public unsafe class WebGPUBridge
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

        public Task<object> requestAdapter()
        {
            Console.WriteLine("[SharpCanvas] WebGPUBridge: requestAdapter called");
            var tcs = new TaskCompletionSource<object>();

            if (_instance == null)
            {
                tcs.SetException(new Exception("WebGPU Instance not initialized"));
                return tcs.Task;
            }

            // Create options
            var options = new RequestAdapterOptions
            {
                PowerPreference = PowerPreference.HighPerformance,
                ForceFallbackAdapter = false
            };

            // Define callback
            PfnRequestAdapterCallback callback = new PfnRequestAdapterCallback((status, adapter, message, userdata) =>
            {
                if (status == RequestAdapterStatus.Success)
                {
                    Console.WriteLine($"[SharpCanvas] Adapter obtained: {(long)adapter:X}");
                    tcs.SetResult(new Adapter(_wgpu, adapter));
                }
                else
                {
                    string msg = Marshal.PtrToStringAnsi((IntPtr)message);
                    Console.WriteLine($"[SharpCanvas] Failed to get adapter: {msg}");
                    tcs.SetException(new Exception($"Failed to get adapter: {msg}"));
                }
            });

            // We need to keep the callback alive? Silk.NET might handle it if we pass it directly?
            // Actually, Silk.NET generates a struct for the callback usually, or takes a delegate.
            // Let's check if RequestAdapter takes a delegate or a function pointer.
            // Usually it takes a PfnRequestAdapterCallback delegate.
            
            _wgpu.InstanceRequestAdapter(_instance, &options, callback, null);

            return tcs.Task;
        }
    }

    public unsafe class Adapter
    {
        private readonly Silk.NET.WebGPU.WebGPU _wgpu;
        private readonly Silk.NET.WebGPU.Adapter* _adapter;

        public Adapter(Silk.NET.WebGPU.WebGPU wgpu, Silk.NET.WebGPU.Adapter* adapter)
        {
            _wgpu = wgpu;
            _adapter = adapter;
        }

        public Task<object> requestDevice()
        {
            Console.WriteLine("[SharpCanvas] WebGPUBridge: requestDevice called");
            var tcs = new TaskCompletionSource<object>();

            // Create descriptor
            var descriptor = new DeviceDescriptor
            {
                // Defaults
            };

            PfnRequestDeviceCallback callback = new PfnRequestDeviceCallback((status, device, message, userdata) =>
            {
                if (status == RequestDeviceStatus.Success)
                {
                    Console.WriteLine($"[SharpCanvas] Device obtained: {(long)device:X}");
                    tcs.SetResult(new Device(_wgpu, device));
                }
                else
                {
                    string msg = Marshal.PtrToStringAnsi((IntPtr)message);
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
}
#endif

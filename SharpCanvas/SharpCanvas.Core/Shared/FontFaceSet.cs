using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpCanvas.Shared
{
    public class FontFaceSet
    {
        private readonly List<FontFace> _fontFaces = new List<FontFace>();
        private TaskCompletionSource<bool> _readyTcs;

        public FontFaceSet()
        {
            _readyTcs = new TaskCompletionSource<bool>();
            _readyTcs.SetResult(true); // Initially ready
            status = "loaded";
        }

        public string status { get; private set; }

        public Task ready => _readyTcs.Task;

        public Task<FontFaceSet> loaded => ready.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                throw t.Exception;
            }
            return this;
        });

        public void add(FontFace fontFace)
        {
            _fontFaces.Add(fontFace);
            UpdateReadyTask();
            fontFace.load();
        }

        public bool delete(FontFace fontFace)
        {
            var removed = _fontFaces.Remove(fontFace);
            if (removed)
            {
                UpdateReadyTask();
            }
            return removed;
        }

        public void clear()
        {
            _fontFaces.Clear();
            UpdateReadyTask();
        }

        private void UpdateReadyTask()
        {
            status = "loading";
            var loadingTasks = _fontFaces.Select(f => f.loaded).ToList();
            if (loadingTasks.Count == 0)
            {
                status = "loaded";
                _readyTcs = new TaskCompletionSource<bool>();
                _readyTcs.SetResult(true);
                return;
            }

            _readyTcs = new TaskCompletionSource<bool>();
            Task.WhenAll(loadingTasks).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    status = "error";
                    _readyTcs.SetException(t.Exception);
                }
                else
                {
                    status = "loaded";
                    _readyTcs.SetResult(true);
                }
            });
        }

        public IEnumerable<FontFace> values()
        {
            return _fontFaces;
        }
    }
}

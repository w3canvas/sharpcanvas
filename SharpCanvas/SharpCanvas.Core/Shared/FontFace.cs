using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharpCanvas.Shared
{
    public class FontFace
    {
        private readonly string? _source;
        private readonly TaskCompletionSource<byte[]> _loader;

        public FontFace(string family, byte[] source, FontFaceDescriptors descriptors)
        {
            this.family = family;
            this.style = descriptors.style;
            this.weight = descriptors.weight;
            this.stretch = descriptors.stretch;
            this.unicodeRange = descriptors.unicodeRange;
            this.featureSettings = descriptors.featureSettings;
            this.variationSettings = descriptors.variationSettings;
            this.display = descriptors.display;

            _loader = new TaskCompletionSource<byte[]>();
            _loader.SetResult(source);
            status = "loaded";
        }

        public FontFace(string family, string source, FontFaceDescriptors descriptors)
        {
            this.family = family;
            this.style = descriptors.style;
            this.weight = descriptors.weight;
            this.stretch = descriptors.stretch;
            this.unicodeRange = descriptors.unicodeRange;
            this.featureSettings = descriptors.featureSettings;
            this.variationSettings = descriptors.variationSettings;
            this.display = descriptors.display;

            _source = source;
            _loader = new TaskCompletionSource<byte[]>();
            status = "unloaded";
        }

        public string display { get; set; }

        public string family { get; set; }

        public string featureSettings { get; set; }

        public Task<FontFace> loaded => load();

        public string status { get; private set; }

        public string stretch { get; set; }

        public string style { get; set; }

        public string unicodeRange { get; set; }

        public string variationSettings { get; set; }

        public string weight { get; set; }

        public async Task<FontFace> load()
        {
            if (status == "loaded" || status == "loading")
            {
                await _loader.Task;
                return this;
            }

            status = "loading";
            try
            {
                var client = new HttpClient();
                var fontData = await client.GetByteArrayAsync(_source);
                _loader.SetResult(fontData);
                status = "loaded";
            }
            catch (Exception e)
            {
                _loader.SetException(e);
                status = "error";
                throw;
            }

            return this;
        }

        public Task<byte[]> GetDataAsync()
        {
            return _loader.Task;
        }
    }

    public class FontFaceDescriptors
    {
        public string style { get; set; } = "normal";
        public string weight { get; set; } = "normal";
        public string stretch { get; set; } = "normal";
        public string unicodeRange { get; set; } = "U+0-10FFFF";
        public string featureSettings { get; set; } = "normal";
        public string variationSettings { get; set; } = "normal";
        public string display { get; set; } = "auto";
    }
}

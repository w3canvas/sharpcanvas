using NUnit.Framework;
using SharpCanvas.Shared;
using System.IO;
using System.Threading.Tasks;
using Moq;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaFontFaceSetTests
    {
        private byte[] _fontBytes;

        [SetUp]
        public void Setup()
        {
            _fontBytes = File.ReadAllBytes("Fonts/DejaVuSans.ttf");
        }

        [Test]
        public void TestFontFaceSet_IsInitiallyLoadedAndReady()
        {
            var fontFaceSet = new FontFaceSet();
            Assert.That(fontFaceSet.status, Is.EqualTo("loaded"));
            Assert.That(fontFaceSet.ready.IsCompletedSuccessfully, Is.True);
        }

        [Test]
        public async Task TestFontFaceSet_BecomesLoadingWhenFontIsAdded()
        {
            var fontFaceSet = new FontFaceSet();
            var fontFace = new FontFace("MyTestFont", _fontBytes, new FontFaceDescriptors());

            fontFaceSet.add(fontFace);
            await fontFace.LoadCalled;

            Assert.That(fontFaceSet.status, Is.EqualTo("loading"));
            await fontFaceSet.ready;
            Assert.That(fontFaceSet.status, Is.EqualTo("loaded"));
        }

        [Test]
        public async Task TestDocumentFonts_Integration()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var document = mockDocument.Object;
            Assert.That(document.defaultView.fonts, Is.Not.Null);

            var fontFace = new FontFace("MyTestFont", _fontBytes, new FontFaceDescriptors());
            document.defaultView.fonts.add(fontFace);
            await fontFace.LoadCalled;

            Assert.That(document.defaultView.fonts.status, Is.EqualTo("loading"));
            await document.defaultView.fonts.ready;
            Assert.That(document.defaultView.fonts.status, Is.EqualTo("loaded"));
        }
    }
}

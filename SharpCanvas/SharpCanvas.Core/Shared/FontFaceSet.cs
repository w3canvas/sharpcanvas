using System.Collections.Generic;

namespace SharpCanvas.Shared
{
    public class FontFaceSet
    {
        private readonly List<FontFace> _fontFaces = new List<FontFace>();

        public void add(FontFace fontFace)
        {
            _fontFaces.Add(fontFace);
        }

        public bool delete(FontFace fontFace)
        {
            return _fontFaces.Remove(fontFace);
        }

        public void clear()
        {
            _fontFaces.Clear();
        }

        public IEnumerable<FontFace> values()
        {
            return _fontFaces;
        }
    }
}

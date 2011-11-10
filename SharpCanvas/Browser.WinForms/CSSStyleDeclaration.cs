using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    public class CSSStyleDeclaration: ICSSStyleDeclaration
    {
        #region Fields

        private IHTMLCanvasElement _canvasElement;
        private readonly Regex rDisplayNone = new Regex(@"display:\s*none");
        private readonly Regex rLeft = new Regex(@"left:(?<left>\d+)");
        private readonly Regex rTop = new Regex(@"top:(?<top>\d+)");
        private readonly Regex rZIndex = new Regex(@"z-index:\s*(?<index>\d+)");

        private string _cssText;
        private string _display;
        private int _top;
        private int _left;
        private int _zIndex;
        private bool _eventsAllowed = true;
        private int _width;
        private int _height;

        #endregion

        #region Constructor

        public CSSStyleDeclaration(IHTMLCanvasElement canvasElement)
        {
            _canvasElement = canvasElement;
        }

        #endregion

        #region Implementation of ICSSStyleDeclaration

        public event StyleChangedHandler StyleChanged;

        public void SuspendEvnets()
        {
            _eventsAllowed = false;
        }

        public void ResumeEvents()
        {
            _eventsAllowed = true;
        }

        public string cssText
        {
            get { return _cssText; }
            set
            {
                _cssText = ParseCSSText(value);
            }
        }

        /// <summary>
        /// Parse passed css text, assign values, fire appropriate events..
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Return formatted and filtered css text</returns>
        private string ParseCSSText(string value)
        {
            string validCss = string.Empty;
            if (rDisplayNone.IsMatch(value.ToString()))
            {
                display = "none";
                validCss += string.Format("display:{0};", display);
            }

            if (rTop.IsMatch(value.ToString()))
            {
                int y = Convert.ToInt32(rTop.Match(value.ToString()).Groups["top"].Value);
                top = y;
                validCss += string.Format("top:{0};", y);
            }

            if (rLeft.IsMatch(value.ToString()))
            {
                int x = Convert.ToInt32(rLeft.Match(value.ToString()).Groups["left"].Value);
                left = x;
                validCss += string.Format("left:{0};", x);
            }

            if (rZIndex.IsMatch(value.ToString()))
            {
                int idx = 0;
                bool isValidZIndex = int.TryParse(rZIndex.Match(value.ToString()).Groups["index"].Value, out idx);
                if (isValidZIndex)
                {
                    zIndex = idx;
                    validCss += string.Format("z-index:{0};", zIndex);
                }
            }
            return validCss;
        }

        private void FireStyleChanged(string attribute, object value)
        {
            if (StyleChanged != null && _eventsAllowed)
            {
                StyleChanged(attribute, value);
            }
        }

        public ulong length
        {
            get { throw new NotImplementedException(); }
        }

        public string item(ulong index)
        {
            throw new NotImplementedException();
        }

        public string getPropertyValue(string property)
        {
            throw new NotImplementedException();
        }

        public string getPropertyPriority(string property)
        {
            throw new NotImplementedException();
        }

        public void setProperty(string property, string value)
        {
            throw new NotImplementedException();
        }

        public void setProperty(string property, string value, string priority)
        {
            throw new NotImplementedException();
        }

        public string removeProperty(string property)
        {
            throw new NotImplementedException();
        }

        public string azimuth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string background
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string backgroundAttachment
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string backgroundColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string backgroundImage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string backgroundPosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string backgroundRepeat
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string border
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderCollapse
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderSpacing
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderTop
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderRight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderBottom
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderLeft
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderTopColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderRightColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderBottomColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderLeftColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderTopStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderRightStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderBottomStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderLeftStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderTopWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderRightWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderBottomWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderLeftWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string borderWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string bottom
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string captionSide
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string clear
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string clip
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string color
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string content
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string counterIncrement
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string counterReset
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string cue
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string cueAfter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string cueBefore
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string cursor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string direction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string display
        {
            get { return _display; }
            set
            {
                _display = value;
                FireStyleChanged("_isVisible", _display == "none" ? false : true);
            }
        }

        public string elevation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string emptyCells
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string cssFloat
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string font
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontFamily
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontSizeAdjust
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontStretch
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontVariant
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fontWeight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int height
        {
            get { return _height; }
            set 
            {
                _height = value;
                FireStyleChanged("height", _height);
            }
        }

        public int left
        {
            get { return _left; }
            set 
            {
                _left = value;
                FireStyleChanged("left", _left);
            }
        }

        public string letterSpacing
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string lineHeight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string listStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string listStyleImage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string listStylePosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string listStyleType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string margin
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string marginTop
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string marginRight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string marginBottom
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string marginLeft
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string markerOffset
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string marks
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string maxHeight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string maxWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string minHeight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string minWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string orphans
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string outline
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string outlineColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string outlineStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string outlineWidth
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string overflow
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string padding
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string paddingTop
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string paddingRight
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string paddingBottom
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string paddingLeft
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string page
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pageBreakAfter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pageBreakBefore
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pageBreakInside
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pause
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pauseAfter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pauseBefore
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pitch
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string pitchRange
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string playDuring
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string quotes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string richness
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string right
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string size
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string speak
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string speakHeader
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string speakNumeral
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string speakPunctuation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string speechRate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string stress
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string tableLayout
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string textAlign
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string textDecoration
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string textIndent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string textShadow
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string textTransform
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int top
        {
            get { return _top; }
            set
            {
                _top = value;
                FireStyleChanged("top", _top);
            }
        }

        public string unicodeBidi
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string verticalAlign
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string voiceFamily
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string volume
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string whiteSpace
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string widows
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int width
        {
            get { return _width; }
            set
            {
                _width = value;
                FireStyleChanged("width", _width);
            }
        }

        public string wordSpacing
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int zIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                //don't need to fire event because HTMLCanvasElement only revela _style.zIndex
                FireStyleChanged("z-index", _zIndex);
            }
        }

        #endregion
    }
}

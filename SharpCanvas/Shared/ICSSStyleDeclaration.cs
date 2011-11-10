using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Shared
{
    public delegate void StyleChangedHandler(string attribute, object value);

    public interface ICSSStyleDeclaration
    {

        event StyleChangedHandler StyleChanged;
        void SuspendEvnets();
        void ResumeEvents();

        string cssText { get; set; }
        ulong length { get; }
        string item(ulong index);

        string getPropertyValue(string property);
        string getPropertyPriority(string property);
        void setProperty(string property, string value);
        void setProperty(string property, string value, string priority);
        string removeProperty(string property);

        //readonly  CSSStyleDeclarationValue values;

        //readonly  CSSRule parentRule;

        #region CSS Properties
        string azimuth { get; set; }
        string background { get; set; }
        string backgroundAttachment { get; set; }
        string backgroundColor { get; set; }
        string backgroundImage { get; set; }
        string backgroundPosition { get; set; }
        string backgroundRepeat { get; set; }
        string border { get; set; }
        string borderCollapse { get; set; }
        string borderColor { get; set; }
        string borderSpacing { get; set; }
        string borderStyle { get; set; }
        string borderTop { get; set; }
        string borderRight { get; set; }
        string borderBottom { get; set; }
        string borderLeft { get; set; }
        string borderTopColor { get; set; }
        string borderRightColor { get; set; }
        string borderBottomColor { get; set; }
        string borderLeftColor { get; set; }
        string borderTopStyle { get; set; }
        string borderRightStyle { get; set; }
        string borderBottomStyle { get; set; }
        string borderLeftStyle { get; set; }
        string borderTopWidth { get; set; }
        string borderRightWidth { get; set; }
        string borderBottomWidth { get; set; }
        string borderLeftWidth { get; set; }
        string borderWidth { get; set; }
        string bottom { get; set; }
        string captionSide { get; set; }
        string clear { get; set; }
        string clip { get; set; }
        string color { get; set; }
        string content { get; set; }
        string counterIncrement { get; set; }
        string counterReset { get; set; }
        string cue { get; set; }
        string cueAfter { get; set; }
        string cueBefore { get; set; }
        string cursor { get; set; }
        string direction { get; set; }
        string display { get; set; }
        string elevation { get; set; }
        string emptyCells { get; set; }
        string cssFloat { get; set; }
        string font { get; set; }
        string fontFamily { get; set; }
        string fontSize { get; set; }
        string fontSizeAdjust { get; set; }
        string fontStretch { get; set; }
        string fontStyle { get; set; }
        string fontVariant { get; set; }
        string fontWeight { get; set; }
        int height { get; set; }
        int left { get; set; }
        string letterSpacing { get; set; }
        string lineHeight { get; set; }
        string listStyle { get; set; }
        string listStyleImage { get; set; }
        string listStylePosition { get; set; }
        string listStyleType { get; set; }
        string margin { get; set; }
        string marginTop { get; set; }
        string marginRight { get; set; }
        string marginBottom { get; set; }
        string marginLeft { get; set; }
        string markerOffset { get; set; }
        string marks { get; set; }
        string maxHeight { get; set; }
        string maxWidth { get; set; }
        string minHeight { get; set; }
        string minWidth { get; set; }
        string orphans { get; set; }
        string outline { get; set; }
        string outlineColor { get; set; }
        string outlineStyle { get; set; }
        string outlineWidth { get; set; }
        string overflow { get; set; }
        string padding { get; set; }
        string paddingTop { get; set; }
        string paddingRight { get; set; }
        string paddingBottom { get; set; }
        string paddingLeft { get; set; }
        string page { get; set; }
        string pageBreakAfter { get; set; }
        string pageBreakBefore { get; set; }
        string pageBreakInside { get; set; }
        string pause { get; set; }
        string pauseAfter { get; set; }
        string pauseBefore { get; set; }
        string pitch { get; set; }
        string pitchRange { get; set; }
        string playDuring { get; set; }
        string position { get; set; }
        string quotes { get; set; }
        string richness { get; set; }
        string right { get; set; }
        string size { get; set; }
        string speak { get; set; }
        string speakHeader { get; set; }
        string speakNumeral { get; set; }
        string speakPunctuation { get; set; }
        string speechRate { get; set; }
        string stress { get; set; }
        string tableLayout { get; set; }
        string textAlign { get; set; }
        string textDecoration { get; set; }
        string textIndent { get; set; }
        string textShadow { get; set; }
        string textTransform { get; set; }
        int top { get; set; }
        string unicodeBidi { get; set; }
        string verticalAlign { get; set; }
        string visibility { get; set; }
        string voiceFamily { get; set; }
        string volume { get; set; }
        string whiteSpace { get; set; }
        string widows { get; set; }
        int width { get; set; }
        string wordSpacing { get; set; }
        int zIndex { get; set; }
        #endregion
    }
}

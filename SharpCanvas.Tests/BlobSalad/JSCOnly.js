
import System;
import System.Windows.Forms;
import System.Drawing;
import Accessibility;
import SharpCanvas.Host;
//import SharpCanvas.Forms;
import SharpCanvas.Browser.Forms;
import SharpCanvas.Browser.Media;
import SharpCanvas.Host.Browser;
import SharpCanvas.Media;
import SharpCanvas;
import SharpCanvas.Host.Image;
import System.Threading;
 
if(typeof(window)== 'undefined') var window = Window.Instance;
var document = new Document();
window.assign(document);
var Image = SharpCanvas.Host.Image;
var CanvasRenderingContext2D;
var IsWindows = true;

//can't compile jsc
if (typeof (navigator) == 'undefined') var navigator = { userAgent: '', appVersion: '' };

function addEvent(o, v, fu){
    o.addEventListener(v, fu, false);
}

function getMouseXY() {
    return function(e) { return { X: e.pageX, Y: e.pageY} };    
}

var windoweventkeyCode = null;
var windowevent = null;

if (typeof (setTimeout) == 'undefined') var setTimeout = function(func, timeout) { 
	window.setTimeout(func, timeout);	
	return 1; 
};
if (typeof (clearTimeout) == 'undefined') var clearTimeout = function(timeoutID) { return; };

var Agent = {isIE: false};
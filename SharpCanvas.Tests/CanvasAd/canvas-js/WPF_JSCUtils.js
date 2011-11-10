
import System;
import System.Windows;
import PresentationFramework;
import PresentationCore;
import WindowsBase;
import SharpCanvas.Host;
import SharpCanvas.Browser.Media;
import SharpCanvas.Host.Prototype;
import SharpCanvas.Host.Image;
import SharpCanvas.Interop;
import SharpCanvas.Media;
import SharpCanvas;
import System.Threading;
import System.Diagnostics;

MessageBox.Show("connect now");

if(typeof(window)== 'undefined') var window = Browser.Instance.Window;
var document = window.document;//new Document();
//window.assign(document);
var Image = SharpCanvas.Host.Image;
var CanvasRenderingContext2D;
var Agent = {
	isWindows: true
};

//can't compile jsc
if (typeof (navigator) == 'undefined') var navigator = { userAgent: 'Windows', appVersion: '' };

function addEvent(o, v, fu){
    o.addEventListener(v, fu, false);
}

function getMouseXY(dx, dy) {
    return function(e) { 
		if(e.pageX || e.pageX === 0){
		    return { X: e.pageX - dx, Y: e.pageY - dy};
		}
		else{
		    return {X: 0, Y: 0};
		}
	};    
}

var emptyFn = function() { };

//var Debug = {WriteLine: emptyFn};
var console = {
	log: function(msg){
		Debug.WriteLine(msg);
	}
};
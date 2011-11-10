
import System;
import System.Windows.Forms;
import System.Drawing;
import System.Diagnostics;
import Accessibility;
import SharpCanvas.Host;
import SharpCanvas.Host.Browser;
import SharpCanvas.Browser.Forms;
import SharpCanvas.Host.Prototype;
import SharpCanvas.Host.Image;
import SharpCanvas.Interop;
import SharpCanvas.Forms;
import SharpCanvas;
import System.Threading;

MessageBox.Show("car ad is loading...");
var Image = SharpCanvas.Host.Image;

function startPoint(window, document) {

var CanvasRenderingContext2D;
var Agent = {
	isWindows: true,
	isIE: false
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

var console = {
	log: function(msg){
		Debug.WriteLine(msg);
	}
};

var commit = function(ctx){
	if(ctx.commit){
		ctx.commit();
	}
};
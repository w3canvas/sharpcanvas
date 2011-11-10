import System;
import System.Windows.Forms;
import System.Drawing;
import SharpCanvas.Host;
import SharpCanvas.Host.Browser;
import SharpCanvas.Host.Prototype;
import SharpCanvas.Interop;

import SharpCanvas.Browser.Forms;//need this because of SharpCanvas.Host.Standalone implementation

MessageBox.Show("connect now");

if(typeof(window)== 'undefined') var window = Browser.Instance.Window;
var document = window.document;
window.globalScope = this;

function addEvent(o, v, fu){
    o.addEventListener(v, fu, false);
}

// function getLoadAdScript(adUrl){
	// return function(){
		// this.loadAssembly(adUrl);
	// };
// }

function makeIframeNode(attribs) {
	    var el = document.createElement("iframe");
	    for (var a in attribs) {
			el.setAttribute(a, attribs[a]);
	    }

	    return el;
}

function makeAd(config){
	var doc = document;
	var iframe = makeIframeNode({
		name: "canvas_ads_frame",
		width: config.canvas_ad_width,
		height: config.canvas_ad_height,
		canvas_ad_client: config.canvas_ad_client,
		//todo: add color and size parameters for the ad
		//todo: where can I get this path?
		src: 'c:\\Personal\\SharpCanvas\\SharpCanvas.Tests\\CanvasAd\\carad.dll'
	});
	//var container = doc.getElementById(config.canvas_container) || doc.body; 
	//todo: investigate layout problem and dynamic user controls loading if necessary.
	var container = doc.body; 
	container.appendChild(iframe);
}

Browser.Instance.Window.onload = (function() {
	var config = {
		//path to specific ad
		//canvas_ad_client: 'c:\\car.dll',
		canvas_ad_width: 120,
		canvas_ad_height: 60,
		canvas_color_border: '',
		canvas_color_bg: '',
		canvas_color_fill: '',
		canvas_color_text: ''
	};
	return function(){
		makeAd(config);
	}
}());

Application.Run(Browser.Instance);
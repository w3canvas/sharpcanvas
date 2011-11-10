function makeAd(){
	var $W = 1 * gup('canvas_ad_width');
	var $H = 1 * gup('canvas_ad_height');
	var bgColor = '#' + gup('canvas_color_bg');
	var color = '#' + gup('canvas_color_border');
	var suite = new CanvasAd($W, $H, bgColor, color);
}
//start application when main window is loaded
Browser.Instance.Window.onload = function() {
    //Browser.Instance.Window.src='http://localhost/canvasad/pagead.html?f=1&canvas_ad_client=car&canvas_ad_width=600&canvas_ad_height=120&canvas_color_border=334455&canvas_color_bg=FF6688&canvas_color_fill=FF6663&canvas_color_text=F77FA0';
	
	//var suite120x600 = new CanvasAd(120, 600, '#99EFF8', '#000021', 0, 170);
	var suite600x120 = new CanvasAd(600, 120, '#FFE228', '#000021', '#FFFFFF', 0, 0);
	// var suite125x125 = new CanvasAd(125, 125, '#55E558', '#000021', 750 - 125 - 20 - 125, 150);
};
var app = new App();
app.Run(Browser.Instance);
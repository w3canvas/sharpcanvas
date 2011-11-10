function addEvent(o, v, fu) {
    if(Agent.isIE){
		o.attachEvent('on' + v, fu, false);		
	}
	else{		
		o.addEventListener(v, fu, false);
	}
}

 
// //for browser only
// var windoweventkeyCode = window.event ? window.event.keyCode : null;
// var windowevent = window.event;

var Agent;

(function() {

	Agent = { };

	var o = navigator.userAgent, ver = navigator.appVersion, n = parseFloat(ver);

	var idx = Math.max(ver.indexOf("WebKit"), ver.indexOf("Safari"), 0);

	if(idx) Agent.isSafari = parseFloat(ver.split("Version/")[1]) || ((parseFloat(ver.substr(idx + 7)) >= 419.3) ? 3 : 2) || 2;

	Agent.isOpera = (o.indexOf("Opera") >= 0) ? n : false;
	Agent.isKhtml = (ver.indexOf("Konqueror") >= 0 || Agent.isSafari) ? n : false;
	Agent.isMoz = (o.indexOf("Gecko") >= 0 && !Agent.isKhtml) ? n : false;
	Agent.isFF = Agent.isIE = false;

	if(Agent.isMoz) Agent.isFF = parseFloat(o.split("Firefox/")[1]) || false;

	if(document.all && !Agent.isOpera) Agent.isIE = parseFloat(ver.split("MSIE ")[1]) || false;
	
	Agent.isMac = (o.indexOf('Mac') >= 0);
	Agent.isWin = (((o.indexOf('Win') >= 0) || (o.indexOf('NT') >= 0) ) && !Agent.isMac) ? true : false;
	Agent.isLinux = (o.indexOf('Linux') >= 0);
	Agent.isWindows = o == "Windows";

})();
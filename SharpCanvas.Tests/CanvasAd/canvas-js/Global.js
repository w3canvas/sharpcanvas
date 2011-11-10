var $, $T, $S, $2D, setAttributes;
var $W = 800, $H = 600;
var $stop, $time; // for running filters
var $w, $h, $url, $image; // image properties

var loader; // Load screen

var $left = 0; var $top = 0;
//var screen = { width: $W, height: $H };

$ = function(v, o) { // ELEMENT

	return (typeof o == 'object' ? o : document).getElementById(v);
	
};

$T = function(v, o) { // TAG

	return (o ? typeof o == 'object' ? o : $(o) : document).getElementsByTagName(v);

};
//return style property for a given object
$S = function(o, r) { // STYLE

    var o = (typeof o == 'object' ? o : $(o));

    if (r) for (var i in r) o.style[i] = r[i];

    return o.style;

};

$2D = function(o) { // CANVAS

	return (typeof o == 'object' ? o : $(o)).getContext('2d');
	
};

setAttributes = function(elm, attrs) {

	for(var attr in attrs) elm.setAttribute(attr, attrs[attr]);

	return elm;

};

// String.prototype

String.prototype.replaceAll = function(a, b) { // Replace all occurrences of the search string with the replacement string

	return this.split(a).join(b);
	
};

String.prototype.trim = function(v) { // Strip whitespace (or other characters) from the beginning and end of a string

	return this.replace(/^\s+|\s+$/g, "");
	
};

String.prototype.ucwords = function(v) { // Uppercase the first character of each word in a string

	return this.replace(/^(.)|\s(.)/g, function($1) { return $1.toUpperCase(); });

};

String.prototype.ucfirst = function(v) { // Make a string's first character uppercase

	return this[0].toUpperCase() + this.substr(1);
	
};

// Cookies

//- http://api.dojotoolkit.org/jsdoc/dojo/HEAD/dojo.cookie
//- http://plugins.jquery.com/files/jquery.cookie.js.txt
var Cookie, props;

Cookie = {};

Cookie = function(name, value, options) {

    var c = document.cookie;

    if (typeof (value) == 'undefined') {

        // var matches = c.match(new RegExp("( ? :^|; )" + dojo.regexp.escapeString(name) + " = ([^;]*)"));
        var matches = [];

        return matches ? decodeURIComponent(matches[1]) : null;

    }
    else {

        props = props || {};

        var exp = props.expires;

        if (typeof exp == "number") {

            var d = new Date();

            d.setTime(d.getTime() + exp * 24 * 60 * 60 * 1000);

            exp = props.expires = d;

        }

        if (exp && exp.toUTCString) { props.expires = exp.toUTCString(); }

        value = encodeURIComponent(value);

        var updatedCookie = name + " = " + value;
        var propName;

        for (propName in props) {

            updatedCookie += "; " + propName;

            var propValue = props[propName];

            if (propValue !== true) { updatedCookie += " = " + propValue; }

        }

        document.cookie = updatedCookie;

    }
};

Cookie.isSupported = function() {

    /*	if(!("cookieEnabled" in navigator)) {
	
		this("__djCookieTest__", "CookiesAllowed");

		navigator.cookieEnabled = this("__djCookieTest__") == "CookiesAllowed";

		if(navigator.cookieEnabled) this("__djCookieTest__", "", { expires: -1 });

	}
*/
    navigator.cookieEnabled = false;
    return navigator.cookieEnabled;

};

// Page Query
var PageQuery;

PageQuery = function(v) {

	var o = window.location.search, n = o.indexOf(v);

	if(n == -1) return false;
	
	var len = v.length, q = (o.substr(n + len) ? o.substr(n + len) : null).split('&');

	var z = { }, v = '';
	
	for(var i in q) {
	
		v = q[i].split(' = ');
		
		z[v[0]] = v[1];
		
	}
	
	return z;
	
};

// Image <-> Base64
var image2data, data2image;

image2data = function(url) {

	var image = new Image(); image.src = url;

	image.onload = function() {

		var o = document.createElement('Canvas');
		
		o.width = image.width;
		o.height = image.height;

		var o2D = o.getContext('2d');

		o2D.drawImage(image, 0, 0);

		TEST(o.toDataURL())

	};
};

data2image = function(url) {

	var image = new Image();
	
	image.src = url;
	
	return image;
	
};

// Date Functions
var now;
now = function() {

	return (new Date()).getTime();

};

// Coordinates
var XY = function(e) { return { X: e.pageX, Y: e.pageY }; };	

// Objects
var clone, mixin;
clone = function(o) {

	if(typeof o != 'object') return(o);

	if(o == null) return(o);
	
	var z = (typeof o.length == 'number') ? [ ] : { };

	for(var i in o) z[i] = clone(o[i]);

	return z;
	
};

mixin = function(o, o1, o2, o3) {
    var j;
    if (typeof (o1) != 'undefined') for (j in o1) o[j] = o1[j];
    if (typeof (o2) != 'undefined') for (j in o2) o[j] = o2[j];
    if (typeof (o3) != 'undefined') for (j in o3) o[j] = o3[j];

    return o;

};

var isArray, isEmpty; // FIXME: Use instance of
isArray = function(o) {

	return Object.prototype.toString.call(o) === '[object Array]';

};

isEmpty = function(o) { 

	for(var i in o) return false; 
	
	return true; 

};

// Debugging

var TEST, Speed_TEST;
TEST = function(v, s) {

	var o = document.getElementById('T'), z1 = '';

	if(typeof v == 'object') {
	
		var b = v && v.length ? ['[',']'] : ['{','}'];

		for(var i in v) {

			if(typeof v[i] == 'object') {

				var c = v[i] && v[i].length ? ['[',']'] : ['{','}'], z2 = '';

				for(var j in v[i]) z2 += (c[0] == '[' ? '': j +': ')+ (typeof v[i][j] == 'string' ? "'"+ v[i][j] +"'" : v[i][j]) +', ';

				z1 += i+': '+c[0]+' '+ z2.substr(0, z2.length-2) +' '+c[1]+', ';

				continue;

			}

			z1 += i + ': ' + v[i] + ', '; // arrays

		}
		
		v = b[0]+' '+ z1.substr(0, z1.length-2) +' '+b[1];

	};

	v = s == 1 ? '<br>'+ v : v;

	o.innerHTML = s ? o.innerHTML + v : v;

};

Speed_TEST = function(a,b,n) { var T1, T2, Z1, Z2;
	
	T1 = now(); for(var i = 0; i<= n; i++) { a(); }; Z1 = now()-T1;
	
	T2 = now(); for(var i = 0; i<= n; i++) { b(); }; Z2 = now()-T2;
	
	TEST('A: '+Z1+', B: '+Z2+' = '+(Math.round(Z2/Z1*1000)/1000), 1);

};

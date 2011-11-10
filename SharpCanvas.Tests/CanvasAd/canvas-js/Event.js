var Event2D;
var Key;

(function() {

    var fix = function(v) { // fix any browser discrepancies

        if (v == 'mousewheel' && Agent.isFF) return 'DOMMouseScroll';

        return v;

    };

    Event2D = {};

    Event2D.add = function(o, v, fu) {
        addEvent(o, v, fu);
    };

    Event2D.remove = function(o, v, fu) {
        v = fix(v);
        if (!Agent.isIE) o.removeEventListener(v, fu, false);
        else { // IE
            o.detachEvent(v, o[v + fu]);
            o["e" + v + fu] = null;
            o[v + fu] = null;
        }
    };
})();

// KEY EVENTS
(function() {

    // keys that don’t stream onkeypress (keyCodes)

    var stream = { 8: 'delete', 13: 'enter', 37: 'left', 39: 'right', 38: 'up', 40: 'down', 33: 'pageUp', 34: 'pageDown', 9: 'tab' };

    // modifies other keys when used in combination (keyCodes)

    var modifier = { 120: 'capsLock', 144: 'numLock', 91: 'start', 224: 'command', 27: 'control', 18: 'alt', 27: 'escape', 16: 'shift', 45: 'insert', 38: 'home', 35: 'end', 13: 'enter' };

    // modifiers that alters the keys’ charCode (keyCodes)

    var alters = { 16: 'shift', 144: 'numLock', 120: 'capsLock' };

    // keys that don’t normally work onkeydown or onkeyup (charCodes)

    var zero = { 95: '_', 124: '|', 58: ':', 62: '>', 60: '<', 63: '?', 128: '~' };

    // maps keyCodes to corresponding charCodes (caches as you type)

    var code = {}; // e.g.: code[65] = { default: 97, shift: 38, alt: 144, alt_shift: 4302 };

    // current keyCode (used to build map between onmousedown -> onmousepress)

    var id;

    // TRACKING

    Key = { meta: false, shift: false, alt: false };

    Key.clear = function() { // clear onkeypress

        if (Key.interval) window.clearInterval(Key.interval);

    };

    Key.core = function(e, k) { // runs during if(stream[k]) onkeydown else onkeypress

        if (Key.down) { e.preventDefault(); return false; } // prevent further execution of onkeypress, run internally

        Key.k = k; Key.down = true; // update key

        Key.listener(e, 'down', k); // run onkeydown

        if (Key.interval) Key.clear(); // switch key mid-steam

        Key.interval = window.setInterval(function() { // setup keypress stream

            if ((now() - Key.time) > 400) Key.listener(e, 'press', k) // run onkeypress

        }, 100);

        Key.time = now(); // delay before stream

    };

    Key.down = function(e) {

        var k = id = e.keyCode;

        Key.down = false;

        if (k == 0) return false; // no worries, fixed onkeypress

        if (!code[id]) code[id] = {}; // create mappings that don’t exist

        if (modifier[k]) { // cache modifier

            if (!alters[k]) Key.clear(); // stop any keypress’s still running

            Key[modifier[k]] = true;

            if ((k == 224 && Agent.isMac) || (k == 27 && !Agent.isMac)) Key.meta = true;

            return false;

        }

        if (stream[k]) Key.core(e, k); // fix keys that don’t stream onkeypress

    };

    Key.press = function(e) {

        var k = e.charCode || e.keyCode;

        if (k == 0) return false; //- check into this

        var lower = (k >= 65 && k <= 90), upper = (k >= 97 && k <= 122); // detect capsLock

        if (lower || upper) Key.capsLock = (lower && !Key.shift) || (upper && Key.shift) ? true : false;

        if (zero[k]) zero.id = k; // keep track of keys that don’t work with onkeyup

        code[id][Key.shift ? Key.alt ? 'alt_shift' : 'shift' : (Key.alt ? 'alt' : 'default')] = k; // mapping to convert keycode -> keychar

        Key.core(e, k);

    };

    Key.up = function(e) {

        var k = e.keyCode;

        if (k == 0 && zero.id) k = zero.id, zero.id = null; // gets value from onkeypress

        if (modifier[k]) { // cache modifier

            if (!alters[k]) Key.clear(); // stop any keypress’s still running

            Key[modifier[k]] = false;

            if ((k == 224 && Agent.isMac) || (k == 27 && !Agent.isMac)) Key.meta = false;

            return false;

        }

        if (code[k] && !stream[k]) k = code[k][Key.shift ? Key.alt ? 'alt_shift' : 'shift' : (Key.alt ? 'alt' : 'default')];

        Key.listener(e, 'up', k); // run onkeyup (forwards key combo’s to attached functions)

        if ((k == Key.k || k == 18 || k == 16)) Key.clear(); // cancel key steaming if k == (alt, shift, or Key.k)

    };

    // LISTENER

    Key.active = {};

    Key.listener = function(e, m, k) {

        var f = null; // listen for key combos with functions attached

        var meta = Key.meta, shift = Key.shift, alt = Key.alt;

        if (meta && shift && (f = Key.active[k + '_meta_shift'])) { if (!f(e, m, k)) e.preventDefault(); }

        else if (meta && alt && (f = Key.active[k + '_meta_alt'])) { if (!f(e, m, k)) e.preventDefault(); }

        else if (meta && (f = Key.active[k + '_meta'])) { if (!f(e, m, k)) e.preventDefault(); }

        else if (shift && (f = Key.active[k + '_shift'])) f(e, m, k);

        else if (alt && (f = Key.active[k + '_alt'])) f(e, m, k);

        else if (1 && (f = Key.active[k])) f(e, m, k);

        else if (1 && (f = Key.active['*'])) f(e, m, k);

    };

})();


// MOUSE EVENTS

function Mouse(mouse2D, dx, dy) {

    //var XY = function(e) { return { X: e.pageX, Y: e.pageY} };
	
    /* can't compile jsc */
    var XY = getMouseXY(dx || 0, dy || 0);

    var f = null, m = 'up', oXY, id; // function, mouse id, origin XY, hex id
    oXY = { X: 0, Y: 0 };

    var cloneXY = function() { // so that oXY cannot be modified

        return { X: oXY.X, Y: oXY.Y };

    };

    this.move = function(e) {
        if(typeof(XY) != 'function'){
			return;
		}
		this.id = '';
        var o = XY(e), r;
        r = { data: [] };

        if (f && f.drag && (m == 'drag' || m == 'down')) { // MouseDrag
			f.drag.call(f.scope, cloneXY(), o, m = 'drag', e);
            return false;

        }

        o.X = clamp(o.X, 0, $W);
        o.Y = clamp(o.Y, 0, $H);

        try {

            if (o.X < 0 || o.Y < 0 || !(r = mouse2D.getImageData(o.X, o.Y, 1, 1))) return; // outside bounds

        }
        catch (err) {

            return; // only getting this bug when closing firebug

        }

        var r = r.data, hex = r[0] << 16 | r[1] << 8 | r[2];

        if ((f = mice[hex]) && r[3] == 255) {

            if (f && f.over) f.over.call(f.scope, o, o, 'over', e); //- MouseOver

            id = hex; // cache hex
            //$S('active').cursor = f.cursor; // change cursor

        }
        else if (id && hex == 0) {           
		   f = mice[id];

            id = null; // unset hex

            if (f && f.out) f.out.call(f.scope, o, o, 'out', e); // MouseOut

            //$S('active').cursor = 'default';

        }

        if (f) this.id = f.id;

        if (f && f.move) f.move.call(f.scope, o, o, 'move', e); // MouseMove

        m = 'move';

    };
	
    this.dblclick = function(e) {

        oXY = XY(e);
		if(oXY.X < 0 || oXY.Y < 0)//outside of the bounds
			return;
			
        //get funct
        r = mouse2D.getImageData(oXY.X, oXY.Y, 1, 1);
        var r = r.data, hex = r[0] << 16 | r[1] << 8 | r[2];
        f = mice[hex];
        //

        if (f && f.dblclick) f.dblclick.call(f.scope, cloneXY(), cloneXY(), 'dblclick', e);

        m = 'dblclick';

    };

    this.down = function(e) {
        oXY = XY(e);
		if(oXY.X < 0 || oXY.Y < 0)//outside of the bounds
			return;
        
        //get funct
        r = mouse2D.getImageData(oXY.X, oXY.Y, 1, 1);
        var r = r.data, hex = r[0] << 16 | r[1] << 8 | r[2];
        f = mice[hex];

        this.time = now(); // timestamp

        if (f && f.down) f.down.call(f.scope, cloneXY(), cloneXY(), 'down', e, hex);

        m = 'down';
        if(e.preventDefault) e.preventDefault();
    };

    this.up = function(e) {
        var o = XY(e);
		if(oXY.X < 0 || oXY.Y < 0)//outside of the bounds
			return;
			
        //get funct
        r = mouse2D.getImageData(o.X, o.Y, 1, 1);
        var r = r.data, hex = r[0] << 16 | r[1] << 8 | r[2];
        f = mice[hex];

        if (f && f.drop && m == 'drag') f.drop.call(f.scope, cloneXY(), o, 'drop', e, hex); // MouseDrop

        if (f && f.up) f.up.call(f.scope, cloneXY(), o, 'up', e); // MouseUp

        m = 'up';

    };

    this.menu = function(e) {
        var o = XY(e);
		if(o.X < 0 || o.Y < 0)//outside of the bounds
			return;
			
        //get funct
        r = mouse2D.getImageData(o.X, o.Y, 1, 1);
        var r = r.data, hex = r[0] << 16 | r[1] << 8 | r[2];
        f = mice[hex];
        //

        if (f && f.menu) f.menu.call(f.scope, XY(e), m = 'menu', e); // MouseMenu

        m = 'menu';

    };

    var delta = 0; // mouse wheel accumulative

    this.wheel = function(e) {
        var v = 0;
        var o = XY(e);
		if(o.X < 0 || o.Y < 0)//outside of the bounds
			return;
			
        //get funct
        r = mouse2D.getImageData(o.X, o.Y, 1, 1);
        var r = r.data, hex = r[0] << 16 | r[1] << 8 | r[2];
        f = mice[hex];
        //

        if (f && f.wheel) {

            if (m != 'wheel') { // reset variables

                oXY = XY(e);

                delta = 0;

            }

            if (e.wheelDelta) { // OPERA + IE

                var v = e.wheelDelta / 120;

                if (Agent.isOpera) v = -v;

            }
            else if (e.detail) { // MOZ

                var v = -e.detail / 3;

            }

            f.wheel.call(f.scope, delta += v, m = 'wheel', e); // MouseWheel

        }
    };

    var mice = {}; // comtains Mouse objects

    var exists = {}; // contains hex’s

    this.compile = function(o) {

        if (exists[o.id]) return exists[o.id];

        var hex = Color.random(), r = (mice[hex] = {});
        //copy all properties to newly created object
        //thats why we can't assign/reassign event handlers after mouse compilation
        for (var i in o) r[i] = o[i];

        if (!r.cursor) r.cursor = 'default';

        return (exists[o.id] = hex);

    };

};
/*
	Canvas2D : v0.1 : 2008.08.05
	add, remove, loader
*/
var Canvas;
if (typeof (setTimeout) == 'undefined') var setTimeout = function(func, timeout) { func(); return 1; };
if (typeof (clearTimeout) == 'undefined') var clearTimeout = function(timeoutID) { return; };

(function() {

    try {
        if (typeof (CanvasRenderingContext2D) == 'undefined')
        //Paul: window guarantee that CanvasRenderingContext2D will be initialized
            CanvasRenderingContext2D = window.CanvasRenderingContext2D; // || { prototype: document.createElement('canvas').getContext('2d').__proto__ };
    } catch (e) {
        // Canvas support has not been loaded.
    }

    // stores Canvas2D references
    var exists = {};

    // Global functions
    Canvas = {

        add: function(w, h, id, style, callback, context) {
            if (exists[id]) {
                callback.apply(context, [exists[id]]);
            }
            var d;
            if (Agent.isIE) {
                //custom:canvas tag should be registered on the page
                d = document.createElement("custom:canvas");
            }
            else {                
                d = document.createElement('Canvas');
				
            }
            d.id = id;
            //hack for winform and browser compatibility
            d.Identifier = id;
            d.width = w;
            d.height = h;

            if (style) d.setAttribute('style', style);
            document.body.appendChild(d);
            if (!Agent.isIE) {
				//if (style) d.setAttribute('style', style);
                exists[id] = d.getContext('2d');
                callback.apply(context, [exists[id]]);
            }
            else {                
                d.init = function() {
					//alert('init');
                    if (style) d.setAttribute('style', style);
					exists[id] = d.getContext('2d');
                    callback.apply(context, [exists[id]]);                    
                };
            }
        },

        remove: function(id) {

            if (!exists[id]) return;

            delete exists[id]; // delete reference

            document.body.removeChild($(id)); // remove DOM

        },

        // customizable animated loading indicator
        loader: function(bars, center, innerRadius, size, rgb) {
            var runID = now(), offset = 0;
            var W = (innerRadius + size.H), X = center.X - W, Y = center.Y - W; W *= 2;

            function draw(c, offset) {

                c.save();
                c.clearRect(X, Y, W, W);
                c.translate(center.X, center.Y);
                for (var i = 0; i < bars; i++) {

                    var cur = (offset + i) % bars;

                    var angle = 2 * cur * Math.PI / bars;

                    c.save();
                    c.translate(innerRadius * Math.sin(-angle), innerRadius * Math.cos(-angle));
                    c.rotate(angle);
                    c.fillStyle = "rgba(" + rgb + ',' + (bars + 1 - i) / (bars + 1) + ")";
                    c.fillRect(-size.W / 2, 0, size.W, size.H);
                    c.restore();

                }
                c.restore();
            };

            // function run() {
                // if (!runID) return;
                // draw(active2D, offset = (offset + 1) % bars);
                // setTimeout(run, 50);
            // };

            // return {
                // stop: function() { runID = 0; active2D.clearRect(X, Y, W, W); },
                // start: function() { runID = now(); run(); }
            // };
			return null;
        }
    };

})();
var PI_2, RAD_DEG, DEG_RAD;
var clamp, range;
var latestMouse;
var Random;
var Convert;

(function() {

    PI_2 = Math.PI * 2; // PI * 2

    RAD_DEG = 180 / Math.PI; // Radians to Degrees

    DEG_RAD = 1 / RAD_DEG; // Degrees to Radians

    clamp = function(n, min, max) {

        return (n < min) ? min : ((n > max) ? max : n);

    };

    range = function(a, b, step) { // create range of numbers

        if (typeof (b) == 'undefined') { b = a, a = 0; }

        var s = step || 1, range = [];

        if (s > 0) for (var i = a; i < b; i += s) range.push(i);

        else if (s < 0) for (var i = a; i > b; i += s) range.push(i);

        return range;

    };

    // Park Miller (1988) "minimal standard" linear congruential pseudo-random number generator.

    (Random = function(seed) {

        this.seed = (typeof seed == 'number') ? seed : Math.random() * 2147483648 >> 0;

        this.n = Number(this.seed);

        return this;

    }).prototype = {

        'int': function() { // unsigned integer (31 bits)

            return this.n = (this.n * 16807) % 2147483647;

        },
        'double': function() { // double between nearly 0 and nearly 1.0

            return (this.n = (this.n * 16807) % 2147483647) / 2147483647;

        },
        int_range: function(min, max) { // unsigned integer (31 bits) between a given range

            min -= 0.4999; max += 0.4999;

            return Math.round(min + ((max - min) * ((this.n = (this.n * 16807) % 2147483647) / 2147483647)));

        },
        double_range: function(min, max) { // double between a given range

            return min + ((max - min) * ((this.n = (this.n * 16807) % 2147483647) / 2147483647));

        }
    };

    // Browsers, and most programs, default to 96ppi — We're using this as the default

    //- Math.sqrt(Math.pow(1440, 2) + Math.pow(900, 2)) / 15.4  ——  perhaps allow user to enter these?

    //- '100%':0, 'xx-small':0, 'x-small':0, 'small':0, 'medium':0, 'large':0, 'x-large':0, 'xx-large':0 // should work with these...

    Convert = {};

    Convert = function(o, j) {

        for (var i in o) {

            var v = o[i];

            if (typeof v == 'object') {

                Convert(v, i);

                continue;

            }

            if (j) {

                this['PX_' + i] = this['PX_' + j] * this[j + '_' + i];
                this[j + '_' + v] = this[j + '_' + i] * this[i + '_' + v];
                this[j + '_PX'] = 1 / this['PX_' + j];
                this[v + '_' + j] = 1 / this[i + '_' + v] * 1 / this[j + '_' + i];
                this[i + '_' + j] = 1 / this[j + '_' + i];

            }

            this['PX_' + v] = this['PX_' + i] * this[i + '_' + v];
            this[i + '_PX'] = 1 / this['PX_' + i];
            this[v + '_PX'] = 1 / this['PX_' + i] * 1 / this[i + '_' + v];
            this[v + '_' + i] = 1 / this[i + '_' + v];

        }
    };

    Convert.PX_PT = 1 / 1.3333; // Points
    Convert.PT_PC = 1 / 12.0; // Picas
    Convert.PX_IN = 1 / 96; // Inches
    Convert.IN_FT = 1 / 12.0; // Feet
    Convert.FT_YD = 1 / 3.0; // Yards
    Convert.PX_MM = 25.4 * Convert.PX_IN; // Millimeter
    Convert.MM_CM = 1 / 10.0; // Centimeter
    Convert.CM_M = 1 / 100.0; // Meter
    Convert.PX_EX = 1 / 8.3667; // Ex
    Convert.EX_EM = 1 / Convert.PX_EX / 16; // Em

    Convert({ PT: 'PC', MM: { CM: 'M' }, IN: { FT: 'YD' }, EX: 'EM' });

})();


var Color; // Fixme: Package and Move
(function() {

    var rand = null;

    Color = {};

    Color.random = function() {

        if (!rand) rand = new Random();

        return rand['double']() * 0xFFFFFF >>> 0;

    };

    Color.HEX_STRING = function(o) {

        var z = o.toString(16), n = z.length;

        while (n < 6) { z = '0' + z; n++; }

        return z;

    };

})();
var idn = 0;
function CanvasAd(width, height, bgColor, color, textColor, px, py){
	//init and declare global variables
    this.$W = width;
    this.$H = height;
	this.$X = px || 0;
	this.$Y = py || 0;
	this.angle = Math.PI / 40;
	var $D = 1 * Math.min(this.$W, this.$H);
	this.ad2D = null;
	this.mouse2D = null;
	this.dragXY = null;
	this.mouseColor = "";
	this.mouse = null;
	//hashtable for active animations
	var textAnimation = 0;
	//canvas creation callbacks
	this.mouseCallback = function(ctx) { this.mouse2D = ctx; this.go(); };
	this.adCallback = function(ctx) { this.ad2D = ctx; this.go(); };
	this.textCallback = function(ctx) { this.text2D = ctx; this.go(); };

	this.initComponent = function(){
		Canvas.add(this.$W, this.$H, "mouseCanvas" + idn, "display:none; top:" + this.$Y + "px;" + "left:" + this.$X + "px;",
				   this.mouseCallback, this); // mouse
		Canvas.add(this.$W, this.$H, "adCanvas" + idn, "z-index: 2; position:absolute; top:" + this.$Y + "px;" + "left:" + this.$X + "px;",
					this.adCallback, this); //adsense
		Canvas.add(this.$W, this.$H, "textCanvas" + idn++, "z-index: 3; position:absolute; top:" + this.$Y + "px;" + "left:" + this.$X + "px;",
				this.textCallback, this); //adsense
	};
	
	this.initMouse = function(){
		this.mouse = new Mouse(this.mouse2D, this.$X, this.$Y);
		var btn = { id: "car-body" + idn, cursor: "crosshair", 
			down: this.startDrag, move: this.doDrag, up: this.stopDrag, over: this.onMouseOver, out: this.onMouseOut, container: this, scope: this };
		this.hex = this.mouse.compile(btn);
		this.mouse2D.fillStyle = "#000000";
		this.mouse2D.fillRect(0, 0, this.$W, this.$H);
		this.mouseColor = "#" + Color.HEX_STRING(this.hex);
		this.mouse2D.fillStyle = this.mouseColor;
		//bind events
		Event2D.add(window.document, "mousedown", this.mouse.down); // mouse down
		Event2D.add(window.document, "mouseup", this.mouse.up); // mouse up
		Event2D.add(window.document, "mousemove", this.mouse.move); // mouse move
		//Event2D.add(window.document, "mouseover", this.mouse.move); // mouse move
		//Event2D.add(window.document, "mouseout", this.mouse.move); // mouse move
	};
				
	this.go = function (){
		if(this.mouse2D && this.ad2D && this.text2D){
			if(Agent.isIE || Agent.isWindows){
				this.text2D = this.ad2D;
			}
			this.initMouse();
			//calc scale factor
			var cW = 550;
			var cH = 280;
			this.scale = 1;
			if(this.$H >= this.$W){
				this.scale =  this.$W / cW;
			}
			else{
				this.scale = this.$H / cH;
			}
			this.renderCar(0, 0);
		}
	};
	
	this.onMouseOut = function(){
		this.removeText();
	};
	
	this.onMouseOver = function(){
		//if lavel is not visible and we"re in not a drag action
		if(!this.dragLabelVisible && !this.dragXY){
			this.dragLabelVisible = true;
			var x = this.carXY.X + 60 * this.scale;
			var y = this.carXY.Y + 120 * this.scale;
			this.startTextAnimation(this.text2D, "Drag to drive", x, y, 2000, textColor, Math.round(70 * this.scale) + "px Arial");
		}
	};
	
	this.removeText = function(){
		var ctx = this.text2D;
		this.stopAnimation(ctx);
		ctx.clearRect(0, 0, this.$W, this.$H);
		this.refreshCar();
		this.dragLabelVisible = false;
	};
	
	this.startTextAnimation = function(ctx, text, x, y, duration, color, font){
		//if animation is not in action
		if(!textAnimation){
			ctx.fillStyle = color;
			ctx.font = font;
			ctx.globalAlpha = 0;
			//increase alpha on value d
			var d = 100 / duration;
			//start animation
			var sa = this.stopAnimation;
			var that = this;
			var scale = this.scale;
			var animation = function(){
				if(duration > 0){
					ctx.globalAlpha = ctx.globalAlpha + d;
					ctx.fillText(text, x, y);
					commit(ctx);
				}
				else{
					if(typeof(sa) == "function")//workaround for standalone env.
						sa.call(that, ctx);
				}
				duration = duration - 100;
			};
			if(!Agent.isWindows || true){//TODO: remove true
				textAnimation = window.setInterval(animation, 100);
			}
			else{
				ctx.globalAlpha = 1 - d;
				animation();
				duration = 0;
				textAnimation = 0;
			}
			
		}
	};
	
	this.stopAnimation = function(ctx){
		if(textAnimation){
			//stop animation
			window.clearInterval(textAnimation);
			//remove animation id from active animations map
			textAnimation = 0;
		}
	};
	
	this.startDrag = function(coord, o, m, e, hex){
		this.dragXY = coord;
		this.removeText();
	};

	this.stopDrag = function(){
		this.dragXY = null;
	};
	
	this.refreshCar = function(){
		this.renderCar(this.carXY.X, this.carXY.Y);
	};

	this.doDrag = function(coord){
		if(this.dragXY){
			var dx = coord.X - this.dragXY.X;
			var dy = coord.Y - this.dragXY.Y;
			this.renderCar(this.carXY.X + dx, this.carXY.Y + dy);
			this.dragXY = coord;
		}
	};
	
	this.renderCar = function(x, y){
		this.clearAd();
		this.clearMouse();
		var path = (x + 504 * this.scale) / this.$W;
		if(this.$W <= this.$H){
			path = (y + 180 * this.scale) / this.$H;
		}
		//draw broken car after 75% of the path
		var isBroken = path > .75;
		//draw the car
		this.drawCar(x, y, this.ad2D, false, isBroken, this.wasBroken);
		//fill car area on mouse surface
		this.drawCar(x, y, this.mouse2D, true, isBroken, this.wasBroken);
		if(isBroken && !this.wasBroken){
			this.wasBroken = true;
		}
		else if(!isBroken && this.wasBroken){
			this.wasBroken = false;
		}
		this.carXY = {X : x, Y: y};
	};

	this.clearAd = function(){
		this.ad2D.clearRect(0, 0, this.$W, this.$H);
		//draw background
		this.ad2D.fillStyle = bgColor;//"#" + gup("canvas_color_bg");
		this.ad2D.strokeStyle = color;//"#" + gup("canvas_color_border");
	};

	this.clearMouse = function(){
		this.mouse2D.clearRect(0, 0, this.$W, this.$H);
		this.mouse2D.fillStyle = this.mouseColor;
	};

	this.drawCar = function(x, y, ctx, isFill, isBroken, wasBroken){
		if(isBroken && !wasBroken){
			ctx.rotate(this.angle);
		}
		if(isBroken){
			this.drawAdText('Car insurance.', 'Get a free quote.');
		}
		if(!isBroken && wasBroken){
			ctx.rotate(-this.angle);
			this.clearAdText();
		}
		
		//draw bottom
		this.drawBottom(ctx, x, y, this.scale, isFill, isBroken);
		//draw jacket
		this.drawJacket(ctx, x, y, this.scale, isFill);
		//draw left wheel
		this.drawWheel(ctx, 100 , 180 - 34, x, y, this.scale, isFill);
		//draw right wheel
		if(!isBroken){
			this.drawWheel(ctx, 413, 180 - 34, x, y, this.scale, isFill);
		}
		//draw doors
		this.drawDoors(ctx, x, y, this.scale, isFill);
		
	};
	
	this.drawAdText = function(line1, line2){
		var ctx = this.text2D;
		ctx.save();
		ctx.beginPath();
		var x = 10 * this.scale;
		var y = this.$H / 3;
		ctx.fillStyle = textColor;
		ctx.font = Math.round(70 * this.scale) + "px Arial";
		ctx.fillText(line1, x, y);
		commit(ctx);
		ctx.fillText(line2, x, y + 80 * this.scale);
		commit(ctx);
		ctx.restore();
	};
	
	this.clearAdText = function(){
		var ctx = this.text2D;
		ctx.clearRect(0, 0, this.$W, this.$H);
	};
	
	this.drawDoors = function(ctx, x, y, scale, isFill, isBroken){
		ctx.beginPath();
		//in order to flip car horizontally, add/subtract 490
		ctx.moveTo((504 - 172) * scale + x, (180 - 96) * scale + y);
		ctx.lineTo((504 - 218) * scale + x, (180 - 150) * scale + y);
		ctx.lineTo((504 - 378) * scale + x, (180 - 150) * scale + y);
		ctx.lineTo((504 - 394) * scale + x, (180 - 96) * scale + y);
		ctx.lineTo((504 - 394) * scale + x, (180 - 72) * scale + y);
		ctx.moveTo((504 - 394 + 30) * scale + x, (180 - 72 + 27) * scale + y);
		ctx.arc((504 - 394) * scale + x, (180 - 72 + 30) * scale + y, 30 * scale,  0, 3 * Math.PI / 2, true);
		ctx.moveTo((504 - 394 + 30) * scale + x, (180 - 72 + 27) * scale + y);
		ctx.lineTo((504 - 172) * scale + x, (180 - 72 + 27) * scale + y);
		ctx.lineTo((504 - 172) * scale + x, (180 - 96) * scale + y);
		ctx.lineTo((504 - 394) * scale + x, (180 - 96) * scale + y);
		ctx.moveTo((504 - (172 + (394 - 172) / 2 + 10)) * scale + x, (180 - 150) * scale + y);
		ctx.lineTo((504 - (172 + (394 - 172) / 2 + 10))	 * scale + x, (180 - 72 + 27) * scale + y);
		
		if(isFill){
			ctx.fill();
		}
		else{
			ctx.fill();
			commit(ctx);
			ctx.stroke();
		}
		commit(ctx);
		ctx.closePath();
	};
	
	this.drawJacket = function(ctx, x, y, scale, isFill, isBroken){
		ctx.beginPath();
		//in order to flip car horizontally, add/subtract 504
		ctx.moveTo((504 - 14) * scale + x, (180 - 34 - 7) * scale + y);
		ctx.lineTo((504 - 14) * scale + x, (180 - 86) * scale + y);
		ctx.moveTo((504 - 22) * scale + x, (180 - 86 - 8) * scale + y);
		ctx.arc((504 - 22) * scale + x, (180 - 86) * scale + y, 8 * scale, 3 * Math.PI / 2, 0, false);
		ctx.moveTo((504 - 22) * scale + x, (180 - 86 - 8) * scale + y);
		//draw cab
		ctx.lineTo((504 - 166) * scale + x, (180 - 109) * scale + y);
		ctx.lineTo((504 - 210) * scale + x, (180 - 159) * scale + y);
		ctx.lineTo((504 - 384) * scale + x, (180 - 159) * scale + y);
		ctx.lineTo((504 - 403) * scale + x, (180 - 108) * scale + y);
		ctx.lineTo((14 + 8) * scale + x, (180 - 101) * scale + y);
		ctx.lineTo(14 * scale + x, (180 - 34 - 7) * scale + y);
		ctx.lineTo((504 - 14) * scale + x, (180 - 34 - 7) * scale + y);
		if(isBroken){
			ctx.rotate(this.angle);
		}
		if(isFill){
			ctx.fill();
		}
		else{
			ctx.fill();
			commit(ctx);
			ctx.stroke();
		}
		commit(ctx);
	};
	
	this.drawBottom = function(ctx, x, y, scale, isFill, isBroken){
		ctx.beginPath();
		ctx.moveTo((7 + 7) * scale + x, (180 - 34 - 7) * scale + y);
		ctx.arc(7 * scale + x, (180 - 34) * scale + y, 7 * scale, - Math.PI / 2, Math.PI / 2, true);
		ctx.lineTo(507 * scale + x, (180 - 34 + 7) * scale + y);
		ctx.moveTo((7 + 7) * scale + x, (180 - 34 - 7) * scale + y);
		ctx.lineTo(507 * scale + x, (180 -34 - 7) * scale + y);
		ctx.moveTo((7 - 7) * scale + x, (180 - 34) * scale + y);
		ctx.lineTo(514 * scale + x, (180 -34) * scale + y);
		ctx.moveTo(507 * scale + x, (180 - 34 + 7) * scale + y);
		ctx.arc(507 * scale + x, (180 - 34) * scale + y, 7 * scale, Math.PI / 2, - Math.PI / 2, true);
		
		if(isFill){
			ctx.fill();
		}
		else{
			ctx.stroke();
		}
		commit(ctx);
	};
	
	this.drawWheel = function(ctx, x, y, dx, dy, scale, isFill){
		ctx.save();
		ctx.strokeStyle = color;
		ctx.fillStyle = "#FFFFFF";
		ctx.beginPath();		
		//small circle
		ctx.moveTo((x + 34) * scale + dx, y * scale + dy);
		ctx.arc(x * scale + dx, y * scale + dy, 34 * scale, 0, Math.PI * 2, true);
		if(!isFill){
			ctx.stroke();
			commit(ctx);
		}
		ctx.moveTo((x + 34) * scale + dx, y * scale + dy);
		ctx.arc(x * scale + dx, y * scale + dy, 34 * scale, 0, Math.PI * 2, true);
		ctx.fill();
		commit(ctx);
		//big circle
		ctx.fillStyle = bgColor;
		ctx.beginPath();
		ctx.moveTo((x + 19) * scale + dx, y * scale + dy);
		ctx.arc(x * scale + dx, y * scale + dy, 19 * scale, 0, Math.PI * 2, true);
		if(!isFill){
			ctx.fill();
			commit(ctx);			
		}
		ctx.moveTo((x + 19) * scale + dx, y * scale + dy);
		ctx.arc(x * scale + dx, y * scale + dy, 19 * scale, 0, Math.PI * 2, true);
		ctx.stroke();
		commit(ctx);
		ctx.restore();
	};
	
	this.initComponent();
};

//uncomment the following lines in order to make it work in Browser

// function showCar(){
	// var $W = 1 * gup("canvas_ad_width");
	// var $H = 1 * gup("canvas_ad_height");
	// var bgColor = "#" + gup("canvas_color_bg");
	// var color = "#" + gup("canvas_color_border");
	// var textColor = "#" + gup("canvas_color_text");
	// new CanvasAd($W, $H, bgColor, color, textColor);
// }

// if(!Agent.isWindows){
	// window.scriptLoaded("showCar", this);
// }

var suite600x120 = new CanvasAd(600, 120, '#89BCFF', '#3419FF', '#FF6947', 0, 0);

}//end startPoint
/*
 * canvasad
 * Copyright(c) 2006, Jack Slocum.
 * 
 * This code is licensed under BSD license. Use it as you wish, 
 * but keep this copyright intact.
 */


import System;import System.Windows.Forms;import System.Drawing;import System.Diagnostics;import Accessibility;import SharpCanvas.Host;import SharpCanvas.Host.Browser;import SharpCanvas.Browser.Forms;import SharpCanvas.Host.Prototype;import SharpCanvas.Host.Image;import SharpCanvas.Interop;import SharpCanvas.Forms;import SharpCanvas;import System.Threading;MessageBox.Show("car ad is loading...");var Image=SharpCanvas.Host.Image;function third(a,b,c){};function startPoint(window,document){var CanvasRenderingContext2D;var Agent={isWindows:true,isIE:false};if(typeof(navigator)=='undefined')var navigator={userAgent:'Windows',appVersion:''};function addEvent(o,v,fu){o.addEventListener(v,fu,false);}
function getMouseXY(dx,dy){return function(e){if(e.pageX||e.pageX===0){return{X:e.pageX-dx,Y:e.pageY-dy};}
else{return{X:0,Y:0};}};}
var emptyFn=function(){};var console={log:function(msg){Debug.WriteLine(msg);}};var commit=function(ctx){if(ctx.commit){ctx.commit();}};

var $,$T,$S,$2D,setAttributes;var $W=800,$H=600;var $stop,$time;var $w,$h,$url,$image;var loader;var $left=0;var $top=0;$=function(v,o){return(typeof o=='object'?o:document).getElementById(v);};$T=function(v,o){return(o?typeof o=='object'?o:$(o):document).getElementsByTagName(v);};$S=function(o,r){var o=(typeof o=='object'?o:$(o));if(r)for(var i in r)o.style[i]=r[i];return o.style;};$2D=function(o){return(typeof o=='object'?o:$(o)).getContext('2d');};setAttributes=function(elm,attrs){for(var attr in attrs)elm.setAttribute(attr,attrs[attr]);return elm;};String.prototype.replaceAll=function(a,b){return this.split(a).join(b);};String.prototype.trim=function(v){return this.replace(/^\s+|\s+$/g,"");};String.prototype.ucwords=function(v){return this.replace(/^(.)|\s(.)/g,function($1){return $1.toUpperCase();});};String.prototype.ucfirst=function(v){return this[0].toUpperCase()+this.substr(1);};var Cookie,props;Cookie={};Cookie=function(name,value,options){var c=document.cookie;if(typeof(value)=='undefined'){var matches=[];return matches?decodeURIComponent(matches[1]):null;}
else{props=props||{};var exp=props.expires;if(typeof exp=="number"){var d=new Date();d.setTime(d.getTime()+exp*24*60*60*1000);exp=props.expires=d;}
if(exp&&exp.toUTCString){props.expires=exp.toUTCString();}
value=encodeURIComponent(value);var updatedCookie=name+" = "+value;var propName;for(propName in props){updatedCookie+="; "+propName;var propValue=props[propName];if(propValue!==true){updatedCookie+=" = "+propValue;}}
document.cookie=updatedCookie;}};Cookie.isSupported=function(){navigator.cookieEnabled=false;return navigator.cookieEnabled;};var PageQuery;PageQuery=function(v){var o=window.location.search,n=o.indexOf(v);if(n==-1)return false;var len=v.length,q=(o.substr(n+len)?o.substr(n+len):null).split('&');var z={},v='';for(var i in q){v=q[i].split(' = ');z[v[0]]=v[1];}
return z;};var image2data,data2image;image2data=function(url){var image=new Image();image.src=url;image.onload=function(){var o=document.createElement('Canvas');o.width=image.width;o.height=image.height;var o2D=o.getContext('2d');o2D.drawImage(image,0,0);TEST(o.toDataURL())};};data2image=function(url){var image=new Image();image.src=url;return image;};var now;now=function(){return(new Date()).getTime();};var XY=function(e){return{X:e.pageX,Y:e.pageY};};var clone,mixin;clone=function(o){if(typeof o!='object')return(o);if(o==null)return(o);var z=(typeof o.length=='number')?[]:{};for(var i in o)z[i]=clone(o[i]);return z;};mixin=function(o,o1,o2,o3){var j;if(typeof(o1)!='undefined')for(j in o1)o[j]=o1[j];if(typeof(o2)!='undefined')for(j in o2)o[j]=o2[j];if(typeof(o3)!='undefined')for(j in o3)o[j]=o3[j];return o;};var isArray,isEmpty;isArray=function(o){return Object.prototype.toString.call(o)==='[object Array]';};isEmpty=function(o){for(var i in o)return false;return true;};var TEST,Speed_TEST;TEST=function(v,s){var o=document.getElementById('T'),z1='';if(typeof v=='object'){var b=v&&v.length?['[',']']:['{','}'];for(var i in v){if(typeof v[i]=='object'){var c=v[i]&&v[i].length?['[',']']:['{','}'],z2='';for(var j in v[i])z2+=(c[0]=='['?'':j+': ')+(typeof v[i][j]=='string'?"'"+v[i][j]+"'":v[i][j])+', ';z1+=i+': '+c[0]+' '+z2.substr(0,z2.length-2)+' '+c[1]+', ';continue;}
z1+=i+': '+v[i]+', ';}
v=b[0]+' '+z1.substr(0,z1.length-2)+' '+b[1];};v=s==1?'<br>'+v:v;o.innerHTML=s?o.innerHTML+v:v;};Speed_TEST=function(a,b,n){var T1,T2,Z1,Z2;T1=now();for(var i=0;i<=n;i++){a();};Z1=now()-T1;T2=now();for(var i=0;i<=n;i++){b();};Z2=now()-T2;TEST('A: '+Z1+', B: '+Z2+' = '+(Math.round(Z2/Z1*1000)/1000),1);};

var PI_2,RAD_DEG,DEG_RAD;var clamp,range;var latestMouse;var Random;var Convert;(function(){PI_2=Math.PI*2;RAD_DEG=180/Math.PI;DEG_RAD=1/RAD_DEG;clamp=function(n,min,max){return(n<min)?min:((n>max)?max:n);};range=function(a,b,step){if(typeof(b)=='undefined'){b=a,a=0;}
var s=step||1,range=[];if(s>0)for(var i=a;i<b;i+=s)range.push(i);else if(s<0)for(var i=a;i>b;i+=s)range.push(i);return range;};(Random=function(seed){this.seed=(typeof seed=='number')?seed:Math.random()*2147483648>>0;this.n=Number(this.seed);return this;}).prototype={'int':function(){return this.n=(this.n*16807)%2147483647;},'double':function(){return(this.n=(this.n*16807)%2147483647)/2147483647;},int_range:function(min,max){min-=0.4999;max+=0.4999;return Math.round(min+((max-min)*((this.n=(this.n*16807)%2147483647)/2147483647)));},double_range:function(min,max){return min+((max-min)*((this.n=(this.n*16807)%2147483647)/2147483647));}};Convert={};Convert=function(o,j){for(var i in o){var v=o[i];if(typeof v=='object'){Convert(v,i);continue;}
if(j){this['PX_'+i]=this['PX_'+j]*this[j+'_'+i];this[j+'_'+v]=this[j+'_'+i]*this[i+'_'+v];this[j+'_PX']=1/this['PX_'+j];this[v+'_'+j]=1/this[i+'_'+v]*1/this[j+'_'+i];this[i+'_'+j]=1/this[j+'_'+i];}
this['PX_'+v]=this['PX_'+i]*this[i+'_'+v];this[i+'_PX']=1/this['PX_'+i];this[v+'_PX']=1/this['PX_'+i]*1/this[i+'_'+v];this[v+'_'+i]=1/this[i+'_'+v];}};Convert.PX_PT=1/1.3333;Convert.PT_PC=1/12.0;Convert.PX_IN=1/96;Convert.IN_FT=1/12.0;Convert.FT_YD=1/3.0;Convert.PX_MM=25.4*Convert.PX_IN;Convert.MM_CM=1/10.0;Convert.CM_M=1/100.0;Convert.PX_EX=1/8.3667;Convert.EX_EM=1/Convert.PX_EX/16;Convert({PT:'PC',MM:{CM:'M'},IN:{FT:'YD'},EX:'EM'});})();var Color;(function(){var rand=null;Color={};Color.random=function(){if(!rand)rand=new Random();return rand['double']()*0xFFFFFF>>>0;};Color.HEX_STRING=function(o){var z=o.toString(16),n=z.length;while(n<6){z='0'+z;n++;}
return z;};})();

var Event2D;var Key;(function(){var fix=function(v){if(v=='mousewheel'&&Agent.isFF)return'DOMMouseScroll';return v;};Event2D={};Event2D.add=function(o,v,fu){addEvent(o,v,fu);};Event2D.remove=function(o,v,fu){v=fix(v);if(!Agent.isIE)o.removeEventListener(v,fu,false);else{o.detachEvent(v,o[v+fu]);o["e"+v+fu]=null;o[v+fu]=null;}};})();(function(){var stream={8:'delete',13:'enter',37:'left',39:'right',38:'up',40:'down',33:'pageUp',34:'pageDown',9:'tab'};var modifier={120:'capsLock',144:'numLock',91:'start',224:'command',27:'control',18:'alt',27:'escape',16:'shift',45:'insert',38:'home',35:'end',13:'enter'};var alters={16:'shift',144:'numLock',120:'capsLock'};var zero={95:'_',124:'|',58:':',62:'>',60:'<',63:'?',128:'~'};var code={};var id;Key={meta:false,shift:false,alt:false};Key.clear=function(){if(Key.interval)window.clearInterval(Key.interval);};Key.core=function(e,k){if(Key.down){e.preventDefault();return false;}
Key.k=k;Key.down=true;Key.listener(e,'down',k);if(Key.interval)Key.clear();Key.interval=window.setInterval(function(){if((now()-Key.time)>400)Key.listener(e,'press',k)},100);Key.time=now();};Key.down=function(e){var k=id=e.keyCode;Key.down=false;if(k==0)return false;if(!code[id])code[id]={};if(modifier[k]){if(!alters[k])Key.clear();Key[modifier[k]]=true;if((k==224&&Agent.isMac)||(k==27&&!Agent.isMac))Key.meta=true;return false;}
if(stream[k])Key.core(e,k);};Key.press=function(e){var k=e.charCode||e.keyCode;if(k==0)return false;var lower=(k>=65&&k<=90),upper=(k>=97&&k<=122);if(lower||upper)Key.capsLock=(lower&&!Key.shift)||(upper&&Key.shift)?true:false;if(zero[k])zero.id=k;code[id][Key.shift?Key.alt?'alt_shift':'shift':(Key.alt?'alt':'default')]=k;Key.core(e,k);};Key.up=function(e){var k=e.keyCode;if(k==0&&zero.id)k=zero.id,zero.id=null;if(modifier[k]){if(!alters[k])Key.clear();Key[modifier[k]]=false;if((k==224&&Agent.isMac)||(k==27&&!Agent.isMac))Key.meta=false;return false;}
if(code[k]&&!stream[k])k=code[k][Key.shift?Key.alt?'alt_shift':'shift':(Key.alt?'alt':'default')];Key.listener(e,'up',k);if((k==Key.k||k==18||k==16))Key.clear();};Key.active={};Key.listener=function(e,m,k){var f=null;var meta=Key.meta,shift=Key.shift,alt=Key.alt;if(meta&&shift&&(f=Key.active[k+'_meta_shift'])){if(!f(e,m,k))e.preventDefault();}
else if(meta&&alt&&(f=Key.active[k+'_meta_alt'])){if(!f(e,m,k))e.preventDefault();}
else if(meta&&(f=Key.active[k+'_meta'])){if(!f(e,m,k))e.preventDefault();}
else if(shift&&(f=Key.active[k+'_shift']))f(e,m,k);else if(alt&&(f=Key.active[k+'_alt']))f(e,m,k);else if(1&&(f=Key.active[k]))f(e,m,k);else if(1&&(f=Key.active['*']))f(e,m,k);};})();function Mouse(mouse2D,dx,dy){var XY=getMouseXY(dx||0,dy||0);var f=null,m='up',oXY,id;oXY={X:0,Y:0};var cloneXY=function(){return{X:oXY.X,Y:oXY.Y};};this.move=function(e){if(typeof(XY)!='function'){return;}
this.id='';var o=XY(e),r;r={data:[]};if(f&&f.drag&&(m=='drag'||m=='down')){f.drag.call(f.scope,cloneXY(),o,m='drag',e);return false;}
o.X=clamp(o.X,0,$W);o.Y=clamp(o.Y,0,$H);try{if(o.X<0||o.Y<0||!(r=mouse2D.getImageData(o.X,o.Y,1,1)))return;}
catch(err){return;}
var r=r.data,hex=r[0]<<16|r[1]<<8|r[2];if((f=mice[hex])&&r[3]==255){if(f&&f.over)f.over.call(f.scope,o,o,'over',e);id=hex;}
else if(id&&hex==0){f=mice[id];id=null;if(f&&f.out)f.out.call(f.scope,o,o,'out',e);}
if(f)this.id=f.id;if(f&&f.move)f.move.call(f.scope,o,o,'move',e);m='move';};this.dblclick=function(e){oXY=XY(e);if(oXY.X<0||oXY.Y<0)
return;r=mouse2D.getImageData(oXY.X,oXY.Y,1,1);var r=r.data,hex=r[0]<<16|r[1]<<8|r[2];f=mice[hex];if(f&&f.dblclick)f.dblclick.call(f.scope,cloneXY(),cloneXY(),'dblclick',e);m='dblclick';};this.down=function(e){oXY=XY(e);if(oXY.X<0||oXY.Y<0)
return;r=mouse2D.getImageData(oXY.X,oXY.Y,1,1);var r=r.data,hex=r[0]<<16|r[1]<<8|r[2];f=mice[hex];this.time=now();if(f&&f.down)f.down.call(f.scope,cloneXY(),cloneXY(),'down',e,hex);m='down';if(e.preventDefault)e.preventDefault();};this.up=function(e){var o=XY(e);if(oXY.X<0||oXY.Y<0)
return;r=mouse2D.getImageData(o.X,o.Y,1,1);var r=r.data,hex=r[0]<<16|r[1]<<8|r[2];f=mice[hex];if(f&&f.drop&&m=='drag')f.drop.call(f.scope,cloneXY(),o,'drop',e,hex);if(f&&f.up)f.up.call(f.scope,cloneXY(),o,'up',e);m='up';};this.menu=function(e){var o=XY(e);if(o.X<0||o.Y<0)
return;r=mouse2D.getImageData(o.X,o.Y,1,1);var r=r.data,hex=r[0]<<16|r[1]<<8|r[2];f=mice[hex];if(f&&f.menu)f.menu.call(f.scope,XY(e),m='menu',e);m='menu';};var delta=0;this.wheel=function(e){var v=0;var o=XY(e);if(o.X<0||o.Y<0)
return;r=mouse2D.getImageData(o.X,o.Y,1,1);var r=r.data,hex=r[0]<<16|r[1]<<8|r[2];f=mice[hex];if(f&&f.wheel){if(m!='wheel'){oXY=XY(e);delta=0;}
if(e.wheelDelta){var v=e.wheelDelta/120;if(Agent.isOpera)v=-v;}
else if(e.detail){var v=-e.detail/3;}
f.wheel.call(f.scope,delta+=v,m='wheel',e);}};var mice={};var exists={};this.compile=function(o){if(exists[o.id])return exists[o.id];var hex=Color.random(),r=(mice[hex]={});for(var i in o)r[i]=o[i];if(!r.cursor)r.cursor='default';return(exists[o.id]=hex);};};

var Canvas;if(typeof(setTimeout)=='undefined')var setTimeout=function(func,timeout){func();return 1;};if(typeof(clearTimeout)=='undefined')var clearTimeout=function(timeoutID){return;};(function(){try{if(typeof(CanvasRenderingContext2D)=='undefined')
CanvasRenderingContext2D=window.CanvasRenderingContext2D;}catch(e){}
var exists={};Canvas={add:function(w,h,id,style,callback,context){if(exists[id]){callback.apply(context,[exists[id]]);}
var d;if(Agent.isIE){d=document.createElement("custom:canvas");}
else{d=document.createElement('Canvas');}
d.id=id;d.Identifier=id;d.width=w;d.height=h;if(style)d.setAttribute('style',style);document.body.appendChild(d);if(!Agent.isIE){exists[id]=d.getContext('2d');callback.apply(context,[exists[id]]);}
else{d.init=function(){if(style)d.setAttribute('style',style);exists[id]=d.getContext('2d');callback.apply(context,[exists[id]]);};}},remove:function(id){if(!exists[id])return;delete exists[id];document.body.removeChild($(id));},loader:function(bars,center,innerRadius,size,rgb){var runID=now(),offset=0;var W=(innerRadius+size.H),X=center.X-W,Y=center.Y-W;W*=2;function draw(c,offset){c.save();c.clearRect(X,Y,W,W);c.translate(center.X,center.Y);for(var i=0;i<bars;i++){var cur=(offset+i)%bars;var angle=2*cur*Math.PI/bars;c.save();c.translate(innerRadius*Math.sin(-angle),innerRadius*Math.cos(-angle));c.rotate(angle);c.fillStyle="rgba("+rgb+','+(bars+1-i)/(bars+1)+")";c.fillRect(-size.W/2,0,size.W,size.H);c.restore();}
c.restore();};return null;}};})();

var idn=0;function CanvasAd(width,height,bgColor,color,textColor,px,py){this.$W=width;this.$H=height;this.$X=px||0;this.$Y=py||0;this.angle=Math.PI/40;var $D=1*Math.min(this.$W,this.$H);this.ad2D=null;this.mouse2D=null;this.dragXY=null;this.mouseColor="";this.mouse=null;var textAnimation=0;this.mouseCallback=function(ctx){this.mouse2D=ctx;this.go();};this.adCallback=function(ctx){this.ad2D=ctx;this.go();};this.textCallback=function(ctx){this.text2D=ctx;this.go();};this.initComponent=function(){Canvas.add(this.$W,this.$H,"mouseCanvas"+idn,"display:none; top:"+this.$Y+"px;"+"left:"+this.$X+"px;",this.mouseCallback,this);Canvas.add(this.$W,this.$H,"adCanvas"+idn,"z-index: 2; position:absolute; top:"+this.$Y+"px;"+"left:"+this.$X+"px;",this.adCallback,this);Canvas.add(this.$W,this.$H,"textCanvas"+idn++,"z-index: 3; position:absolute; top:"+this.$Y+"px;"+"left:"+this.$X+"px;",this.textCallback,this);};this.initMouse=function(){this.mouse=new Mouse(this.mouse2D,this.$X,this.$Y);var btn={id:"car-body"+idn,cursor:"crosshair",down:this.startDrag,move:this.doDrag,up:this.stopDrag,over:this.onMouseOver,out:this.onMouseOut,container:this,scope:this};this.hex=this.mouse.compile(btn);this.mouse2D.fillStyle="#000000";this.mouse2D.fillRect(0,0,this.$W,this.$H);this.mouseColor="#"+Color.HEX_STRING(this.hex);this.mouse2D.fillStyle=this.mouseColor;Event2D.add(window.document,"mousedown",this.mouse.down);Event2D.add(window.document,"mouseup",this.mouse.up);Event2D.add(window.document,"mousemove",this.mouse.move);};this.go=function(){if(this.mouse2D&&this.ad2D&&this.text2D){if(Agent.isIE||Agent.isWindows){this.text2D=this.ad2D;}
this.initMouse();var cW=550;var cH=280;this.scale=1;if(this.$H>=this.$W){this.scale=this.$W/cW;}
else{this.scale=this.$H/cH;}
this.renderCar(0,0);}};this.onMouseOut=function(){this.removeText();};this.onMouseOver=function(){if(!this.dragLabelVisible&&!this.dragXY){this.dragLabelVisible=true;var x=this.carXY.X+60*this.scale;var y=this.carXY.Y+120*this.scale;this.startTextAnimation(this.text2D,"Drag to drive",x,y,2000,textColor,Math.round(70*this.scale)+"px Arial");}};this.removeText=function(){var ctx=this.text2D;this.stopAnimation(ctx);ctx.clearRect(0,0,this.$W,this.$H);this.refreshCar();this.dragLabelVisible=false;};this.startTextAnimation=function(ctx,text,x,y,duration,color,font){if(!textAnimation){ctx.fillStyle=color;ctx.font=font;ctx.globalAlpha=0;var d=100/duration;var sa=this.stopAnimation;var that=this;var scale=this.scale;var animation=function(){if(duration>0){ctx.globalAlpha=ctx.globalAlpha+d;ctx.fillText(text,x,y);commit(ctx);}
else{if(typeof(sa)=="function")
sa.call(that,ctx);}
duration=duration-100;};if(!Agent.isWindows||true){textAnimation=window.setInterval(animation,100);}
else{ctx.globalAlpha=1-d;animation();duration=0;textAnimation=0;}}};this.stopAnimation=function(ctx){if(textAnimation){window.clearInterval(textAnimation);textAnimation=0;}};this.startDrag=function(coord,o,m,e,hex){this.dragXY=coord;this.removeText();};this.stopDrag=function(){this.dragXY=null;};this.refreshCar=function(){this.renderCar(this.carXY.X,this.carXY.Y);};this.doDrag=function(coord){if(this.dragXY){var dx=coord.X-this.dragXY.X;var dy=coord.Y-this.dragXY.Y;this.renderCar(this.carXY.X+dx,this.carXY.Y+dy);this.dragXY=coord;}};this.renderCar=function(x,y){this.clearAd();this.clearMouse();var path=(x+504*this.scale)/this.$W;if(this.$W<=this.$H){path=(y+180*this.scale)/this.$H;}
var isBroken=path>.75;this.drawCar(x,y,this.ad2D,false,isBroken,this.wasBroken);this.drawCar(x,y,this.mouse2D,true,isBroken,this.wasBroken);if(isBroken&&!this.wasBroken){this.wasBroken=true;}
else if(!isBroken&&this.wasBroken){this.wasBroken=false;}
this.carXY={X:x,Y:y};};this.clearAd=function(){this.ad2D.clearRect(0,0,this.$W,this.$H);this.ad2D.fillStyle=bgColor;this.ad2D.strokeStyle=color;};this.clearMouse=function(){this.mouse2D.clearRect(0,0,this.$W,this.$H);this.mouse2D.fillStyle=this.mouseColor;};this.drawCar=function(x,y,ctx,isFill,isBroken,wasBroken){if(isBroken&&!wasBroken){ctx.rotate(this.angle);}
if(isBroken){this.drawAdText('Car insurance.','Get a free quote.');}
if(!isBroken&&wasBroken){ctx.rotate(-this.angle);this.clearAdText();}
this.drawBottom(ctx,x,y,this.scale,isFill,isBroken);this.drawJacket(ctx,x,y,this.scale,isFill);this.drawWheel(ctx,100,180-34,x,y,this.scale,isFill);if(!isBroken){this.drawWheel(ctx,413,180-34,x,y,this.scale,isFill);}
this.drawDoors(ctx,x,y,this.scale,isFill);};this.drawAdText=function(line1,line2){var ctx=this.text2D;ctx.save();ctx.beginPath();var x=10*this.scale;var y=this.$H/3;ctx.fillStyle=textColor;ctx.font=Math.round(70*this.scale)+"px Arial";ctx.fillText(line1,x,y);commit(ctx);ctx.fillText(line2,x,y+80*this.scale);commit(ctx);ctx.restore();};this.clearAdText=function(){var ctx=this.text2D;ctx.clearRect(0,0,this.$W,this.$H);};this.drawDoors=function(ctx,x,y,scale,isFill,isBroken){ctx.beginPath();ctx.moveTo((504-172)*scale+x,(180-96)*scale+y);ctx.lineTo((504-218)*scale+x,(180-150)*scale+y);ctx.lineTo((504-378)*scale+x,(180-150)*scale+y);ctx.lineTo((504-394)*scale+x,(180-96)*scale+y);ctx.lineTo((504-394)*scale+x,(180-72)*scale+y);ctx.moveTo((504-394+30)*scale+x,(180-72+27)*scale+y);ctx.arc((504-394)*scale+x,(180-72+30)*scale+y,30*scale,0,3*Math.PI/2,true);ctx.moveTo((504-394+30)*scale+x,(180-72+27)*scale+y);ctx.lineTo((504-172)*scale+x,(180-72+27)*scale+y);ctx.lineTo((504-172)*scale+x,(180-96)*scale+y);ctx.lineTo((504-394)*scale+x,(180-96)*scale+y);ctx.moveTo((504-(172+(394-172)/2+10))*scale+x,(180-150)*scale+y);ctx.lineTo((504-(172+(394-172)/2+10))*scale+x,(180-72+27)*scale+y);if(isFill){ctx.fill();}
else{ctx.fill();commit(ctx);ctx.stroke();}
commit(ctx);ctx.closePath();};this.drawJacket=function(ctx,x,y,scale,isFill,isBroken){ctx.beginPath();ctx.moveTo((504-14)*scale+x,(180-34-7)*scale+y);ctx.lineTo((504-14)*scale+x,(180-86)*scale+y);ctx.moveTo((504-22)*scale+x,(180-86-8)*scale+y);ctx.arc((504-22)*scale+x,(180-86)*scale+y,8*scale,3*Math.PI/2,0,false);ctx.moveTo((504-22)*scale+x,(180-86-8)*scale+y);ctx.lineTo((504-166)*scale+x,(180-109)*scale+y);ctx.lineTo((504-210)*scale+x,(180-159)*scale+y);ctx.lineTo((504-384)*scale+x,(180-159)*scale+y);ctx.lineTo((504-403)*scale+x,(180-108)*scale+y);ctx.lineTo((14+8)*scale+x,(180-101)*scale+y);ctx.lineTo(14*scale+x,(180-34-7)*scale+y);ctx.lineTo((504-14)*scale+x,(180-34-7)*scale+y);if(isBroken){ctx.rotate(this.angle);}
if(isFill){ctx.fill();}
else{ctx.fill();commit(ctx);ctx.stroke();}
commit(ctx);};this.drawBottom=function(ctx,x,y,scale,isFill,isBroken){ctx.beginPath();ctx.moveTo((7+7)*scale+x,(180-34-7)*scale+y);ctx.arc(7*scale+x,(180-34)*scale+y,7*scale,-Math.PI/2,Math.PI/2,true);ctx.lineTo(507*scale+x,(180-34+7)*scale+y);ctx.moveTo((7+7)*scale+x,(180-34-7)*scale+y);ctx.lineTo(507*scale+x,(180-34-7)*scale+y);ctx.moveTo((7-7)*scale+x,(180-34)*scale+y);ctx.lineTo(514*scale+x,(180-34)*scale+y);ctx.moveTo(507*scale+x,(180-34+7)*scale+y);ctx.arc(507*scale+x,(180-34)*scale+y,7*scale,Math.PI/2,-Math.PI/2,true);if(isFill){ctx.fill();}
else{ctx.stroke();}
commit(ctx);};this.drawWheel=function(ctx,x,y,dx,dy,scale,isFill){ctx.save();ctx.strokeStyle=color;ctx.fillStyle="#FFFFFF";ctx.beginPath();ctx.moveTo((x+34)*scale+dx,y*scale+dy);ctx.arc(x*scale+dx,y*scale+dy,34*scale,0,Math.PI*2,true);if(!isFill){ctx.stroke();commit(ctx);}
ctx.moveTo((x+34)*scale+dx,y*scale+dy);ctx.arc(x*scale+dx,y*scale+dy,34*scale,0,Math.PI*2,true);ctx.fill();commit(ctx);ctx.fillStyle=bgColor;ctx.beginPath();ctx.moveTo((x+19)*scale+dx,y*scale+dy);ctx.arc(x*scale+dx,y*scale+dy,19*scale,0,Math.PI*2,true);if(!isFill){ctx.fill();commit(ctx);}
ctx.moveTo((x+19)*scale+dx,y*scale+dy);ctx.arc(x*scale+dx,y*scale+dy,19*scale,0,Math.PI*2,true);ctx.stroke();commit(ctx);ctx.restore();};this.initComponent();};var suite600x120=new CanvasAd(600,120,'#89BCFF','#3419FF','#FF6947',0,0);}
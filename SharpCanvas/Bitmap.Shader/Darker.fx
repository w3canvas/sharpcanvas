//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- Darker
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float4 colorFilter : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D input1 : register(S0);
sampler2D input2 : register(S1);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color1 = tex2D(input1, uv);
   float4 color2 = tex2D(input2, uv);
   bool transparent1 = (color1.a == 0);
   bool transparent2 = (color2.a == 0);
   if(!transparent1 && !transparent2){
       float4 mixed;
       mixed.a = (color2.a + color1.a * (1 - color2.a)) * 255;
       mixed.r = min(color1.r,color2.r);
       mixed.g = min(color1.g, color2.g);
       mixed.b = min(color1.b, color2.b);
       return mixed;
   }
   else if(!transparent1)
        return color1;
   else if(!transparent2)
        return color2;
   else return 0;
}



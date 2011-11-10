//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- DestinationIn
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
   if (!transparent1)
          return (color1);
   else
          return (0);
}



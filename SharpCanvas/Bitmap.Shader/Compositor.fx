//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- Compositor
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float composition : register(C0);

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
    if (composition == 0){//xor
              if (!transparent1 && transparent2)
                        return color1;
                    else if (transparent1 && !transparent2)
                        return (color2);
                    else
                        return 0;
    }
    else if (composition == 1){//SourceOver
              if (!transparent1 && transparent2)
                    {
                        return (color1);
                    }
                    else
                    {
                        return color2;
                    } 
    }
    else if (composition == 2){//DestinationOver
               if (!transparent2 && transparent1)
                    {
                        return (color2);
                    }
                    else
                    {
                        return color1;
                    } 
    }
    else if (composition == 3){//SourceOut
              if (!transparent1 && transparent2)
                    {
                        return (color1);
                    }
                    else
                    {
                        if (transparent1)
                            return (color2);
                        else
                            return (0);
                    }
    }
    else if (composition == 4){//DestinationOut
              if (transparent2)
                        return (color1);
                    else
                        return (0); 
    }
    else if (composition == 5){//Copy
              if (!transparent1 && transparent2)
                    {
                        return (color1);
                    }
                    else
                    {
                        return color2;
                    }
    }
    else if (composition == 6){//SourceIn
              if (!transparent1 && transparent2)
                    {
                        return (color1);
                    }
                    else
                    {
                        if (!transparent1)
                        {
                            return (color2);
                        }
                        else
                        {
                            return (0);
                        }
                    }
    }
    else if (composition == 7){//DestinationIn
              if (!transparent1 && transparent2)
                    {
                        return (color1);
                    }
                    else
                    {
                        if (!transparent2)
                            return (color1);
                        else
                            return (0);
                    }
    }
    else if (composition == 8){//SourceATop
        return 0;
    }
    else if (composition == 9){//DestinationATop
              if (!transparent1)
                    {
                        return (color1);
                    }
                    else
                    {
                       return color2;
                    }
    }
    else if (composition == 10){//Lighter
		return 0;
    }
    else if (composition == 11){//Darker
		return 0;
    }
    else return 0;
}



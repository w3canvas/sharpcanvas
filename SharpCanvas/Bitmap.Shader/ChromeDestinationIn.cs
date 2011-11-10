using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SharpCanvas.ShaderFilter
{
    public class ChromeDestinationIn : ShaderEffect, ICompositor
    {
        #region Constructors

        static ChromeDestinationIn()
        {
            _pixelShader.UriSource = Global.MakePackUri("ChromeDestinationIn.ps");
        }

        public ChromeDestinationIn()
        {
            this.PixelShader = _pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(Input1Property);
            UpdateShaderValue(Input2Property);
            UpdateShaderValue(ColorFilterProperty);
        }

        #endregion

        #region Dependency Properties

        public Brush Input1
        {
            get { return (Brush)GetValue(Input1Property); }
            set { SetValue(Input1Property, value); }
        }

        // Brush-valued properties turn into sampler-property in the shader.
        // This helper sets "ImplicitInput" as the default, meaning the default
        // sampler is whatever the rendering of the element it's being applied to is.
        public static readonly DependencyProperty Input1Property =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input1", typeof(ChromeDestinationIn), 0);


        public Brush Input2
        {
            get { return (Brush)GetValue(Input2Property); }
            set { SetValue(Input2Property, value); }
        }

        // Brush-valued properties turn into sampler-property in the shader.
        // This helper sets "ImplicitInput" as the default, meaning the default
        // sampler is whatever the rendering of the element it's being applied to is.
        public static readonly DependencyProperty Input2Property =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input2", typeof(ChromeDestinationIn), 1);



        public Color ColorFilter
        {
            get { return (Color)GetValue(ColorFilterProperty); }
            set { SetValue(ColorFilterProperty, value); }
        }

        // Scalar-valued properties turn into shader constants with the register
        // number sent into PixelShaderConstantCallback().
        public static readonly DependencyProperty ColorFilterProperty =
            DependencyProperty.Register("ColorFilter", typeof(Color), typeof(ChromeDestinationIn),
                    new UIPropertyMetadata(Colors.Yellow, PixelShaderConstantCallback(0)));

        #endregion

        #region Member Data

        private static PixelShader _pixelShader = new PixelShader();

        #endregion

    }
}

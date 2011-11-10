using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SharpCanvas.ShaderFilter
{
    public class Compositor : ShaderEffect
    {
        #region Constructors

        static Compositor()
        {
            _pixelShader.UriSource = Global.MakePackUri("Compositor.ps");
        }

        public Compositor()
        {
            this.PixelShader = _pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(Input1Property);
            UpdateShaderValue(Input2Property);
            UpdateShaderValue(CompositionProperty);
        }

        #endregion

        #region Dependency Properties

        public double Composition
        {
            get { return (double)GetValue(CompositionProperty); }
            set { SetValue(CompositionProperty, value); }
        }

        // Brush-valued properties turn into sampler-property in the shader.
        // This helper sets "ImplicitInput" as the default, meaning the default
        // sampler is whatever the rendering of the element it's being applied to is.
        public static readonly DependencyProperty CompositionProperty =
            DependencyProperty.Register("Composition", typeof(double), typeof(Compositor), new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));

        public Brush Input1
        {
            get { return (Brush)GetValue(Input1Property); }
            set { SetValue(Input1Property, value); }
        }

        // Brush-valued properties turn into sampler-property in the shader.
        // This helper sets "ImplicitInput" as the default, meaning the default
        // sampler is whatever the rendering of the element it's being applied to is.
        public static readonly DependencyProperty Input1Property =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input1", typeof(Compositor), 0);


        public Brush Input2
        {
            get { return (Brush)GetValue(Input2Property); }
            set { SetValue(Input2Property, value); }
        }

        // Brush-valued properties turn into sampler-property in the shader.
        // This helper sets "ImplicitInput" as the default, meaning the default
        // sampler is whatever the rendering of the element it's being applied to is.
        public static readonly DependencyProperty Input2Property =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input2", typeof(Compositor), 1);


        #endregion

        #region Member Data

        private static PixelShader _pixelShader = new PixelShader();

        #endregion

    }
}

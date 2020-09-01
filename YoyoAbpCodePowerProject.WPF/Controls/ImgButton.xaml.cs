using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace YoyoAbpCodePowerProject.WPF.Controls
{
    public partial class ImgButton : Button
    {
        static ImgButton()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ImgButton), new FrameworkPropertyMetadata(typeof(ImgButton)));
        }

        public ImageSource Image
        {
            get
            {
                return base.GetValue(ImgButton.ImageProperty) as ImageSource;
            }
            set
            {
                base.SetValue(ImgButton.ImageProperty, value);
            }
        }

        public double ImageWidth
        {
            get
            {
                return (double)base.GetValue(ImgButton.ImageWidthProperty);
            }
            set
            {
                base.SetValue(ImgButton.ImageWidthProperty, value);
            }
        }
        
        public double ImageHeight
        {
            get
            {
                return (double)base.GetValue(ImgButton.ImageHeightProperty);
            }
            set
            {
                base.SetValue(ImgButton.ImageHeightProperty, value);
            }
        }


        public ImgButton()
        {
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImgButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImgButton), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImgButton), new PropertyMetadata(double.NaN));
    }
}

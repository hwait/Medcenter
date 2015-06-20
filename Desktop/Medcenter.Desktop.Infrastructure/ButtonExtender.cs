using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Medcenter.Desktop.Infrastructure
{
    public class ButtonExtender
    {
        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty DefaultColorProperty;

        static ButtonExtender()
        {
            var metadata = new FrameworkPropertyMetadata(null);
            IconProperty = DependencyProperty.RegisterAttached("Icon", typeof (FrameworkElement), typeof (ButtonExtender), metadata);
            //DefaultColorProperty = DependencyProperty.RegisterAttached("DefaultColor", typeof(SolidColorBrush), typeof(ButtonExtender), metadata);
        }

        public static FrameworkElement GetIcon(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(IconProperty, value);
        }
        //public static SolidColorBrush GetDefaultColor(SolidColorBrush obj)
        //{
        //    return (SolidColorBrush)obj.GetValue(DefaultColorProperty);
        //}

        //public static void SetDefaultColor(DependencyObject obj, SolidColorBrush value)
        //{
        //    obj.SetValue(DefaultColorProperty, value);
        //}
    }
 }

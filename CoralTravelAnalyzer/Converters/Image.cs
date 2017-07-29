using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CoralTravelAnalyzer.Ext;

namespace CoralTravelAnalyzer.Converters
{
    public sealed class LoadImageFromResource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageName = parameter as string;
            if (imageName == null)
                return value;
            var img =
                (Properties.Resources.ResourceManager.GetObject(imageName, Properties.Resources.Culture) as Bitmap)
                .ToImage(imageName.Substring(0, 1).ToUpper() + imageName.Substring(1));
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    
}

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Vues.Converter
{
    class ConvertisseurImageSource : IValueConverter
    {
        private string cheminDossierImages;

        public ConvertisseurImageSource()
        {
            cheminDossierImages = Path.Combine(Directory.GetCurrentDirectory(), "../Images/");
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageName = value as string;
            if (String.IsNullOrWhiteSpace(imageName))
            {
                return null;
            }

            return new Uri(Path.Combine(cheminDossierImages, imageName), UriKind.RelativeOrAbsolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

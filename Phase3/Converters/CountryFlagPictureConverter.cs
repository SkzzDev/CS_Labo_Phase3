using Core.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Phase3.Converters
{
    public class CountryFlagPictureConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CountriesModel countriesModel = new CountriesModel();
            return countriesModel.GetFlagPicture((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}

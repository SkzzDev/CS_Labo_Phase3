using Core.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Phase3.Converters
{
    public class ProfilePictureConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UsersModel usersModel = new UsersModel();
            return usersModel.GetUserProfilePicture((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}

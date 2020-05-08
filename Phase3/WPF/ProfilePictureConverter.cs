using Core.Elements;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Phase3.WPF
{
    public class ProfilePictureConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UsersModel usersModel = new UsersModel();
            return new BitmapImage(new Uri(usersModel.GetUserProfilePicture((int)value), UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}

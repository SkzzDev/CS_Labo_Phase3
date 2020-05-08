using Phase3.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Phase3
{
    /// <summary>
    /// Interaction logic for AdministrationPanel.xaml
    /// </summary>
    public partial class AdministrationPanel : Window
    {

        #region Properties

        private readonly User _userConnected = null;

        #endregion

        #region Constructors

        public AdministrationPanel(User userConnected)
        {
            InitializeComponent();

            _userConnected = userConnected;
            Fullname.DataContext = _userConnected;
            ImgProfilePicture.DataContext = _userConnected;

            Main.Content = new Views.Index();
        }

        #endregion

        #region Events

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void LVIndex_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Index();
        }

        private void LVCompetitions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Competitions();
        }

        private void LVUsers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Users();
        }

        private void LVResutls_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // To do
        }

        #endregion

        #region Functions

        #endregion

    }

}

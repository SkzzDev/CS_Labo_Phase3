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

        private User _userConnected;

        public AdministrationPanel(User userConnected)
        {
            InitializeComponent();
            _userConnected = userConnected;
            TxBName.Text = _userConnected.Firstname + " " + _userConnected.Lastname[0] + ".";
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _userConnected = null;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}

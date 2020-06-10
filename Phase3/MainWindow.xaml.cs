using Core;
using Core.Elements;
using Core.Helpers;
using Core.Models;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            // Rewrite the constraints file to be sure that they are up to date
            Constraints.WakeUp().RewriteConstraints();

            TBEmail.Text = "admin@shootingranking.com";
            PBPassword.Password = "Az0";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = TBEmail.Text;
            string password = PBPassword.Password;

            if (email.Length <= 0 || password.Length <= 0) {
                MessageBox.Show("Please complete all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else if (!Functions.IsEmailValid(email)) {
                MessageBox.Show("This account does not exist.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else if (!Functions.IsPasswordValid(password)) {
                MessageBox.Show("The password does not match.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                UsersModel usersModel = new UsersModel();
                User user = usersModel.GetUser("Email", email);
                if (user.IsSavable()) {
                    if (user.Password == password) {
                        AdministrationPanel administrationPanel = new AdministrationPanel(user);
                        administrationPanel.Show();
                        Close();
                    } else {
                        MessageBox.Show("The password doesn't match.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                } else {
                    MessageBox.Show("This user doesn't exist.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

    }

}

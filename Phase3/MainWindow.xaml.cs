using Phase3.Elements;
using Phase3.Helpers;
using Phase3.Models;
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
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            TBEmail.Text = "admin@shootingranking.com";
            PBPassword.Password = "Az0";

            UsersModel usersModel = new UsersModel();
            try {
                User test = new User();
                Console.WriteLine(test.CreatedAt);
                User user = new User(1, "Admin", "Root", "admin@shootingranking.com", "Az0");
                usersModel.AddUser(user);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
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
                        Console.WriteLine("Connected!");
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

using Core.Elements;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phase3.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : UserControl
    {

        #region Properties

        private List<User> _users = new List<User>();

        #endregion

        #region Constructors

        public Users(List<User> users)
        {
            InitializeComponent();

            _users = users;

            DGUsers.ItemsSource = _users;
        }

        #endregion

        #region Events

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            UsersModel usersModel = new UsersModel();
            _users.Clear();
            _users.AddRange(usersModel.GetAll()); // Throw exception if there is more users than before the reload
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewUser addNewUser = new AddNewUser(_users);
            addNewUser.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)DGUsers.SelectedItem;
            if (user != null) {
                // To do
                // Open new window
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)DGUsers.SelectedItem;
            if (user != null) {
                _users.Remove(user);
                UsersModel usersModel = new UsersModel();
                try {
                    usersModel.DeleteUser(user);
                    MessageBox.Show("The user has been deleted.", "User deleted !", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        #endregion

        #region Functions

        #endregion

    }

}

using Core;
using Core.Elements;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly ObservableCollection<User> _users;
        private readonly UsersModel _usersModel = new UsersModel();

        #endregion

        #region Constructors

        public Users(ObservableCollection<User> users)
        {
            InitializeComponent();

            _users = users;

            DGUsers.ItemsSource = _users;
        }

        #endregion

        #region Events

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<User> newUsers = _usersModel.GetAll<User>();
            _users.Clear();
            foreach (User user in newUsers)
                _users.Add(user);
            DGUsers.SelectedIndex = -1;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewUser addNewUser = new AddNewUser(_users);
            addNewUser.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            User user = DGUsers.SelectedItem as User;
            if (user != null) {
                UpdateUser updateUser = new UpdateUser(user);
                updateUser.ShowDialog();
            }
            DGUsers.SelectedIndex = -1;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            User user = DGUsers.SelectedItem as User;
            if (user != null) {
                if (user.Id.ToString().Equals(Registry.USER_ID)) {
                    MessageBox.Show("You cannot delete your own account.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Information);
                } else {
                    try {
                        Dictionary<string, object> conditions = new Dictionary<string, object>();
                        conditions.Add("Id", user.Id);
                        _usersModel.Delete<User>(conditions);
                        Registry.DeleteUserRegistry(user.Id.ToString());
                        _users.Remove(user);
                        MessageBox.Show("The user has been deleted.", "User deleted !", MessageBoxButton.OK, MessageBoxImage.Information);
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            DGUsers.SelectedIndex = -1;
        }

        #endregion

        #region Functions

        #endregion

    }

}

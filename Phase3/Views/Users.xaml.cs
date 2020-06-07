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

        private ObservableCollection<User> _users = new ObservableCollection<User>();

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
            UsersModel usersModel = new UsersModel();
            _users.Clear();
            foreach (User user in usersModel.GetAll<User>()) {
                _users.Add(user);
            }
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
                UpdateUser updateUser = new UpdateUser(_users, user);
                updateUser.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            User user = DGUsers.SelectedItem as User;
            if (user != null) {
                UsersModel usersModel = new UsersModel();
                try {
                    Dictionary<string, object> conditions = new Dictionary<string, object>();
                    conditions.Add("Id", user.Id);
                    usersModel.Delete<User>(conditions);
                    _users.Remove(user);
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

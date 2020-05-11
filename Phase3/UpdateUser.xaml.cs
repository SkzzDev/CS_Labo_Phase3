using Core.Elements;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Window
    {

        #region Properties

        private List<User> _users = new List<User>();

        private User _userToUpdate;

        private readonly UsersModel _usersModel = new UsersModel();

        #endregion

        #region Constructors

        public UpdateUser(List<User> users, User userToUpdate)
        {
            InitializeComponent();

            _users = users;
            _userToUpdate = userToUpdate;

            TxBUserInfo.Text = "(#" + userToUpdate.Id + ") " + userToUpdate.Email;
            TBId.Text = userToUpdate.Id.ToString();
            TBFirstname.Text = userToUpdate.Firstname;
            TBLastname.Text = userToUpdate.Lastname;
            TBEmail.Text = userToUpdate.Email;
            PBPassword.Password = userToUpdate.Password;
        }

        #endregion

        #region Events

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(TBId.Text, out id)) {
                User newUser = new User(id, TBFirstname.Text, TBLastname.Text, TBEmail.Text, PBPassword.Password);
                if (newUser.IsSavable()) {
                    if (id != _userToUpdate.Id && _usersModel.Exists("Id", id)) {
                        MessageBox.Show("The id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else if (_usersModel.Exists("Email", TBEmail.Text)) {
                        MessageBox.Show("This email is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        try {
                            _usersModel.UpdateUser(_userToUpdate, newUser);
                            _userToUpdate.Hydrate(newUser);
                            MessageBox.Show("The user has been updated.", "User updated !", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                } else {
                    SortedDictionary<string, string> errors = newUser.GetInvalidFields();
                    MessageBox.Show(errors.Values.First(), "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            } else {
                MessageBox.Show("The id must be a positive integer.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

    }

}

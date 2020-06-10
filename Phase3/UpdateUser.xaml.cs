using Core;
using Core.Elements;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<User> _users = new ObservableCollection<User>();

        private User _userToUpdate;

        private readonly UsersModel _usersModel = new UsersModel();

        #endregion

        #region Constructors

        public UpdateUser(ObservableCollection<User> users, User userToUpdate)
        {
            InitializeComponent();

            _users = users;
            _userToUpdate = userToUpdate;

            TxBInfo.Text = "(#" + userToUpdate.Id + ") " + userToUpdate.Email;
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
            string idStr = TBId.Text.Trim();
            string firstname = TBFirstname.Text.Trim();
            string lastname = TBLastname.Text.Trim();
            string email = TBEmail.Text.Trim();
            string password = PBPassword.Password;
            if (idStr.Equals("") || firstname.Equals("") || lastname.Equals("") || email.Equals("") || password.Trim().Equals("")) {
                MessageBox.Show("You must fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                if (int.TryParse(TBId.Text, out int id)) {
                    User newUser = new User(id, firstname, lastname, email, password, _userToUpdate.CreatedAt, DateTime.Now);
                    if (newUser.IsSavable()) {
                        if (id != _userToUpdate.Id && _usersModel.Exists<User>("Id", id)) {
                            MessageBox.Show("The id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        } else if (!email.Equals(_userToUpdate.Email) && _usersModel.Exists<User>("Email", TBEmail.Text)) {
                            MessageBox.Show("This email is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        } else {
                            try {
                                Registry.MoveUserRegistry(_userToUpdate.Id.ToString(), id.ToString());
                                Dictionary<string, object> conditions = new Dictionary<string, object>();
                                conditions.Add("Id", _userToUpdate.Id);
                                _usersModel.Update<User>(newUser, conditions);
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
        }

        #endregion

    }

}

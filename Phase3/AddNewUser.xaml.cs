using Core.Elements;
using Core.Helpers;
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
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {

        #region Properties

        private ObservableCollection<User> _users = new ObservableCollection<User>();

        private readonly UsersModel _usersModel = new UsersModel();

        #endregion

        #region Constructors

        public AddNewUser(ObservableCollection<User> users)
        {
            InitializeComponent();

            _users = users;

            TBId.Text = _usersModel.GetNextId(users).ToString();
        }

        #endregion

        #region Events

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string idStr = TBId.Text.Trim();
            string firstname = TBFirstname.Text.Trim();
            string lastname = TBLastname.Text.Trim();
            string email = TBEmail.Text.Trim();
            string password = PBPassword.Password;
            if (idStr.Equals("") || firstname.Equals("") || lastname.Equals("") || email.Equals("") || password.Equals("")) {
                MessageBox.Show("You must fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                if (int.TryParse(idStr, out int id)) {
                    User newUser = new User(id, firstname, lastname, email, password);
                    if (newUser.IsSavable()) {
                        if (!Functions.IsPasswordValid(password)) {
                            MessageBox.Show("This password format is invalid.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        } else if (!Functions.IsEmailValid(email)) {
                            MessageBox.Show("This email format is invalid.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        } else if (_usersModel.Exists<User>("Id", id)) {
                            MessageBox.Show("This id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        } else if (_usersModel.Exists<User>("Email", email)) {
                            MessageBox.Show("This email is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        } else {
                            try {
                                _usersModel.Add<User>(newUser);
                                _users.Add(newUser);
                                MessageBox.Show("The user has been added.", "User added !", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

    }

}

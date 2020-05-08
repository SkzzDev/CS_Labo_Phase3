using Phase3.Core.Elements;
using Phase3.Core.Models;
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

        private List<User> users = new List<User>();

        #endregion

        #region Constructors

        public Users()
        {
            InitializeComponent();

            UsersModel usersModel = new UsersModel();
            users = usersModel.GetAll();

            DGUsers.ItemsSource = users;
        }

        #endregion

        #region Events

        #endregion

        #region Functions

        #endregion

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}

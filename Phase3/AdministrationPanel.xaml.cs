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
using System.Windows.Shapes;

namespace Phase3
{
    /// <summary>
    /// Interaction logic for AdministrationPanel.xaml
    /// </summary>
    public partial class AdministrationPanel : Window
    {

        #region Properties

        private readonly User _userConnected = null;

        private ObservableCollection<User> _users = new ObservableCollection<User>();
        private ObservableCollection<Competition> _competitions = new ObservableCollection<Competition>();
        private ObservableCollection<Shooter> _shooters = new ObservableCollection<Shooter>();
        private ObservableCollection<Country> _countries = new ObservableCollection<Country>();

        #endregion

        #region Constructors

        public AdministrationPanel(User userConnected)
        {
            InitializeComponent();

            UsersModel usersModel = new UsersModel();
            _users = usersModel.GetAll<User>();

            CompetitionsModel competitionsModel = new CompetitionsModel();
            _competitions = competitionsModel.GetAll<Competition>();

            ShootersModel shootersModel = new ShootersModel();
            _shooters = shootersModel.GetAll<Shooter>();

            CountriesModel countriesModel = new CountriesModel();
            _countries = countriesModel.GetAll<Country>();

            _userConnected = userConnected;
            Fullname.DataContext = _userConnected;
            ImgProfilePicture.DataContext = _userConnected;

            Main.Content = new Views.Index(_shooters, _competitions);
        }

        #endregion

        #region Events

        private void MI_Nothing_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void LVIndex_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Index(_shooters, _competitions);
        }

        private void LVCompetitions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Competitions(_competitions);
        }

        private void LVUsers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Users(_users);
        }

        private void LVShooters_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Shooters(_shooters, _countries);
        }

        private void LVResutls_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Views.Results(_competitions);
        }

        #endregion

        #region Functions

        #endregion
    }

}

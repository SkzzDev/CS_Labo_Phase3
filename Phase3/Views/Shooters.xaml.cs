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
    /// Interaction logic for Shooters.xaml
    /// </summary>
    public partial class Shooters : UserControl
    {

        #region Properties

        private ObservableCollection<Shooter> _shooters = new ObservableCollection<Shooter>();
        private ObservableCollection<Country> _countries = new ObservableCollection<Country>();

        #endregion

        #region Constructors

        public Shooters(ObservableCollection<Shooter> shooters, ObservableCollection<Country> countries)
        {
            InitializeComponent();

            _shooters = shooters;
            _countries = countries;

            DGShooters.ItemsSource = _shooters;
        }

        #endregion

        #region Events

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            ShootersModel shootersModel = new ShootersModel();
            _shooters.Clear();
            foreach (Shooter Shooter in shootersModel.GetAll<Shooter>()) {
                _shooters.Add(Shooter);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewShooter addNewShooter = new AddNewShooter(_shooters, _countries);
            addNewShooter.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Shooter shooter = DGShooters.SelectedItem as Shooter;
            if (shooter != null) {
                UpdateShooter updateShooter = new UpdateShooter(_shooters, _countries, shooter);
                updateShooter.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Shooter shooter = DGShooters.SelectedItem as Shooter;
            if (shooter != null) {
                ShootersModel shootersModel = new ShootersModel();
                try {
                    Dictionary<string, object> conditions = new Dictionary<string, object>();
                    conditions.Add("Id", shooter.Id);
                    shootersModel.Delete<Shooter>(conditions);
                    _shooters.Remove(shooter);
                    MessageBox.Show("The shooter has been deleted.", "Shooter deleted !", MessageBoxButton.OK, MessageBoxImage.Information);
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

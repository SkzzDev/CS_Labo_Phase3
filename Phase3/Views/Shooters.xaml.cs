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

        private ObservableCollection<Shooter> _shooters;
        private ObservableCollection<Country> _countries;
        private ShootersModel _shootersModel = new ShootersModel();

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
            ObservableCollection<Shooter> newShooters = _shootersModel.GetAll<Shooter>();
            _shooters.Clear();
            foreach (Shooter shooter in newShooters)
                _shooters.Add(shooter);
            DGShooters.SelectedIndex = -1;
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
            DGShooters.SelectedIndex = -1;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Shooter shooter = DGShooters.SelectedItem as Shooter;
            if (shooter != null) {
                try {
                    Dictionary<string, object> conditions = new Dictionary<string, object>();
                    conditions.Add("Id", shooter.Id);
                    _shootersModel.Delete<Shooter>(conditions);
                    _shooters.Remove(shooter);
                    MessageBox.Show("The shooter has been deleted.", "Shooter deleted !", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            DGShooters.SelectedIndex = -1;
        }

        #endregion

        #region Functions

        #endregion

    }

}

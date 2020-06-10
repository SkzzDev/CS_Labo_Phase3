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
    /// Interaction logic for AddNewShooter.xaml
    /// </summary>
    public partial class AddNewShooter : Window
    {

        #region Properties

        private ObservableCollection<Shooter> _shooters = new ObservableCollection<Shooter>();

        private readonly ShootersModel _shootersModel = new ShootersModel();

        #endregion

        #region Constructors

        public AddNewShooter(ObservableCollection<Shooter> shooters, ObservableCollection<Country> countries)
        {
            InitializeComponent();

            CBCountries.ItemsSource = countries;
            CBCountries.SelectedIndex = 0;

            _shooters = shooters;
        }

        #endregion

        #region Events

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string id = TBId.Text.Trim();
            string firstname = TBFirstname.Text.Trim();
            string lastname = TBLastname.Text.Trim();
            if (id.Equals("") || firstname.Equals("") || lastname.Equals("") || DPBirthday.SelectedDate == null) {
                MessageBox.Show("You must fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                DateTime birthday = (DateTime)DPBirthday.SelectedDate;
                Shooter newShooter = new Shooter(id, firstname, lastname, birthday, (Country)CBCountries.SelectedItem);
                if (newShooter.IsSavable()) {
                    if (_shootersModel.Exists<Shooter>("Id", id)) {
                        MessageBox.Show("This id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        try {
                            _shootersModel.Add<Shooter>(newShooter);
                            _shooters.Add(newShooter);
                            MessageBox.Show("The Shooter has been added.", "Shooter added !", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                } else {
                    SortedDictionary<string, string> errors = newShooter.GetInvalidFields();
                    MessageBox.Show(errors.Values.First(), "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
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

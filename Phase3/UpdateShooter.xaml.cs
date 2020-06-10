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
    /// Interaction logic for UpdateShooter.xaml
    /// </summary>
    public partial class UpdateShooter : Window
    {

        #region Properties

        private ObservableCollection<Shooter> _shooters = new ObservableCollection<Shooter>();

        private Shooter _shooterToUpdate;

        private readonly ShootersModel _shootersModel = new ShootersModel();
        private readonly CountriesModel _countriesModel = new CountriesModel();

        #endregion

        #region Constructors

        public UpdateShooter(ObservableCollection<Shooter> shooters, ObservableCollection<Country> countries, Shooter shooterToUpdate)
        {
            InitializeComponent();

            CBCountries.ItemsSource = countries;
            Country countryRef = _countriesModel.GetCountryRefInsideList(shooterToUpdate.Nationality, countries);
            if (countryRef != null) {
                CBCountries.SelectedItem = countryRef;
            } else {
                CBCountries.SelectedIndex = 0;
            }

            _shooters = shooters;
            _shooterToUpdate = shooterToUpdate;

            TxBInfo.Text = "(" + shooterToUpdate.Id + ") " + shooterToUpdate.Firstname[0] + ". " + shooterToUpdate.Lastname[0] + ".";
            TBId.Text = shooterToUpdate.Id.ToString();
            TBFirstname.Text = shooterToUpdate.Firstname;
            TBLastname.Text = shooterToUpdate.Lastname;
            DPBirthday.SelectedDate = shooterToUpdate.Birthday;
        }

        #endregion

        #region Events

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string id = TBId.Text.Trim();
            string firstname = TBFirstname.Text.Trim();
            string lastname = TBLastname.Text.Trim();
            if (id.Equals("") || firstname.Equals("") || lastname.Equals("") || DPBirthday.SelectedDate == null) {
                MessageBox.Show("You must fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                DateTime birthday = (DateTime)DPBirthday.SelectedDate;
                Shooter newShooter = new Shooter(id, firstname, lastname, birthday, (Country)CBCountries.SelectedItem, _shooterToUpdate.CreatedAt, DateTime.Now);
                if (newShooter.IsSavable()) {
                    if (!id.Equals(_shooterToUpdate.Id) && _shootersModel.Exists<Shooter>("Id", id)) {
                        MessageBox.Show("This id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        try {
                            if (!id.Equals(_shooterToUpdate.Id)) {
                                _shootersModel.UpdateReferences(_shooterToUpdate.Id, id);
                            }
                            Dictionary<string, object> conditions = new Dictionary<string, object>();
                            conditions.Add("Id", _shooterToUpdate.Id);
                            _shootersModel.Update<Shooter>(newShooter, conditions);
                            _shooterToUpdate.Hydrate(newShooter);
                            MessageBox.Show("The shooter has been updated.", "Shooter updated !", MessageBoxButton.OK, MessageBoxImage.Information);
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

        #endregion

    }

}

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
    /// Interaction logic for UpdateCompetition.xaml
    /// </summary>
    public partial class UpdateCompetition : Window
    {

        #region Properties

        private ObservableCollection<Competition> _competitions = new ObservableCollection<Competition>();

        private Competition _competitionToUpdate;

        private readonly CompetitionsModel _competitionsModel = new CompetitionsModel();

        #endregion

        #region Constructors

        public UpdateCompetition(ObservableCollection<Competition> competitions, Competition competitionToUpdate)
        {
            InitializeComponent();

            _competitions = competitions;
            _competitionToUpdate = competitionToUpdate;

            TxBInfo.Text = "(#" + competitionToUpdate.Id + ") " + competitionToUpdate.Name;
            TBId.Text = competitionToUpdate.Id.ToString();
            TBName.Text = competitionToUpdate.Name;
            DPStartDate.SelectedDate = competitionToUpdate.StartDate;
            DPEndDate.SelectedDate = competitionToUpdate.EndDate;
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
            string name = TBName.Text.Trim();
            if (idStr.Equals("") || name.Equals("") || DPStartDate.SelectedDate == null || DPEndDate.SelectedDate == null) {
                MessageBox.Show("You must fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                if (int.TryParse(idStr, out int id)) {
                    DateTime startDate = (DateTime)DPStartDate.SelectedDate;
                    DateTime endDate = (DateTime)DPEndDate.SelectedDate;
                    if (endDate < startDate) {
                        MessageBox.Show("The ending date can't be before the starting date.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        Competition newCompetition = new Competition(id, name, startDate, endDate, _competitionToUpdate.CreatedAt, DateTime.Now);
                        if (newCompetition.IsSavable()) {
                            if (id != _competitionToUpdate.Id && _competitionsModel.Exists<Competition>("Id", id)) {
                                MessageBox.Show("The id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                            } else {
                                try {
                                    if (id != _competitionToUpdate.Id) {
                                        // Rename competition's results' file if the id changed
                                        string oldResultsFilename = Functions.GetXmlFilePath("results/" + _competitionToUpdate.Id.ToString());
                                        string newResultsFilename = Functions.GetXmlFilePath("results/" + newCompetition.Id.ToString());
                                        System.IO.File.Move(oldResultsFilename, newResultsFilename);
                                    }
                                    Dictionary<string, object> conditions = new Dictionary<string, object>();
                                    conditions.Add("Id", _competitionToUpdate.Id);
                                    _competitionsModel.Update<Competition>(newCompetition, conditions);
                                    _competitionToUpdate.Hydrate(newCompetition);
                                    MessageBox.Show("The competition has been updated.", "Competition updated !", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Close();
                                } catch (Exception ex) {
                                    MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                        } else {
                            SortedDictionary<string, string> errors = newCompetition.GetInvalidFields();
                            MessageBox.Show(errors.Values.First(), "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                } else {
                    MessageBox.Show("The id must be a positive integer.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        #endregion

    }

}

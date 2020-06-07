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
            if (int.TryParse(TBId.Text, out int id)) {
                if (DPStartDate.SelectedDate == null) {
                    MessageBox.Show("You must select a starting date.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                } else if (DPStartDate.SelectedDate == null) {
                    MessageBox.Show("You must select a ending date.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                } else {
                    DateTime startDate = (DateTime)DPStartDate.SelectedDate;
                    DateTime endDate = (DateTime)DPEndDate.SelectedDate;
                    if (endDate < startDate) {
                        MessageBox.Show("The ending date can't be before the starting date.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        Competition newCompetition = new Competition(id, TBName.Text, startDate, endDate, _competitionToUpdate.CreatedAt, DateTime.Now);
                        if (newCompetition.IsSavable()) {
                            if (id != _competitionToUpdate.Id && _competitionsModel.Exists<Competition>("Id", id)) {
                                MessageBox.Show("The id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                            } else {
                                try {
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
                }
            } else {
                MessageBox.Show("The id must be a positive integer.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

    }

}

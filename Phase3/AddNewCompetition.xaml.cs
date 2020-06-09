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
    /// Interaction logic for AddNewCompetition.xaml
    /// </summary>
    public partial class AddNewCompetition : Window
    {

        #region Properties

        private ObservableCollection<Competition> _competitions = new ObservableCollection<Competition>();

        private readonly CompetitionsModel _competitionsModel = new CompetitionsModel();

        #endregion

        #region Constructors

        public AddNewCompetition(ObservableCollection<Competition> competitions)
        {
            InitializeComponent();

            _competitions = competitions;

            TBId.Text = _competitionsModel.GetNextId(_competitions).ToString();
        }

        #endregion

        #region Events

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TBId.Text.Trim(), out int id)) {
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
                        string name = TBName.Text.Trim();
                        Competition newCompetition = new Competition(id, name, startDate, endDate, DateTime.Now, DateTime.Now);
                        if (newCompetition.IsSavable()) {
                            if (_competitionsModel.Exists<Competition>("Id", id)) {
                                MessageBox.Show("This id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                            } else {
                                try {
                                    _competitionsModel.Add<Competition>(newCompetition);
                                    _competitions.Add(newCompetition);
                                    MessageBox.Show("The competition has been added.", "Competition added !", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }

}

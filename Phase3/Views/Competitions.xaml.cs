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
    /// Interaction logic for Competitions.xaml
    /// </summary>
    public partial class Competitions : UserControl
    {

        #region Properties

        private ObservableCollection<Competition> _competitions;
        private CompetitionsModel _competitionsModel = new CompetitionsModel();

        #endregion

        #region Constructors

        public Competitions(ObservableCollection<Competition> competitions)
        {
            InitializeComponent();

            _competitions = competitions;

            DGCompetitions.ItemsSource = _competitions;
        }

        #endregion

        #region Events

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Competition> newCompetitions = _competitionsModel.GetAll<Competition>();
            _competitions.Clear();
            foreach (Competition competition in newCompetitions)
                _competitions.Add(competition);
            DGCompetitions.SelectedIndex = -1;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewCompetition addNewCompetition = new AddNewCompetition(_competitions);
            addNewCompetition.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Competition competition = DGCompetitions.SelectedItem as Competition;
            if (competition != null) {
                UpdateCompetition updateCompetition = new UpdateCompetition(_competitions, competition);
                updateCompetition.ShowDialog();
            }
            DGCompetitions.SelectedIndex = -1;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Competition competition = DGCompetitions.SelectedItem as Competition;
            if (competition != null) {
                try {
                    Dictionary<string, object> conditions = new Dictionary<string, object>();
                    conditions.Add("Id", competition.Id);
                    _competitionsModel.Delete<Competition>(conditions);
                    _competitions.Remove(competition);
                    MessageBox.Show("The competition has been deleted.", "Competition deleted !", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            DGCompetitions.SelectedIndex = -1;
        }

        #endregion

        #region Functions

        #endregion

    }

}

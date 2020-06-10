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
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : UserControl
    {

        #region Properties

        private ObservableCollection<Competition> _competitions;
        private CompetitionsModel _competitionsModel = new CompetitionsModel();

        #endregion

        #region Constructors

        public Results(ObservableCollection<Competition> competitions)
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

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            Competition competition = DGCompetitions.SelectedItem as Competition;
            if (competition != null) {
                ViewCompetition viewCompetition = new ViewCompetition(competition);
                viewCompetition.ShowDialog();
            }
            DGCompetitions.SelectedIndex = -1;
        }

        #endregion

        #region Functions

        #endregion
    }

}

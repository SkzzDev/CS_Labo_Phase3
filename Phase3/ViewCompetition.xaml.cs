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
    /// Interaction logic for ViewCompetition.xaml
    /// </summary>
    public partial class ViewCompetition : Window
    {
        #region Properties

        private readonly Competition _competition;
        private readonly ResultsModel _resultsModel;
        private ObservableCollection<Result> _results;

        #endregion

        #region Constructors

        public ViewCompetition(Competition competition)
        {
            InitializeComponent();

            Title = "SRA - #" + competition.Id.ToString() + " « " + competition.Name + " »";

            _competition = competition;

            TxBCompetitionName.Text = "« " + competition.Name + " »";
            TxBStartDate.Text = competition.StartDate.ToString("d MMMM yyyy");
            TxBEndDate.Text = competition.EndDate.ToString("d MMMM yyyy");

            _resultsModel = new ResultsModel(competition.Id);
            _resultsModel.CreateIfDontExist();
            _results = _resultsModel.GetAll<Result>();

            DGResults.ItemsSource = _results;
        }

        #endregion

        #region Events

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Result> newResults = _resultsModel.GetAll<Result>();
            _results.Clear();
            foreach (Result result in newResults)
                _results.Add(result);
            DGResults.SelectedIndex = -1;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewResult addNewResult = new AddNewResult(_results, _resultsModel);
            addNewResult.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Result result = DGResults.SelectedItem as Result;
            if (result != null) {
                // UpdateResult updateResult = new UpdateResult(_results, result);
                // updateResult.ShowDialog();
            }
            DGResults.SelectedIndex = -1;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Result result = DGResults.SelectedItem as Result;
            if (result != null) {
                try {
                    Dictionary<string, object> conditions = new Dictionary<string, object>();
                    conditions.Add("ShootedById", result.ShootedById);
                    _resultsModel.Delete<Result>(conditions);
                    _results.Remove(result);
                    MessageBox.Show("The result has been deleted.", "Result deleted !", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            DGResults.SelectedIndex = -1;
        }

        #endregion

        #region Functions

        #endregion

    }
}

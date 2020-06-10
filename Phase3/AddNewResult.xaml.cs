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
    /// Interaction logic for AddNewResult.xaml
    /// </summary>
    public partial class AddNewResult : Window
    {

        #region Properties

        private readonly ObservableCollection<Result> _results = new ObservableCollection<Result>();

        private readonly ShootersModel _shootersModel = new ShootersModel();
        private readonly ResultsModel _resultsModel;

        #endregion

        #region Constructors

        public AddNewResult(ObservableCollection<Result> results, ResultsModel resultsModel)
        {
            InitializeComponent();

            // Model passé en paramètre car il comporte un DataFile précis qu'il faudrait recréer ici.. Plus simple ainsi
            _resultsModel = resultsModel;
            _results = results;
        }

        #endregion

        #region Events

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string id = TBId.Text.Trim();
            string serie1Str = TBSerie1.Text.Trim();
            string serie2Str = TBSerie2.Text.Trim();
            string serie3Str = TBSerie3.Text.Trim();
            string serie4Str = TBSerie4.Text.Trim();
            if (id.Equals("") || serie1Str.Equals("") || serie2Str.Equals("") || serie3Str.Equals("") || serie4Str.Equals("")) {
                MessageBox.Show("You must fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                if (!_shootersModel.Exists<Shooter>("Id", id)) {
                    MessageBox.Show("This shooter id does not exist.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                } else {
                    if (_resultsModel.Exists<Result>("ShootedById", id)) {
                        MessageBox.Show("This shooter already have results for this competition.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        if (float.TryParse(serie1Str, out float serie1) && float.TryParse(serie2Str, out float serie2) && float.TryParse(serie3Str, out float serie3) && float.TryParse(serie4Str, out float serie4)) {
                            Result newResult = new Result(serie1, serie2, serie3, serie4, id);
                            if (newResult.IsSavable()) {
                                try {
                                    _resultsModel.Add<Result>(newResult);
                                    _results.Add(newResult);
                                    MessageBox.Show("The shooter's results have been added.", "Results added !", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Close();
                                } catch (Exception ex) {
                                    MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            } else {
                                SortedDictionary<string, string> errors = newResult.GetInvalidFields();
                                MessageBox.Show(errors.Values.First(), "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        } else {
                            MessageBox.Show("All series should be int or float (sep = ,)", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
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

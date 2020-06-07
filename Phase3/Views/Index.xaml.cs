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
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : UserControl
    {

        #region Properties

        #endregion

        #region Constructors

        public Index(ObservableCollection<Shooter> shooters, ObservableCollection<Competition> competitions)
        {
            InitializeComponent();

            int shootersNumber = shooters.Count, competitionsNumber = competitions.Count;

            Stats.Text = "";

            Stats.Inlines.Add("Currently, ");

            if (shootersNumber == 0) {
                Stats.Inlines.Add("no shooter is registered");
            } else if (shootersNumber == 1) {
                Stats.Inlines.Add(new Run("1") { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" shooter is registered");
            } else if (shootersNumber > 1) {
                Stats.Inlines.Add(new Run(shootersNumber.ToString()) { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" shooters are registered");
            }

            Stats.Inlines.Add(" et ");

            if (competitionsNumber == 0) {
                Stats.Inlines.Add("no competition has been organised");
            } else if (competitionsNumber == 1) {
                Stats.Inlines.Add(new Run("1") { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" competition has been organised");
            } else if (competitionsNumber > 1) {
                Stats.Inlines.Add(new Run(competitionsNumber.ToString()) { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" competitions have been organised");
            }

            Stats.Inlines.Add(".");
        }

        #endregion

        #region Events

        #endregion

        #region Functions

        #endregion

    }

}

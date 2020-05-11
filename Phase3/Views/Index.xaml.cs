using Core.Models;
using System;
using System.Collections.Generic;
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

        public Index()
        {
            InitializeComponent();

            RecalculateStats();
        }

        #endregion

        #region Events

        #endregion

        #region Functions

        public void RecalculateStats()
        {
            ShootersModel shootersModel = new ShootersModel();
            int shootersNumber = shootersModel.GetNumberOfShooters();

            CompetitionsModel competitionsModel = new CompetitionsModel();
            int compsNumber = competitionsModel.GetNumberOfCompetitions();

            Stats.Text = "";

            Stats.Inlines.Add("Currently, ");

            if (shootersNumber == 0) {
                Stats.Inlines.Add("no shooter is registered");
            } else if(shootersNumber == 1) {
                Stats.Inlines.Add(new Run("1") { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" shooter is registered");
            } else if (shootersNumber > 1) {
                Stats.Inlines.Add(new Run(shootersNumber.ToString()) { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" shooters are registered");
            }

            Stats.Inlines.Add(" et ");

            if (shootersNumber == 0) {
                Stats.Inlines.Add("no competition has been organised");
            } else if (compsNumber == 1) {
                Stats.Inlines.Add(new Run("1") { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" competition has been organised");
            } else if (compsNumber > 1) {
                Stats.Inlines.Add(new Run(compsNumber.ToString()) { FontWeight = FontWeights.Bold });
                Stats.Inlines.Add(" competitions have been organised");
            }

            Stats.Inlines.Add(".");
        }

        #endregion

    }

}

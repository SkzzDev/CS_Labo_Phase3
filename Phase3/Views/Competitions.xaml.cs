using Core.Elements;
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
    /// Interaction logic for Competitions.xaml
    /// </summary>
    public partial class Competitions : UserControl
    {

        #region Properties

        private List<Competition> _competitions = new List<Competition>();

        #endregion

        #region Constructors

        public Competitions(List<Competition> competitions)
        {
            InitializeComponent();

            _competitions = competitions;

            DGCompetitions.ItemsSource = _competitions;
        }

        #endregion

        #region Events

        #endregion

        #region Functions

        #endregion

    }

}

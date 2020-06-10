using Core;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();

            TBXmlsPath.Text = Registry.GetXmlsPath();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            string xmlsPath = TBXmlsPath.Text.Trim();
            if (!xmlsPath.Equals("")) {
                if (Directory.Exists(xmlsPath)) {
                    Registry.SetXmlsPath(xmlsPath);
                    MessageBox.Show("The default xmls' dir path has been changed.", "Updated!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                } else {
                    MessageBox.Show("This directory does not exist.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            } else {
                MessageBox.Show("Please fill all the fields.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

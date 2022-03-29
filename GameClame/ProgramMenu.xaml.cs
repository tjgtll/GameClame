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
using System.Windows.Shapes;

namespace GameClame
{
    /// <summary>
    /// Interaction logic for ProgramMenu.xaml
    /// </summary>
    public partial class ProgramMenu : Window
    {
        public ProgramMenu()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            MainWindow OP = new MainWindow();
            OP.Show();
        }
    }
}

using GameClame.Model;
using GameClame.Model.Tetris;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;

namespace GameClame
{
    /// <summary>
    /// Interaction logic for ProgramMenu.xaml
    /// </summary>
    public partial class ProgramMenu : Window
    {
        public ObservableCollection<Highscore> HighscoreList
        {
            get; set;
        } = new ObservableCollection<Highscore>();

        public ProgramMenu()
        {
            LoadHighscoreList();
            InitializeComponent();
        }

        private void FlappyBird_Click(object sender, RoutedEventArgs e)
        {
            FlappyBird OP = new FlappyBird();
            OP.Show();
        }

        private void Tetris_Click(object sender, RoutedEventArgs e)
        {
            TetrisWindow OP = new TetrisWindow();
            OP.Show();
        }

        private void TicTac_Click(object sender, RoutedEventArgs e)
        {
            Tic_TacWindow OP = new Tic_TacWindow();
            OP.Show();
        }

        private void Highscore_Click(object sender, RoutedEventArgs e)
        {
            LoadHighscoreList();
            bdrHighscoreList.Visibility = Visibility.Visible;
        }

        private void LoadHighscoreList()
        {
            if (File.Exists("highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
                using (Stream reader = new FileStream("highscorelist.xml", FileMode.Open))
                {
                    List<Highscore> tempList = (List<Highscore>)serializer.Deserialize(reader);
                    this.HighscoreList.Clear();
                    foreach (var item in tempList.OrderByDescending(x => x.Score))
                        this.HighscoreList.Add(item);
                }
            }


        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            bdrHighscoreList.Visibility = Visibility.Collapsed;
        }
    }
}

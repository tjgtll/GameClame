﻿using GameClame.Model;
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
    }
}

using GameClame.Model.Tetris;
using GameClame.Tetris.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace GameClame
{
    /// <summary>
    /// Interaction logic for TetrisWindow.xaml
    /// </summary>
    public partial class TetrisWindow : Window
    {
        private bool IsPause = false;

        private bool IsSoundOff = true;

        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/Tetris/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] backgroundImage = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/Tetris/Background.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Background1.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/Tetris/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Tetris/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 20;

        private GameState gameState = new GameState();

        private MediaPlayer mediaPlayer = new MediaPlayer();

        public ObservableCollection<Highscore> HighscoreList
        {
            get; set;
        } = new ObservableCollection<Highscore>();

        public TetrisWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.TetrisGrid);
            mediaPlayer.Open(new Uri("Sound/ostTheme.wav", UriKind.RelativeOrAbsolute));
            mediaPlayer.Volume = 0.5;
            mediaPlayer.MediaEnded += MediaPlayer_Ended;
            mediaPlayer.Play();

        }

        private void MediaPlayer_Ended(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private Image[,] SetupGameCanvas(TetrisGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(TetrisGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.TetrisGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Счет: {gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                if (IsPause)
                {
                    return;
                }

                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Счет: {gameState.Score}";
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                case Key.Escape:
                    Pause(sender,e);
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            int lowestHighscore = (this.HighscoreList.Count > 0 ? this.HighscoreList.Min(x => x.Score) : 0);
            var value = Convert.ToInt32(FinalScoreText.Text[6.. (FinalScoreText.Text.Length)]);
            if (value > lowestHighscore)
            {
                HighscoreList.Add(new Highscore()
                {
                    Score = value,
                    PlayerName  = this.txtPlayerName.Text
                });
                SaveHighscoreList();
            }
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void SaveHighscoreList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Highscore>));
            using (Stream writer = new FileStream("highscorelist.xml", FileMode.Create))
            {
                serializer.Serialize(writer, this.HighscoreList);
            }
        }

        private async void Pause(object sender, RoutedEventArgs e)
        {
            if (IsPause)
            {
                IsPause = false;
                Draw(gameState);
                GamePauseMenu.Visibility = Visibility.Hidden;
                bdrHighscoreList.Visibility = Visibility.Collapsed;
                await GameLoop();
                return;
            }
            IsPause = true;
            GamePauseMenu.Visibility = Visibility.Visible;
            PauseScoreText.Text = $"Счет: {gameState.Score}";
        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Close();
            this.Close();
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            IsPause = false;
            Draw(gameState);
            GamePauseMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private async void RePlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            IsPause = false;
            mediaPlayer.Play();
            GamePauseMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void Exit_Ended(object sender, EventArgs e)
        {
            mediaPlayer.Close();
            this.Close();
        }

        private void Music_Click(object sender, RoutedEventArgs e)
        {
            if (IsSoundOff)
            {
                mediaPlayer.Stop();
                IsSoundOff = false;
            }
            else
            {
                mediaPlayer.Play();
                IsSoundOff = true;
            }
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

        private void Highscore_Click(object sender, RoutedEventArgs e)
        {
            LoadHighscoreList();
            bdrHighscoreList.Visibility = Visibility.Visible;
        }
    }
}

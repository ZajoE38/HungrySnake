using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class frmSnake : Form
    {
        #region Variables
        enum GameItem { Free, Snake, Bonus };
        enum Direction { Up, Down, Left, Right };
        struct Coordinates
        {
            public int x;
            public int y;
        }

        int snakeLength;
        Direction headDirection;
        GameItem[,] gameBoard;        
        Coordinates[] snakeXY;
        Graphics graphics;
        Random random;
        #endregion


        public frmSnake()
        {
            InitializeComponent();
            gameBoard = new GameItem[11, 11];
            snakeXY = new Coordinates[100];
            random = new Random();
        }


        private void frmSnake_Load(object sender, EventArgs e)
        {
            canvas.Image = new Bitmap(420, 420);
            graphics = Graphics.FromImage(canvas.Image);
            graphics.Clear(Color.White);

            for (int i = 1; i <= 10; i++)
            {
                graphics.DrawImage(imgList.Images[6], i * 35, 0);
                graphics.DrawImage(imgList.Images[6], i * 35, 385);
            }
            for (int i = 0; i <= 11; i++)
            {                
                graphics.DrawImage(imgList.Images[6], 0, i * 35);
                graphics.DrawImage(imgList.Images[6], 385, i * 35);
            }

            snakeXY[0].x = 5;
            snakeXY[0].y = 5;
            snakeXY[1].x = 5;
            snakeXY[1].y = 6;
            snakeXY[2].x = 5;
            snakeXY[2].y = 7;

            graphics.DrawImage(imgList.Images[5], 5 * 35, 5 * 35);
            graphics.DrawImage(imgList.Images[4], 5 * 35, 6 * 35);
            graphics.DrawImage(imgList.Images[4], 5 * 35, 7 * 35);

            gameBoard[5, 5] = GameItem.Snake;
            gameBoard[5, 6] = GameItem.Snake;
            gameBoard[5, 7] = GameItem.Snake;

            headDirection = Direction.Up;
            snakeLength = 3;

            for (int i = 0; i < 4; i++) Bonus();                      
        }

        private void Bonus()
        {
            int bonusType = random.Next(0, 4);
            int x;
            int y;

            do
            {
                x = random.Next(1, 10);
                y = random.Next(1, 10);
            }
            while (gameBoard[x,y] != GameItem.Free);

            graphics.DrawImage(imgList.Images[bonusType], x * 35, y * 35);
            gameBoard[x, y] = GameItem.Bonus;
        }

        private void GameOver(string str)
        {
            timer.Enabled = false;
            MessageBox.Show("Game Over. " + str);
        }

        private void frmSnake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    headDirection = Direction.Up;
                    break;
                case Keys.Down:
                    headDirection = Direction.Down;
                    break;
                case Keys.Left:
                    headDirection = Direction.Left;
                    break;
                case Keys.Right:
                    headDirection = Direction.Right;
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            graphics.FillRectangle(Brushes.White, snakeXY[snakeLength - 1].x * 35, snakeXY[snakeLength - 1].y * 35, 35, 35);
            gameBoard[snakeXY[snakeLength - 1].x, snakeXY[snakeLength - 1].y] = GameItem.Free;

            for (int i = snakeLength; i > 0; i--)
            {
                snakeXY[i].x = snakeXY[i - 1].x;
                snakeXY[i].y = snakeXY[i - 1].y;
            }

            graphics.DrawImage(imgList.Images[4], snakeXY[0].x * 35, snakeXY[0].y * 35);

            switch (headDirection)
            {
                case Direction.Up:
                    snakeXY[0].y = snakeXY[0].y - 1;
                    break;
                case Direction.Down:
                    snakeXY[0].y = snakeXY[0].y + 1;
                    break;
                case Direction.Left:
                    snakeXY[0].x = snakeXY[0].x - 1;
                    break;
                case Direction.Right:
                    snakeXY[0].x = snakeXY[0].x + 1;
                    break;
            }

            if (snakeXY[0].x < 1 || snakeXY[0].x > 10 || snakeXY[0].y < 1 || snakeXY[0].y > 10)
            {
                GameOver("You hit the wall.");
                return;
            }

            if (gameBoard[snakeXY[0].x, snakeXY[0].y] == GameItem.Snake)
            {
                GameOver("You hit your body.");
                return;
            }

            if (gameBoard[snakeXY[0].x, snakeXY[0].y] == GameItem.Bonus)
            {
                graphics.DrawImage(imgList.Images[4], snakeXY[snakeLength].x * 35, snakeXY[snakeLength].y * 35);
                gameBoard[snakeXY[snakeLength].x, snakeXY[snakeLength].y] = GameItem.Snake;
                snakeLength++;
                if (snakeLength < 96) Bonus();
            }

            graphics.DrawImage(imgList.Images[5], snakeXY[0].x * 35, snakeXY[0].y * 35);
            gameBoard[snakeXY[0].x, snakeXY[0].y] = GameItem.Snake;

            canvas.Refresh();
        }
    }
}
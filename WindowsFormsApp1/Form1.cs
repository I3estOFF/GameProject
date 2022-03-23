using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        Graphics g;
        List<Rectangle> PlatformHB = new List<Rectangle>(); // hitboxy platform
        Rectangle Player = new Rectangle(80, 200, Properties.Resources.Chungus.Width, Properties.Resources.Chungus.Height); //hitbox postaci
        Bitmap PlayerBitmap;
        bool playerRight = false;
        bool playerLeft = false;
        bool playerUp = false;
        bool isGrounded = false; // sprawdza czy gracz dotyka ziemi
        bool faceLeft = false; // zapamietuje ostatni kierunek ruchu
        int fallSpeed = 0;
        int jumpSpeed = 200;
        int playerSpeed = 5;
        //int scroll = 0;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            
            WindowState = FormWindowState.Maximized;
        }


        private void KeyU(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                playerLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                playerRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                playerUp = false;
                jumpSpeed = 200;
                isGrounded = false;
            }

        }

        private void KeyDow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                playerLeft = true;
                faceLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                playerRight = true;
                faceLeft=false;
            }
            if (e.KeyCode == Keys.Up && isGrounded)
            {
                playerUp = true;
                fallSpeed = 0;
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playerLeft == true)
            {
                Player.X -= playerSpeed;
            }
            if (playerRight == true && Player.X < this.Width)
            {
                Player.X += playerSpeed;
            }
            if (playerUp == true && isGrounded)
            {
                Player.Y -= jumpSpeed / 10;
                if (jumpSpeed > 0)
                    jumpSpeed -= 11;
                else if (jumpSpeed <= 0)
                {
                    playerUp = false;
                    fallSpeed += 50;
                }
            }

            if (Player.Y < this.Height && playerUp == false)
            {
                Player.Y += fallSpeed / 10;
                if (fallSpeed < 100)
                    fallSpeed += 9;
                isGrounded = false;
            }

            foreach (Rectangle hb in PlatformHB)
            {
                // kolizja gracza z gorna krawedzia zatrzymuje opadanie //dziala
                if (Player.Y+Player.Height < hb.Y+20 && Player.Y+Player.Height > hb.Y-4 &&
                    Player.X < hb.X + hb.Width+50 && Player.X > hb.X-50)
                {
                    fallSpeed = 0;
                    jumpSpeed = 200;
                    isGrounded = true;
                }
                //kolizja z dolna krawedzia zatrzymuje skok // dziala
                if (Player.Y < hb.Y + hb.Height + 40 && Player.Y > hb.Y + hb.Height &&
                    Player.X < hb.X + hb.Width && Player.X > hb.X)
                {
                    jumpSpeed = 0;
                }

                //kolizja z bocznymi krawedziami 
                if (Player.Contains(hb.X,hb.Y) || Player.Contains(hb.X+hb.Width, hb.Y + hb.Height))
                {
                        Player.Y--;
                }

            }



            pictureBox1.Image = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            generatePlatform(0, Height - 100, Width);

            generatePlatform(Width / 10, Height / 3, Width / 5);
            generatePlatform(2 * Width / 5, 2 * Height / 3, Width / 5);
            generatePlatform(3 * Width / 5, 2 * Height / 5, Width / 20);
            generatePlatform(Width / 3, Height / 10, Width / 20);

            PlayerBitmap = Properties.Resources.Chungus;

            if (faceLeft) // zmienia zwrot postaci
            {
                PlayerBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
 

            g.DrawImage(PlayerBitmap, Player.X, Player.Y);
            pictureBox1.Refresh();

        }

        int platWidth = Properties.Resources._31.Width;
        int platHeight = Properties.Resources._31.Height;

        private void generatePlatform(int posX, int posY, int _width)
        {

            g.DrawImage(Properties.Resources._31, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + platWidth, 1);
            PlatformHB.Add(rect);
            if (platWidth < _width)
            {
                generatePlatform(posX + platWidth, posY, _width - platWidth);
            }

        }

        private void generatePlatformRandom(int numberOf)
        {
            Random rand = new Random();
            for (int i = 0; i < numberOf; i++)
            {
                int ran1 = rand.Next(10, 700);
                int ran2 = rand.Next(0, 350);
                int ran3 = rand.Next(100, 200);
                generatePlatform(ran1, ran2, ran3);
            }
        }
    }
}

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
        List<Rectangle> CarrotHB = new List<Rectangle>();
        List<Rectangle> EmptyHB = new List<Rectangle>();
        Rectangle Player = new Rectangle(80, 200, Properties.Resources.Chungus.Width, Properties.Resources.Chungus.Height); //hitbox postaci
        Bitmap PlayerBitmap;
        bool playerRight = false;
        bool playerLeft = false;
        bool playerUp = false;
        bool isGrounded = false; // sprawdza czy gracz dotyka ziemi
        bool faceLeft = false; // zapamietuje ostatni kierunek ruchu
        bool mar1 = true;
        
        const int fallSpeedAcceleration = 9;
        const int jumpSpeedDeceleration = 20;
        const int maxJumpSpeed = 300;
        const int maxFallSpeed = 150;
        const int maxPlayerSpeed = 7;
        int fallSpeed = 0;
        int jumpSpeed = maxJumpSpeed;
        int playerSpeed = maxPlayerSpeed;
        int punkty = 0;
        //int scroll = 0;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            label1.Text = "Punkty: " + punkty;
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
        private void onFormLoad(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PlayerMovement();
            PlatformCollideWith(Player);

            generatePlatform(0, Height - 100, Width);

            generatePlatform(Width / 10, Height / 3, Width / 5);
            generatePlatform(2 * Width / 5, 2 * Height / 3, Width / 5);
            generatePlatform(3 * Width / 5, 2 * Height / 5, Width / 20);
            generatePlatform(Width / 3, Height / 10, Width / 20);



            bool GotIt = false; //Czy ma marchewke?
            foreach (Rectangle cb in CarrotHB)
            {
               
                if (Player.Contains(cb) && GotIt == false)
                {

                    label1.Text = "Punkty: " + punkty++;
                    GotIt = true;
                    generateEmpty(2 * Width / 4, 3 * Height / 5, Width / 80);
                    mar1 = false;
                    break;
                }
                else
                break;

            }
            if(mar1 == true)
            generateCarrot(2 * Width / 4, 3 * Height / 5, Width / 80);



            pictureBox1.Refresh();
            g.Clear(Color.Transparent);

        }

        int platWidth = Properties.Resources._31.Width;
        int platHeight = Properties.Resources._31.Height;

        int CarrotWidth = Properties.Resources.Carrot.Width;
        int CarrotHeight = Properties.Resources.Carrot.Height;

        int EmptyWidth = Properties.Resources.Carrot.Width;
        int EmptyHeight = Properties.Resources.Carrot.Height;

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

        private void generateCarrot(int posX, int posY, int _width)
        {

            g.DrawImage(Properties.Resources.Carrot, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + CarrotWidth, 1);
            CarrotHB.Add(rect);
            if (CarrotWidth < _width)
            {
                generateCarrot(posX + CarrotWidth, posY, _width - CarrotWidth);
            }
        }

        private void generateEmpty(int posX, int posY, int _width)
        {

            g.DrawImage(Properties.Resources.Empty, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + EmptyWidth, 1);
            EmptyHB.Add(rect);
            if (EmptyWidth < _width)
            {
                generateCarrot(posX + EmptyWidth, posY, _width - EmptyWidth);
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


        public void PlayerMovement()
        {
            PlayerBitmap = Properties.Resources.Chungus;

            if (faceLeft) // zmienia zwrot postaci
            {
                PlayerBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            g.DrawImage(PlayerBitmap, Player.X, Player.Y);

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
                    jumpSpeed -= jumpSpeedDeceleration;
                else if (jumpSpeed <= 0)
                {
                    playerUp = false;
                    fallSpeed += 50;
                }
            }

            if (Player.Y < this.Height && playerUp == false)
            {
                Player.Y += fallSpeed / 10;
                if (fallSpeed < maxFallSpeed)
                    fallSpeed += fallSpeedAcceleration;
                isGrounded = false;
            }
        }
        public void PlatformCollideWith(Rectangle Rb)
        {
            foreach (Rectangle hb in PlatformHB)
            {
                // kolizja gracza z gorna krawedzia zatrzymuje opadanie //dziala
                if (Rb.Y + Rb.Height < hb.Y + 20 && Rb.Y + Rb.Height > hb.Y - 4 &&
                    Rb.X < hb.X + hb.Width + 50 && Rb.X > hb.X - 50)
                {
                    fallSpeed = 0;
                    jumpSpeed = maxJumpSpeed;
                    isGrounded = true;
                }
                //kolizja z dolna krawedzia zatrzymuje skok // dziala
                if (Rb.Y < hb.Y + hb.Height + 40 && Rb.Y > hb.Y + hb.Height &&
                    Rb.X < hb.X + hb.Width && Rb.X > hb.X)
                {
                    jumpSpeed = 0;
                }

                //kolizja z bocznymi krawedziami 
                if (Rb.Contains(hb.X, hb.Y) || Rb.Contains(hb.X + hb.Width, hb.Y + hb.Height))
                {
                    Rb.Y--;
                }

            }
        }
    }
}

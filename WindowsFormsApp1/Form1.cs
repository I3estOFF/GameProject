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
        Graphics gBackground;
        Graphics gPlayer;
        Bitmap PlayerBitmap;
        List<Rectangle> PlatformHB = new List<Rectangle>(); // hitboxy platform
        List<Rectangle> CarrotHB = new List<Rectangle>();
        List<Rectangle> EmptyHB = new List<Rectangle>();
        Player p;

        public bool mar1 = true;
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
                p.playerLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                p.playerRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                p.playerUp = false;
                p.jumpSpeed = 200;
                p.isGrounded = false;
            }

        }

        private void KeyDow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                p.playerLeft = true;
                p.faceLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                p.playerRight = true;
                p.faceLeft=false;
            }
            if (e.KeyCode == Keys.Up && p.isGrounded)
            {
                p.playerUp = true;
                p.fallSpeed = 0;
            }

        }
        private void onFormLoad(object sender, EventArgs e)
        {
            pictureBoxBackground.Image = new Bitmap(this.Width, this.Height);
            gBackground = Graphics.FromImage(pictureBoxBackground.Image);
            pictureBoxPlayer.Image = new Bitmap(this.Width, this.Height);
            gPlayer = Graphics.FromImage(pictureBoxPlayer.Image);

            pictureBoxPlayer.Dock = DockStyle.Fill;
            pictureBoxPlayer.Parent = pictureBoxBackground;
            pictureBoxPlayer.BackColor = Color.Transparent;

            p = new Player(gPlayer, PlayerBitmap, Width,Height, PlatformHB);

            generatePlatform(0, Height - 100, Width);

            generatePlatform(Width / 10, Height / 3, Width / 5);
            generatePlatform(2 * Width / 5, 2 * Height / 3, Width / 5);
            generatePlatform(3 * Width / 5, 2 * Height / 5, Width / 20);
            generatePlatform(Width / 3, Height / 10, Width / 20);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            p.PlayerMovement();
            p.PlatformPlayerCollision();




            bool GotIt = false; //Czy ma marchewke?
            foreach (Rectangle cb in CarrotHB)
            {
               
                if (p.playerBox.Contains(cb) && GotIt == false)
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



            pictureBoxBackground.Refresh();
            //gBackground.Clear(Color.Transparent);
            pictureBoxPlayer.Refresh();
            gPlayer.Clear(Color.Transparent);

        }

        int platWidth = Properties.Resources._31.Width;
        int platHeight = Properties.Resources._31.Height;

        int CarrotWidth = Properties.Resources.Carrot.Width;
        int CarrotHeight = Properties.Resources.Carrot.Height;

        int EmptyWidth = Properties.Resources.Carrot.Width;
        int EmptyHeight = Properties.Resources.Carrot.Height;

        private void generatePlatform(int posX, int posY, int _width)
        {

            gBackground.DrawImage(Properties.Resources._31, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + platWidth, 1);
            PlatformHB.Add(rect);
            if (platWidth < _width)
            {
                generatePlatform(posX + platWidth, posY, _width - platWidth);
            }

        }

        private void generateCarrot(int posX, int posY, int _width)
        {

            gBackground.DrawImage(Properties.Resources.Carrot, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + CarrotWidth, 1);
            CarrotHB.Add(rect);
            if (CarrotWidth < _width)
            {
                generateCarrot(posX + CarrotWidth, posY, _width - CarrotWidth);
            }
        }

        private void generateEmpty(int posX, int posY, int _width)
        {

            gBackground.DrawImage(Properties.Resources.Empty, posX, posY);
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


      
    }
}

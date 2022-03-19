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
        bool goLeft, goRight, goUp, goDown;
        int playerspeed = 12;
        int jumpspeed = 8;

        public Form1()
        {
            InitializeComponent();
            generatePlatform(0, 400, 1000); //podloga

            generatePlatform(500, 300, 300);
            generatePlatform(50, 75, 120);
            generatePlatform(600, 200, 80);
            generatePlatform(270, 200, 180);



            //generatePlatformRandom(5);

        }



        public void generatePlatformRandom(int numberOf)
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
        public void generatePlatform(int posX, int posY, int width)
        {

            PictureBox grass = new PictureBox();
            Bitmap grassImage = Properties.Resources.grass;

            grass.Image = (Image)grassImage;
            grass.SizeMode = PictureBoxSizeMode.StretchImage;

            grass.Location = new Point(posX, posY);
            grass.Height = 10;
            grass.Width = width;
            grass.Tag = "grass";
            Controls.Add(grass);


            PictureBox ground = new PictureBox();
            Bitmap groundImage = Properties.Resources.ground;

            ground.Image = (Image)groundImage;
            ground.SizeMode = PictureBoxSizeMode.StretchImage;

            ground.Location = new Point(posX, posY + 10);
            ground.Height = 1000;
            ground.Width = width;
            ground.Tag = "ground";
            Controls.Add(ground);


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }




        private void KeyU(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            else if (e.KeyCode == Keys.Up)
            {
                goUp = false;
                goDown = true;
            }

        }

        private void KeyDow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                goDown = false;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (goLeft == true && player.Left >= 0 + 2)
            {
                player.Left -= playerspeed;
            }
            if (goRight == true && player.Left <= 816 - 80)
            {
                player.Left += playerspeed;
            }
            if (goUp == true && player.Top >= 0)
            {
                player.Top -= jumpspeed;
            }
            if (goDown == true && player.Top <= 489 - player.Height - 88)
            {
                player.Top += jumpspeed;
            }
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.DrawImage(Properties.Resources._31, 0, 380);
            int width = Properties.Resources._31.Width;
            int height = Properties.Resources._31.Height;
            RectangleF destinationRect = new RectangleF(0, 380, 4f * width,1.3f * height);
            RectangleF sourceRect = new RectangleF(0, 0, 1f * width, .75f * height);
            g.DrawImage(Properties.Resources._31, destinationRect, sourceRect, GraphicsUnit.Pixel);
            g.DrawImage(Properties.Resources._31, 500, 300);
            g.DrawImage(Properties.Resources._31, 50, 75);
            g.DrawImage(Properties.Resources._31, 600, 200);
            g.DrawImage(Properties.Resources._31, 270, 200);

            g.DrawImage(Properties.Resources.bpxurrk5l37211, player.Left, player.Top);
            pictureBox1.Refresh();
        }

    }
}

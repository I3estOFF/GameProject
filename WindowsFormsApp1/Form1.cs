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
<<<<<<< HEAD
        Graphics g;
        List<Rectangle> PlatformHB = new List<Rectangle>(); // hitboxy platform
        bool playerRight = false;
        bool playerLeft = false;
        bool playerUp = false;
        bool isGrounded = false;
        int fallSpeed = 0;
        int jumpSpeed = 200;
        int playerSpeed = 5;
        //int scroll = 0;

        public Form1()
        {
            InitializeComponent();

        }




=======
        bool goLeft, goRight, goUp, goDown;
        int playerspeed = 12;
        int jumpspeed = 8;
        bool jump;
        int force = 8;
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
>>>>>>> 82393a4af41d5d342f0d0f9370b94b84bfffdea1

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
<<<<<<< HEAD
            if (e.KeyCode == Keys.Up)
            {
                playerUp = false;
                jumpSpeed = 200;
                isGrounded = false;
=======
            else if (jump == true)
            {
                jump = false;
>>>>>>> 82393a4af41d5d342f0d0f9370b94b84bfffdea1
            }

        }

        private void KeyDow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                playerLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                playerRight = true;
            }
<<<<<<< HEAD
            if (e.KeyCode == Keys.Up && isGrounded)
            {
                playerUp = true;
                fallSpeed = 0;
=======
            else if (e.KeyCode == Keys.Space && jump == false)
            {
                jump = true;
>>>>>>> 82393a4af41d5d342f0d0f9370b94b84bfffdea1
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
<<<<<<< HEAD

            if (playerLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (playerRight == true && player.Left < this.Width)
=======
            player.Top += jumpspeed;
            if (goLeft == true && player.Left >= 0 + 2)
>>>>>>> 82393a4af41d5d342f0d0f9370b94b84bfffdea1
            {
                player.Left += playerSpeed;

            }
            if (playerUp == true && isGrounded)
            {
                player.Top -= jumpSpeed / 10;
                if (jumpSpeed > 0)
                    jumpSpeed -= 7;
                else if (jumpSpeed <= 0)
                {
                    playerUp = false;
                    fallSpeed += 50;
                }
            }
<<<<<<< HEAD
            if (player.Location.Y < this.Height && playerUp == false)
            {
                player.Top += fallSpeed / 10;
                if (fallSpeed < 100)
                    fallSpeed += 7;
            }
            foreach (Rectangle hb in PlatformHB)
            {
                if (player.Bounds.IntersectsWith(hb))
                {
                    fallSpeed = 0;
                    player.Top = hb.Y - player.Height;
                    isGrounded = true;
                }

            }
            pictureBox1.Image = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(pictureBox1.Image);
=======
            if (jump == true && force < 0)
            {
                jump = false;
            }
            if (jump == true)                                           //&& w tym miejscu dopisać warunek kolizji z platformą
            {
                jumpspeed = -8;
                force -= 1;
            }
            else
            {
                jumpspeed = 10;
            }
            foreach(Control x in this.Controls)
            {
                if(player.Top <= 489 - player.Height - 88)
                {
                    force = 8;
                    player.Top = 315;
                }
            }
            pictureBox1.Size = new Size(this.Width, this.Height);
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
>>>>>>> 82393a4af41d5d342f0d0f9370b94b84bfffdea1

            g.DrawImage(Properties.Resources.bpxurrk5l37211, player.Left, player.Top);


            generatePlatform(0, Height - 100, Width);

            generatePlatform(Width/10, Height/3, Width/5);
            generatePlatform(2*Width/5, 2*Height/3, Width/5);
            generatePlatform(3*Width/5, 2*Height/5, Width/20);
            generatePlatform(Width/3, Height/10, Width/20);
            



            pictureBox1.Refresh();

        }

        int platWidth = Properties.Resources._31.Width;
        int platHeight = Properties.Resources._31.Height;
        private void generatePlatform(int posX,int posY, int _width)
        {
           
            g.DrawImage(Properties.Resources._31, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width+platWidth, 1);
            PlatformHB.Add(rect);
            if(platWidth < _width)
            {
                generatePlatform(posX+platWidth, posY, _width - platWidth);
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

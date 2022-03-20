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
            else if (jump == true)
            {
                jump = false;
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
            else if (e.KeyCode == Keys.Space && jump == false)
            {
                jump = true;
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            player.Top += jumpspeed;
            if (goLeft == true && player.Left >= 0 + 2)
            {
                player.Left -= playerspeed;
            }
            if (goRight == true && player.Left <= 816 - 80)
            {
                player.Left += playerspeed;
            }
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

            g.DrawImage(Properties.Resources.bpxurrk5l37211, player.Left, player.Top);
            pictureBox1.Refresh();

        }

    }
}

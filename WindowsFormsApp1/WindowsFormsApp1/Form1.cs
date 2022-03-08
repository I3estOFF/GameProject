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
        public Form1()
        {
            InitializeComponent();

            generatePlatform(0,400,1000); //podloga
            
            generatePlatform(500,300,300);
            generatePlatform(50,75,120);
            generatePlatform(600,200,80);
            generatePlatform(270,200,180);



            //generatePlatformRandom(5);

        }



        public void generatePlatformRandom(int numberOf)
        {
            Random rand = new Random();
            for (int i = 0; i < numberOf; i++)
            {
                int ran1 = rand.Next(10,700);
                int ran2 = rand.Next(0,350);
                int ran3 = rand.Next(100,200);
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
            
            ground.Location = new Point(posX, posY+10);
            ground.Height = 1000; 
            ground.Width = width;
            ground.Tag = "ground";
            Controls.Add(ground);


        }
    }
}

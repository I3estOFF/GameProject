using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{


    public partial class Form1 : Form
    {

        int ResolutionWidth = 1; // zmienne trzymajace rzeczywista rozdzielczosc ekranu
        int ResolutionHeight = 1;

        static Graphics gBackground;
        Graphics gPlayer;
        Bitmap PlayerBitmap;
        List<Rectangle> CarrotHB = new List<Rectangle>();
        List<Rectangle> EmptyHB = new List<Rectangle>();
        Player p;
        World w;
        int screenShift = 0;

        public bool mar1 = true;
        int punkty = 0;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            label1.Text = "Punkty: " + punkty;
        }

        //dzieją się rzeczy
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

        }
        public float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
        // i dostajemy wspolczynnik skalowania obrazu
        private void onFormLoad(object sender, EventArgs e)
        {
            //inicjalizacja obiektow
            pictureBoxBackground.Image = new Bitmap(this.Width, this.Height);
            gBackground = Graphics.FromImage(pictureBoxBackground.Image);
            pictureBoxPlayer.Image = new Bitmap(this.Width, this.Height);
            gPlayer = Graphics.FromImage(pictureBoxPlayer.Image);


            System.Diagnostics.Debug.WriteLine(Width);
            //przeskalowanie grafik do 100% rozmiaru
            gBackground.ScaleTransform(1f / getScalingFactor(), 1 / getScalingFactor());
            gPlayer.ScaleTransform(1f / getScalingFactor(), 1 / getScalingFactor());

            //wyliczenie i zapisanie aktualnej rozdzielczosci ekranu
            ResolutionWidth = Convert.ToInt32(Width * getScalingFactor());
            ResolutionHeight = Convert.ToInt32(Height * getScalingFactor());


            System.Diagnostics.Debug.WriteLine(ResolutionWidth);

            //umiejscowienie pictureBoxa z graczem 
            pictureBoxPlayer.Dock = DockStyle.Fill;
            pictureBoxPlayer.Parent = pictureBoxBackground;
            pictureBoxPlayer.BackColor = Color.Transparent;


            w = new World(gBackground, ResolutionWidth, ResolutionHeight);
            p = new Player(gPlayer, PlayerBitmap, ResolutionWidth, ResolutionHeight, w.PlatformHB);
            w.SetPlayer(p);

         

            w.generateGround(0, ResolutionHeight - 100, 50);
            w.generatePlatformRandom(4);
            
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
                p.faceLeft = false;
            }
            if (e.KeyCode == Keys.Up && p.isGrounded)
            {
                p.playerUp = true;
                p.fallSpeed = 0;
            }

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
                    generateEmpty(2 * ResolutionWidth / 4, 3 * ResolutionHeight / 5, ResolutionWidth / 80);
                    mar1 = false;
                    break;
                }
                else
                    break;

            }
            if (mar1 == true)
                generateCarrot(2 * ResolutionWidth / 4, 3 * ResolutionHeight / 5, ResolutionWidth / 80);



            
            pictureBoxPlayer.Refresh();

            gPlayer.Clear(Color.Transparent);

        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            
            if(screenShift% 50==1)
            {
                w.generatePlatformRandom(1);
            }
            else if(screenShift% 250==3)
            {
                w.generateCloud();
            }
            screenShift++;
            gBackground.Clear(Color.Transparent);
            w.RenderClouds();
            w.RenderPlatforms();
        }



        int CarrotWidth = Properties.Resources.Carrot.Width;
        int CarrotHeight = Properties.Resources.Carrot.Height;

        int EmptyWidth = Properties.Resources.Carrot.Width;
        int EmptyHeight = Properties.Resources.Carrot.Height;

        private void generateCarrot(int posX, int posY, int _width)
        {

            gBackground.DrawImage(Properties.Resources.Carrot, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + CarrotWidth, 1);
            CarrotHB.Add(rect);

        }

        private void generateEmpty(int posX, int posY, int _width)
        {

            gBackground.DrawImage(Properties.Resources.Empty, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, _width + EmptyWidth, 1);
            EmptyHB.Add(rect);

        }
        


    }
}

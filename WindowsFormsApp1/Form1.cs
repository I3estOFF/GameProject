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
        private void onFormLoad(object sender, EventArgs e)
        {
            //inicjalizacja obiektow
            pictureBoxBackground.Image = new Bitmap(this.Width, this.Height);
            gBackground = Graphics.FromImage(pictureBoxBackground.Image);
            pictureBoxPlayer.Image = new Bitmap(this.Width, this.Height);
            gPlayer = Graphics.FromImage(pictureBoxPlayer.Image);


            System.Diagnostics.Debug.WriteLine(Width);
            //przeskalowanie grafik do 100% rozmiaru
            gBackground.ScaleTransform(1f/getScalingFactor(), 1/getScalingFactor());
            gPlayer.ScaleTransform(1f / getScalingFactor(), 1 / getScalingFactor());
            
            //wyliczenie i zapisanie aktualnej rozdzielczosci ekranu
            ResolutionWidth = Convert.ToInt32(Width * getScalingFactor());
            ResolutionHeight = Convert.ToInt32(Height * getScalingFactor());
            
      
            System.Diagnostics.Debug.WriteLine(ResolutionWidth);

            //umiejscowienie pictureBoxa z graczem 
            pictureBoxPlayer.Dock = DockStyle.Fill;
            pictureBoxPlayer.Parent = pictureBoxBackground;
            pictureBoxPlayer.BackColor = Color.Transparent;

            p = new Player(gPlayer, PlayerBitmap, ResolutionWidth, ResolutionHeight, PlatformHB);

            generatePlatform(0, ResolutionHeight - 100, 1000);

            generatePlatformRandom(5);
            //generatePlatform(Width / 10, Height / 3, 10);
            //generatePlatform(2 * Width / 5, 2 * Height / 3, 5);
            //generatePlatform(3 * Width / 5, 2 * Height / 5, 10);
            //generatePlatform(Width / 3, Height / 9, 7);

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



            pictureBoxBackground.Refresh();
            //gBackground.Clear(Color.Transparent);
            pictureBoxPlayer.Refresh();
            gPlayer.Clear(Color.Transparent);

        }



        int CarrotWidth = Properties.Resources.Carrot.Width;
        int CarrotHeight = Properties.Resources.Carrot.Height;

        int EmptyWidth = Properties.Resources.Carrot.Width;
        int EmptyHeight = Properties.Resources.Carrot.Height;


        int platWidth = Properties.Resources._311.Width;
        int platHeight = Properties.Resources._311.Height;
        private void generatePlatform(int posX, int posY, int _width)
        {

            gBackground.DrawImage(Properties.Resources._311, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, platWidth * _width, 1);
            PlatformHB.Add(rect);
            int len = 0;
            for (int i = 0; i < _width; i++)
            {
                gBackground.DrawImage(Properties.Resources._311, posX + len, posY);
                len += platWidth;
            }

        }

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

        private void generatePlatformRandom(int numberOf)
        {
            Random rand = new Random();

            int ranX = rand.Next(50, ResolutionWidth - 200);
            int ranY = ResolutionHeight - 100;
            int ranWidth = 10;
            int prevRanX = 0;
            int prevRanY = 0;
            int prevRanWidth = 0;



            int jumpDistance = 70;

            int ranXRight;
            int minY = p.playerBox.Height + 40; // ponad glowa gracza
            int maxY = 200; //wysokosc skoku


            for (int i = 0; i < numberOf; i++)
            {
                //zapamietuje poprzednie wartosci na ktorych bazie tworzymy kolejne
                prevRanWidth = ranWidth;
                prevRanX = ranX;
                prevRanY = ranY;

                ranY = prevRanY - rand.Next(minY, maxY);
                ///////////////////////////

                //losuje poczatek nowej platformy
                ranX = rand.Next(10, prevRanX + prevRanWidth * platWidth + jumpDistance);

                //uzupelniamy ewentualna luke szerokoscia platformy          
                if (ranX < prevRanX) // jesli zaczyna sie przed poprzednia platforma
                {
                    System.Diagnostics.Debug.WriteLine("condition 1");
                    if (prevRanX - jumpDistance > ranX)
                        ranXRight = rand.Next(prevRanX - jumpDistance, prevRanX + (prevRanWidth - 3) * platWidth - jumpDistance);
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("ranX: " + ranX);

                        System.Diagnostics.Debug.WriteLine("prevRanX - jumpDistance + prevRanWidth = " + (prevRanX - jumpDistance + prevRanWidth * platWidth));
                        //probably tutaj jest blad gdy nastepna platforma zaczyna sie mniej niz jumpdistance przed a konczy za platforma
                        ranXRight = rand.Next(ranX, prevRanX - jumpDistance + (prevRanWidth - 3) * platWidth);

                    }

                    ranWidth = (ranXRight - ranX) / platWidth + 5;
                }
                else if (ranX >= prevRanX) // jesli zaczyna sie na poprzedniej platformie lub za nia
                {

                    System.Diagnostics.Debug.WriteLine("condition else if");
                    //ranX = ranX + 100;
                    if (ranX < prevRanX + prevRanWidth * platWidth + jumpDistance)
                    {
                        System.Diagnostics.Debug.WriteLine("ranX: " + ranX);

                        System.Diagnostics.Debug.WriteLine("prevRanX + prevRanWidth*platWidth - jumpDistance = " + (prevRanX - jumpDistance + prevRanWidth * platWidth));

                        ranXRight = rand.Next(ranX, prevRanX + prevRanWidth * platWidth + jumpDistance);

                        ranWidth = (ranXRight - ranX) / platWidth + 5;
                    }
                    else
                    {
                        ranX = prevRanX + prevRanWidth * platWidth + jumpDistance;
                        ranWidth = rand.Next(7, 10);

                        System.Diagnostics.Debug.WriteLine("condition else");
                    }
                }

                for (int n = 0; n < 10; n++)
                {
                    if (ranX + ranWidth * platWidth >= ResolutionWidth)
                    {

                        System.Diagnostics.Debug.WriteLine("too far right");
                        ranX -= 2 * (ranX + ranWidth * platWidth - ResolutionWidth);
                    }

                }



                /////////////////////////////////
                //System.Diagnostics.Debug.WriteLine(prevRanX);
                System.Diagnostics.Debug.WriteLine("Generated Platform: " + ranX + " " + ranY + " " + ranWidth);
                //System.Diagnostics.Debug.WriteLine(ranY);
                //System.Diagnostics.Debug.WriteLine(ranWidth);

                generatePlatform(ranX, ranY, ranWidth);
            }
        }

    }
}

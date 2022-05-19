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
        int screenShift = 0;
        static int punkty = 0;
        int czas = 0;


        static Graphics gBackground;
        Graphics gPlayer;
        Bitmap PlayerBitmap;
        Player p;
        Overlay overlay;
        World w;
        ObjectCollection objectCollection;

        public Form1()                                                                                                      //inicjalizacja okna
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            label1.Text = "Punkty: 0";
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        }
        public float getScalingFactor()                                                                                 //uzyskiwanie współczynnika skalowania
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
        private void onFormLoad(object sender, EventArgs e)                                                            //inicjalizowanie gry
        {
            //inicjalizacja obiektow
            pictureBoxBackground.Image = new Bitmap(this.Width, this.Height);
            gBackground = Graphics.FromImage(pictureBoxBackground.Image);
            pictureBoxPlayer.Image = new Bitmap(this.Width, this.Height);
            gPlayer = Graphics.FromImage(pictureBoxPlayer.Image);

            System.Diagnostics.Debug.WriteLine(Width);

            //przeskalowanie grafik do 100% rozmiaru
            float scalingFactor = getScalingFactor();
            gBackground.ScaleTransform(1f / scalingFactor, 1 / scalingFactor);
            gPlayer.ScaleTransform(1f / scalingFactor, 1 / scalingFactor);

            //wyliczenie i zapisanie aktualnej rozdzielczosci ekranu
            ResolutionWidth = Convert.ToInt32(Width * getScalingFactor());
            ResolutionHeight = Convert.ToInt32(Height * getScalingFactor());

            System.Diagnostics.Debug.WriteLine(ResolutionWidth);
            ///////////////////////////////
            
            //umiejscowienie pictureBoxa z graczem 
            pictureBoxPlayer.Dock = DockStyle.Fill;
            pictureBoxPlayer.Parent = pictureBoxBackground;
            pictureBoxPlayer.BackColor = Color.Transparent;
            label1.Parent = pictureBoxBackground;
            pictureBox2.Parent = pictureBoxBackground;
            pictureBoxPlayer.BackColor = Color.Transparent;
            label1.BringToFront();
            pictureBox2.BringToFront();

            //setup pictureboxa z GameOver
            OverlayLayer.Parent = pictureBoxPlayer;
            OverlayLayer.BackColor = Color.Transparent;
            OverlayLayer.BringToFront();
            OverlayLayer.Visible = false;

            label1.Visible = false;
            pictureBox2.Visible = false;

            objectCollection = new ObjectCollection(timer1, timer2, timer3, timer4, timer5, menuTimer, gBackground, gPlayer, label1, pictureBox2, ResolutionWidth, ResolutionHeight);
            
            w = new World(gBackground, ResolutionWidth, ResolutionHeight);
            p = new Player(gPlayer, PlayerBitmap, ResolutionWidth, ResolutionHeight,objectCollection);
            w.SetPlayer(p);
            p.setWorld(w);
            overlay = new Overlay(OverlayLayer, objectCollection, p);
            overlay.scaleGraphics(getScalingFactor());
            overlay.gimmeForm(this);
            overlay.MainMenu();
            p.setOverlay(overlay);




            w.generateGround(0, ResolutionHeight - 100, 50);
            w.generatePlatformRandom(4);
            //overlay.StartGame();
        }
        private void KeyU(object sender, KeyEventArgs e)                                                                        //sterowanie
        {
            if (!p.canMove)
                return;
            if (e.KeyCode == Keys.A)
            {
                p.playerLeft = false;
            }
            if (e.KeyCode == Keys.D)
            {
                p.playerRight = false;
            }
            if (e.KeyCode == Keys.W)
            {
                p.playerUp = false;
                p.jumpSpeed = 200;
                p.isGrounded = false;
            }
        }

        private void KeyDow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                overlay.MainMenu();
            if (!p.canMove)
                return;
            if (e.KeyCode == Keys.A)
            {
                p.playerLeft = true;
                p.faceLeft = true;
            }
            if (e.KeyCode == Keys.D)
            {
                p.playerRight = true;
                p.faceLeft = false;
            }
            if (e.KeyCode == Keys.W && p.isGrounded)
            {
                p.playerUp = true;
                p.fallSpeed = 0;
            }
            if (e.KeyCode == Keys.Space && timer1.Enabled == true)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = false;
            }
            else
            {
                timer1.Enabled = true;
                timer2.Enabled = true;
                timer3.Enabled = true;
            }


        }

        private void timer1_Tick(object sender, EventArgs e)                                                                    //Timery
        {
            if (!p.canMove)
                return;
            p.PlayerMovement();
            p.PlatformPlayerCollision();
            p.MeteorPlayerCollision();
            p.CarrotPlayerCollision();
            p.GoldenCarrotPlayerCollision();
            p.KubotyPlayerCollision();
            p.HelmetPlayerCollision();
            pictureBoxPlayer.Refresh();
            gPlayer.Clear(Color.Transparent);
            label1.Text = "   Punkty: " + punkty + "   " + p.pkt + "x    " + p.gpkt + "x      ";

            if (p.hearts == 3)
                pictureBox2.Image = Properties.Resources.hearts3;
            else if (p.hearts == 4)
                pictureBox2.Image = Properties.Resources.hearts4;
            else if (p.hearts == 2)
                pictureBox2.Image = Properties.Resources.hearts2;
            else if (p.hearts == 1)
                pictureBox2.Image = Properties.Resources.hearts1;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!p.canMove)
                return;
            if (screenShift% 20==1)
            {
                w.generatePlatformRandom(1);
                
            }
            else if(screenShift% 400==0)
            {
                w.generateCloud();                                                           
            }
            else if(screenShift% 111==1)
            {
                w.generateCarrotRandom();
            }

            screenShift++;

            gBackground.Clear(Color.Transparent);
            
            w.RenderClouds();                                                               
            w.RenderPlatforms();
            w.RenderCarrots();
            w.RenderGoldenCarrots();
            w.RenderKubots();
            w.RenderHelmet();
            w.RenderMeteorite();

            if (w.boomed)
                w.renderExplosion();
            gBackground.DrawRectangle(new Pen(Brushes.Blue),p.playerBox);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!p.canMove)
                return;
            if(w.screenScrollSpeed >=2)
                punkty = punkty + 1;
            
            czas = czas + 1;
            if (czas == 30)
            {
                w.screenScrollSpeed2 = 3;
                w.screenScrollSpeed = 2;
            
            }
            if (punkty == 400) w.screenScrollSpeed++;
            if (punkty == 800) w.screenScrollSpeed++;
            if (punkty == 1200) w.screenScrollSpeed++;
            Random rand = new Random();
            int chance = rand.Next(1, 101);
            if (chance <= 6 && punkty > 50 && punkty < 200)
            {
                w.generateMeteorite();
            }

            else if (chance <= 6 && punkty > 1250 && punkty < 1500)
            {
                w.generateMeteorite();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)                                                   //losowanie szansy na wystąpienie power-up'ów
        {
            Random rand = new Random();
            int chance = rand.Next(1, 101);
            
            if (chance <= 30 && w.screenScrollSpeed >= 3)             //30% szans co 3 sekundy
            {
                w.generateKubotsRandom();
            }
            if (chance <= 80 && w.screenScrollSpeed >= 3)             //do testów 80% szans na pojawienie się hełmu co 3 sekundy
            {
                w.generateHelmetRandom();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int chance = rand.Next(1, 101);
            if (chance <= 10 && w.screenScrollSpeed >= 3)             //10% szans co 2,5 sekundy
            {
                w.generateGoldenCarrotRandom();
            }

        }

        private void pictureBoxPlayer_MouseClick(object sender, MouseEventArgs e)
        {
            
            w.explosion.X = (int)((float)Cursor.Position.X * getScalingFactor());
            w.explosion.Y = (int)((float)Cursor.Position.Y * getScalingFactor());
            w.popMeteorite();
            
        }

        private void OverlayLayer_MouseClick(object sender, MouseEventArgs e)
        {
            overlay.mousePosition.X = (int)((float)Cursor.Position.X * getScalingFactor());
            overlay.mousePosition.Y = (int)((float)Cursor.Position.Y * getScalingFactor());
        }

        private void menuTimer_Tick(object sender, EventArgs e)
        {
            overlay.checkMenuInput(w);
          
        }
    }
}

using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Overlay
    {
        private PictureBox OverlayLayer;
        private ObjectCollection objectCollection;
        private Player player;
        public Point mousePosition;
        Graphics g;
        Form form;
        public bool gameStarted = false;
        public bool gameExited = false;
        public bool gamePaused = false;

        public Overlay(PictureBox OverlayLayer, ObjectCollection objectCollection, Player player)
        {
            OverlayLayer.BackColor = Color.Transparent;

            this.OverlayLayer = OverlayLayer;
            this.objectCollection = objectCollection;
            this.player = player;

            OverlayLayer.Visible = false;
            OverlayLayer.Image = new Bitmap(objectCollection.resolutionWidth, objectCollection.resolutionHeight);
            g = Graphics.FromImage(OverlayLayer.Image);
            OverlayLayer.Dock = DockStyle.Fill;
        }

        public void MainMenu()                                                                                  //MainMenu
        {
            gamePaused = true;
            OverlayLayer.Visible = true;
            objectCollection.timer1.Stop();
            System.Diagnostics.Debug.WriteLine("timer 1 stopped");
            objectCollection.timer2.Stop();

            System.Diagnostics.Debug.WriteLine("timer 2 stopped");
            objectCollection.timer3.Stop();

            System.Diagnostics.Debug.WriteLine("timer 3 stopped");
            RenderMenu();
            Rectangle start = new Rectangle(860, 430, 190, 60);
            Rectangle exit = new Rectangle(860, 550, 190, 70);

            g.DrawRectangle(new Pen(Brushes.Black), start);
            g.DrawRectangle(new Pen(Brushes.Black), exit);
            OverlayLayer.BringToFront();
            var t = Task.Run(() => waitForInput(start, exit));
            t.Wait();
        }
        private async void waitForInput(Rectangle start, Rectangle exit)
        {
            while (true)                                                                                            //Start/Exit
            {
                if (start.Contains(mousePosition) && gamePaused)
                {
                    gameStarted = true;
                    break;
                }
                if (exit.Contains(mousePosition))
                {
                    gameExited = true;
                    break;

                }
                await Task.Delay(10);
                
            }
        }
        public void checkMenuInput(World w)
        {
            if (gameStarted)
            {
                gamePaused = false;
                StartGame();

            }
            if (gameExited)
            {
                closeGame();
            }
        }
        private void RenderMenu()                                                                                       //Renderowanie Manu
        {
            OverlayLayer.Visible = true;
            OverlayLayer.BringToFront();
            g.DrawImage(Properties.Resources.main_menu, objectCollection.resolutionWidth / 3, objectCollection.resolutionHeight / 3);
           
        }

        public void scaleGraphics(float scalingFactor)
        {
            g.ScaleTransform(1f / scalingFactor, 1 / scalingFactor);
        }

        public void StartGame()                                                                                     //Uruchamianie gry
        {
            
            if(gameStarted && gamePaused == false)
            {
                

                System.Diagnostics.Debug.WriteLine("game started");
                objectCollection.label.Visible = true;
                objectCollection.picturebox2.Visible = true;
                objectCollection.timer1.Start();
                objectCollection.timer2.Start();
                objectCollection.timer3.Start();
                OverlayLayer.Visible = false;
                gameStarted = false;
                mousePosition = new Point(0, 0);    
            }
        }

        public void GameOver()                                                                                //GameOver
        {
            gamePaused = true;
            player.playerLeft = false;
            player.playerRight = false;
            player.playerLeft = false;
            player.playerUp = false;

            g.Clear(Color.Transparent);
            g.DrawImage(Properties.Resources.game_over, 500, 500);
            OverlayLayer.Visible = true;
            objectCollection.timer1.Enabled = false;
            objectCollection.timer2.Enabled = false;
            objectCollection.timer3.Enabled = false;

             MainMenu();
        }

        public void closeGame()                                                                                 //Wychodzenie z gry
        {
            form.Close();
        }

        public void gimmeForm(Form f)
        {
            form = f;
        }
    }
}

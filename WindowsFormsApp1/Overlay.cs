using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

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
        public bool gameContinue = false;

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
            objectCollection.timer2.Stop();
            objectCollection.timer3.Stop();
            RenderMenu();
            Rectangle start = new Rectangle(860, 430, 190, 60);
            Rectangle exit = new Rectangle(860, 550, 190, 70);

            g.DrawRectangle(new Pen(Brushes.Black), start);
            g.DrawRectangle(new Pen(Brushes.Black), exit);
            OverlayLayer.BringToFront();
            var t = Task.Run(() => waitForInput(start, exit));
            t.Wait();
        }

        public void PauseMenu()
        {
            gamePaused = true;
            OverlayLayer.Visible = true;
            objectCollection.timer1.Stop();
            objectCollection.timer2.Stop();
            objectCollection.timer3.Stop();
            RenderPauseMenu();
            Rectangle cont = new Rectangle(840, 440, 300, 70);
            Rectangle start = new Rectangle(900, 520, 190, 60);
            Rectangle exit = new Rectangle(900, 600, 190, 70);

            g.DrawRectangle(new Pen(Brushes.Black), start);
            g.DrawRectangle(new Pen(Brushes.Black), exit);
            g.DrawRectangle(new Pen(Brushes.Black), cont);
            OverlayLayer.BringToFront();
            var t = Task.Run(() => waitForInput(cont,start, exit));
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
        private async void waitForInput(Rectangle cont, Rectangle start, Rectangle exit)
        {
            while (true)                                                                                            //Start/Exit
            {
                if (cont.Contains(mousePosition))
                {
                    gameContinue = true;
                    break;
                }
                if (start.Contains(mousePosition))
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
            if (gameContinue)
            {
                gamePaused = false;
                ContinueGame();
            }

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

        private void RenderPauseMenu()
        {
            OverlayLayer.Visible = true;
            OverlayLayer.BringToFront();
            g.DrawImage(Properties.Resources.main_menu_continue, objectCollection.resolutionWidth / 3, objectCollection.resolutionHeight / 3);

        }

        public void scaleGraphics(float scalingFactor)
        {
            g.ScaleTransform(1f / scalingFactor, 1 / scalingFactor);
        }

        public void StartGame()                                                                                     //Uruchamianie gry
        {

            if (gameStarted && gamePaused == false)
            {
                restartGame();

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

        public void ContinueGame()
        {
                objectCollection.label.Visible = true;
                objectCollection.picturebox2.Visible = true;
                objectCollection.timer1.Start();
                objectCollection.timer2.Start();
                objectCollection.timer3.Start();
                OverlayLayer.Visible = false;
                gameStarted = false;
                gameContinue = false;
                mousePosition = new Point(0, 0);
                System.Diagnostics.Debug.WriteLine("game Continued");
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

        public void restartGame()
        {
            
            objectCollection.world.Reset();
            objectCollection.player.Reset();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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
        private bool gameStarted = false;
        private bool gameExited = false;

        public Overlay(PictureBox OverlayLayer, ObjectCollection objectCollection, Player player)
        {
            OverlayLayer.BackColor = Color.Transparent;
            
            this.OverlayLayer = OverlayLayer;
            this.objectCollection = objectCollection;
            this.player = player;

            OverlayLayer.Visible = false;
            OverlayLayer.Image = new Bitmap(objectCollection.resolutionWidth,objectCollection.resolutionHeight);
            g = Graphics.FromImage(OverlayLayer.Image);
            OverlayLayer.Dock = DockStyle.Fill;
        }

        public void MainMenu()
        {
            player.canMove = false;
            OverlayLayer.Visible = true;
            objectCollection.timer1.Enabled = false;
            objectCollection.timer2.Enabled = false;
            objectCollection.timer3.Enabled = false;
            RenderMenu();
            Rectangle start = new Rectangle(860,430,190,60);
            Rectangle exit = new Rectangle(860,550,190,70);

            g.DrawRectangle(new Pen(Brushes.Black), start);
            g.DrawRectangle(new Pen(Brushes.Black), exit);
            OverlayLayer.BringToFront();
            var t = Task.Run(() => waitForInput(start,exit));
            t.Wait();



        }
        private async void waitForInput(Rectangle start, Rectangle exit)
        {
            while (true)
            {
                if (start.Contains(mousePosition))
                {
                    gameStarted = true;
                    gameExited = false;
                    break;
                }
                if (exit.Contains(mousePosition))
                {
                    gameExited = true;
                    gameStarted = false;
                    break;
                }
                await Task.Delay(10);
            }
        }
        public void checkMenuInput(World w)
        {
            if (gameStarted)
            {
                w.gamePaused = false;
                StartGame();

            }
            if (gameExited)
            {
                closeGame();
            }
        }
        private void RenderMenu()
        {
            OverlayLayer.Visible = true;
            OverlayLayer.BringToFront();

            g.DrawImage(Properties.Resources.main_menu,objectCollection.resolutionWidth/3,objectCollection.resolutionHeight/3);
        
        
        }

        public void scaleGraphics(float scalingFactor)
        {
            g.ScaleTransform(1f / scalingFactor, 1 / scalingFactor);
        }

        public void StartGame()
        {
            player.canMove = true;
            objectCollection.label.Visible = true;
            objectCollection.timer1.Enabled = true;
            objectCollection.timer2.Enabled = true;
            objectCollection.timer3.Enabled = true;
            OverlayLayer.Visible = false;

        }


        public async void GameOver()
        {
            player.playerLeft = false;
            player.playerRight = false;
            player.playerLeft = false;
            player.playerUp = false;
            player.jumpSpeed = 0;
            player.fallSpeed = 0;
            player.playerSpeed = 0;

            player.canMove = false;
            g.Clear(Color.Transparent);
            g.DrawImage(Properties.Resources.game_over, 500,500);
            OverlayLayer.Visible = true;
            objectCollection.timer1.Enabled = false;
            objectCollection.timer2.Enabled = false;
            objectCollection.timer3.Enabled = false;
            await Task.Delay(5000);
            MainMenu();
        }

        public void closeGame()
        {
            form.Close();
        }

        public void gimmeForm(Form f)
        {
            form = f;
        }



    }
}

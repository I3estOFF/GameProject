using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Player
    {
        public bool canMove = false;

        public bool playerRight = false;
        public bool playerLeft = false;
        public bool playerUp = false;
        public bool isGrounded = false; // sprawdza czy gracz dotyka ziemi
        public bool faceLeft = false; // zapamietuje ostatni kierunek ruchu
        bool playerSideCollison = false;
        
        
        const int fallSpeedAcceleration = 9;
        const int jumpSpeedDeceleration = 18;
        const int maxFallSpeed = 100;
        int maxPlayerSpeed = 7;
        public int maxJumpSpeed = 300;
        public int fallSpeed = 0;
        public int jumpSpeed;
        public int playerSpeed;
        public int pkt = 0;
        public int gpkt = 0;
        public bool playereatgold = false;
        public bool playerhavekuboty = false;
        public bool playerhavehelmet = false;
        readonly int Width;
        readonly int Heigth;

        Graphics gPlayer;
        Bitmap player;
        World w;
        public Rectangle playerBox; //hitbox postaci
        Overlay overlay;
        ObjectCollection objectCollection;



        public Player(Graphics _g, Bitmap _player, int W, int H, ObjectCollection objectCollection)
        {
            playerBox = new Rectangle(80, H - 200, Properties.Resources.Chungus.Width, Properties.Resources.Chungus.Height);
            gPlayer = _g;
            player = _player;
            Width = W;
            Heigth = H;
            this.objectCollection = objectCollection;

        }
        public void setWorld(World w)
        {
            this.w = w;
        }
        public void setOverlay(Overlay overlay)
        {
            this.overlay = overlay;
        }
        
        public void PlayerMovement()                                                                                                //ruch gracza
        {
            if (!canMove)
                return;

            //buffy
            if (playereatgold == true && playerhavekuboty == true && playerhavehelmet == false)
            {
                player = Properties.Resources.goldenchunguskuboty;
            }
            else if (playereatgold == true && playerhavekuboty == false && playerhavehelmet == false)
            {
                player = Properties.Resources.goldenchungus;
            }
            else if (playerhavekuboty == true && playereatgold == false && playerhavehelmet == false)
            {
                player = Properties.Resources.chunguskuboty;
            }
            else if (playerhavehelmet == true && playerhavekuboty == false && playereatgold == false)
            {
                player = Properties.Resources.Chungushelmet;
            }
            else if (playerhavehelmet == true && playereatgold ==true && playerhavekuboty == false)
            {
                player = Properties.Resources.goldenchungushelmet;
            }
            else if (playerhavehelmet == true && playereatgold == true && playerhavekuboty == true)
            {
                player = Properties.Resources.goldenchunguskubotyhelmet;
            }
            else if (playerhavehelmet == true && playerhavekuboty == true && playereatgold == false)
            {
                player = Properties.Resources.chunguskubotyhelmet;
            }
            else
                player = Properties.Resources.Chungus;

            playerSpeed = maxPlayerSpeed;



            // podstawowy ruch
            if (faceLeft) // zmienia zwrot postaci
            {
                player.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            gPlayer.DrawImage(player, playerBox.X, playerBox.Y);

            if (playerLeft == true && !playerSideCollison)
            {
                playerBox.X -= playerSpeed;
            }

            if (playerRight == true && playerBox.X < Width && !playerSideCollison)
            {
                playerBox.X += playerSpeed;
            }

            if (playerUp == true && isGrounded)
            {
                playerBox.Y -= jumpSpeed/10 ;
                if (jumpSpeed > 0)
                    jumpSpeed -= jumpSpeedDeceleration;
                else if (jumpSpeed <= 0)
                {
                    playerUp = false;
                    fallSpeed += 30;
                }
            }

            if (playerBox.Y < Heigth && playerUp == false)
            {
                playerBox.Y += fallSpeed / 10;
                if (fallSpeed < maxFallSpeed)
                    fallSpeed += fallSpeedAcceleration;
                isGrounded = false;
            }
            if (playerBox.Top > Heigth)
                overlay.GameOver();

            if (playerBox.Top < 200)
            {
               //if(pk)

            }
        }

        public void PlatformPlayerCollision()                                                                                   //kolizja gracza z platformą
        {
            playerSideCollison = false;
            foreach (Rectangle hb in w.PlatformHB)
            {
                // kolizja gracza z gorna krawedzia zatrzymuje opadanie //dziala
                if (playerBox.Y + playerBox.Height < hb.Y + 20 && playerBox.Y + playerBox.Height > hb.Y - 4 &&
                    playerBox.X < hb.X + hb.Width +20 && playerBox.X > hb.X - 50)
                {
                    playerBox.Y = hb.Y - playerBox.Height;
                    fallSpeed = 0;
                    jumpSpeed = maxJumpSpeed;
                    isGrounded = true;
                }
                //kolizja z dolna krawedzia zatrzymuje skok // dziala
                if (playerBox.Y < hb.Y + hb.Height + 40 && playerBox.Y > hb.Y + hb.Height &&
                    playerBox.X < hb.X + hb.Width +5 && playerBox.X > hb.X -40)
                {
                    jumpSpeed = 0;
                }
                //kolizja z bocznymi krawedziami 
                if (playerBox.Contains(hb.X-5, hb.Y + 20) || playerBox.Contains(hb.X + hb.Width+10, hb.Y + 20))
                {
                    playerSideCollison = true;
                }
            }
        }


        public void CarrotPlayerCollision()                                                                                   //kolizja gracza z marchewką
        {
            Rectangle toDelete = new Rectangle();
            foreach (Rectangle tt in w.carrots)
            {
                if (playerBox.Contains(tt))
                {                
                    toDelete = tt;
                }
            }
            if(!toDelete.IsEmpty)
            {
                w.carrots.Remove(toDelete);
                pkt += 1;
            }                    
        }


        public async void GoldenCarrotPlayerCollision()                                                                       //kolizja gracza ze złotą marchewką
        {
            Rectangle toDeleteg = new Rectangle();
            foreach (Rectangle gt in w.gcarrots)
            {
                if (playerBox.Contains(gt))
                {
                    toDeleteg = gt;
                }
            }
            if (!toDeleteg.IsEmpty)
            {
                w.gcarrots.Remove(toDeleteg);
                gpkt += 1;
                maxJumpSpeed = 400;
                playereatgold = true;
                await Task.Delay(3000);                          //daje zwiększony skok na 3 sekundy
                maxJumpSpeed = 300;
                playereatgold = false;
            }
        }

        public async void KubotyPlayerCollision()                                                                       //kolizja gracza z kubotami
        {
            Rectangle toDeleteg = new Rectangle();
            foreach (Rectangle kb in w.kuboty)
            {
                if (playerBox.Contains(kb))
                {
                    toDeleteg = kb;
                }
            }
            if (!toDeleteg.IsEmpty)
            {
                w.kuboty.Remove(toDeleteg);
                maxPlayerSpeed = 11;
                playerhavekuboty = true;
                await Task.Delay(5000);                          //daje przyspieszenie na 5 sekund
                playerhavekuboty = false;
                maxPlayerSpeed = 7;
            }
        }

        public void HelmetPlayerCollision()                                                                       //kolizja gracza z kubotami
        {
            Rectangle toDeleteg = new Rectangle();
            foreach (Rectangle hm in w.helmets)
            {
                if (playerBox.Contains(hm))
                {
                    toDeleteg = hm;
                }
            }
            if (!toDeleteg.IsEmpty)
            {
                w.helmets.Remove(toDeleteg);
                playerhavehelmet = true;

            }
        }
        public void MeteorPlayerCollision()                                                                                   //kolizja gracza z marchewką
        {
            Rectangle toDelete = new Rectangle();
            foreach (Rectangle tt in w.meteorites)
            {
                Rectangle temp = new Rectangle(tt.X+12, tt.Y+8, 25, 25);
                objectCollection.Background.DrawRectangle(new Pen(Brushes.DarkGreen), temp);
                if (playerBox.IntersectsWith(temp))
                {
                    canMove = false;
                    overlay.GameOver();
                    toDelete = tt;
                }
            }
        }
    }
}

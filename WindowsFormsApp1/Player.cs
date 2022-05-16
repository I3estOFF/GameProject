using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    internal class Player
    {
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
        readonly int Width;
        readonly int Heigth;

        Graphics gPlayer;
        Bitmap player;
        public Rectangle playerBox; //hitbox postaci
        List<Rectangle> PlatformHB;
        List<Rectangle> carrots;
        List<Rectangle> gcarrots;
        List<Rectangle> kuboty;


        public Player(Graphics _g, Bitmap _player, int W, int H, List<Rectangle> hb, List<Rectangle> tt, List<Rectangle> gt, List<Rectangle> kb)
        {
            playerBox = new Rectangle(80, H - 200, Properties.Resources.Chungus.Width, Properties.Resources.Chungus.Height);
            gPlayer = _g;
            player = _player;
            Width = W;
            Heigth = H;
            PlatformHB = hb;
            carrots = tt;
            gcarrots = gt;
            kuboty = kb;
        }
        
        public void PlayerMovement()                                                                                                //ruch gracza
        {
            if (playereatgold == true && playerhavekuboty == true)
            {
                player = Properties.Resources.goldenchunguskuboty;
            }
            else if (playereatgold == true && playerhavekuboty == false)
            {
                player = Properties.Resources.goldenchungus;
            }
            else if (playerhavekuboty == true && playereatgold == false)
                player = Properties.Resources.chunguskuboty;
            else
                player = Properties.Resources.Chungus;

            playerSpeed = maxPlayerSpeed;


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
        }

        public void PlatformPlayerCollision()                                                                                   //kolizja gracza z platformą
        {
            playerSideCollison = false;
            foreach (Rectangle hb in PlatformHB)
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
            foreach (Rectangle tt in carrots)
            {
                if (playerBox.Contains(tt))
                {                
                    toDelete = tt;
                }
            }
            if(!toDelete.IsEmpty)
            {
                carrots.Remove(toDelete);
                pkt += 1;
            }                    
        }

        public async void GoldenCarrotPlayerCollision()                                                                       //kolizja gracza ze złotą marchewką
        {
            Rectangle toDeleteg = new Rectangle();
            foreach (Rectangle gt in gcarrots)
            {
                if (playerBox.Contains(gt))
                {
                    toDeleteg = gt;
                }
            }
            if (!toDeleteg.IsEmpty)
            {
                gcarrots.Remove(toDeleteg);
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
            foreach (Rectangle kb in kuboty)
            {
                if (playerBox.Contains(kb))
                {
                    toDeleteg = kb;
                }
            }
            if (!toDeleteg.IsEmpty)
            {
                kuboty.Remove(toDeleteg);
                maxPlayerSpeed = 11;
                playerhavekuboty = true;
                await Task.Delay(5000);                          //daje przyspieszenie na 5 sekund
                playerhavekuboty = false;
                maxPlayerSpeed = 7;
            }
        }
    }
}

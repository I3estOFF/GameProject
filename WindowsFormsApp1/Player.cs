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
        const int jumpSpeedDeceleration = 20;
        const int maxJumpSpeed = 300;
        const int maxFallSpeed = 150;
        const int maxPlayerSpeed = 7;
        public int fallSpeed = 0;
        public int jumpSpeed = maxJumpSpeed;
        public int playerSpeed = maxPlayerSpeed;

        Graphics gPlayer;
        Bitmap player;
        public Rectangle playerBox = new Rectangle(80, 200, Properties.Resources.Chungus.Width, Properties.Resources.Chungus.Height); //hitbox postaci
        List<Rectangle> PlatformHB;
        readonly int Width;
        readonly int Heigth;

        public Player(Graphics _g, Bitmap _player, int W, int H, List<Rectangle> hb)
        {
            gPlayer = _g;
            player = _player;
            Width = W;
            Heigth = H;
            PlatformHB = hb;
        }
        public void PlayerMovement()
        {
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
            if (playerRight == true && playerBox.X < this.Width && !playerSideCollison)
            {
                playerBox.X += playerSpeed;
            }
            if (playerUp == true && isGrounded)
            {
                playerBox.Y -= jumpSpeed / 10;
                if (jumpSpeed > 0)
                    jumpSpeed -= jumpSpeedDeceleration;
                else if (jumpSpeed <= 0)
                {
                    playerUp = false;
                    fallSpeed += 50;
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

        public void PlatformPlayerCollision()
        {
            playerSideCollison = false;
            foreach (Rectangle hb in PlatformHB)
            {
                // kolizja gracza z gorna krawedzia zatrzymuje opadanie //dziala
                if (playerBox.Y + playerBox.Height < hb.Y + 20 && playerBox.Y + playerBox.Height > hb.Y - 4 &&
                    playerBox.X < hb.X + hb.Width + 50 && playerBox.X > hb.X - 50)
                {
                    fallSpeed = 0;
                    jumpSpeed = maxJumpSpeed;
                    isGrounded = true;
                }
                //kolizja z dolna krawedzia zatrzymuje skok // dziala
                if (playerBox.Y < hb.Y + hb.Height + 40 && playerBox.Y > hb.Y + hb.Height &&
                    playerBox.X < hb.X + hb.Width && playerBox.X > hb.X)
                {
                    jumpSpeed = 0;
                }

                //kolizja z bocznymi krawedziami 
                if (playerBox.Contains(hb.X, hb.Y + 10) || playerBox.Contains(hb.X + hb.Width, hb.Y + hb.Height - 10))
                {
                    playerSideCollison = true;
                }

            }
        }


    }
}

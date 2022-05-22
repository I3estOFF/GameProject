using System.Drawing;
using System.Threading.Tasks;

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
        public bool sprinting = false;
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
        public int hearts = 3;
        public bool helmeton = false;

        Graphics gPlayer;
        Bitmap playerBitmap;
        World w;
        public Rectangle playerBox; //hitbox postaci
        Overlay overlay;
        ObjectCollection objectCollection;



        public Player(Graphics _g, Bitmap _player, int W, int H, ObjectCollection objectCollection)
        {
            playerBox = new Rectangle(80, H - 200, Properties.Resources.Chungus.Width, Properties.Resources.Chungus.Height);
            gPlayer = _g;
            playerBitmap = _player;
            Width = W;
            Heigth = H;
            this.objectCollection = objectCollection;

        }

        public void Reset()
        {
            playerBox.X = 80;
            playerBox.Y = Heigth - 200;
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

            //zmiana skina zależnie od power-upów
            if (playereatgold == true && playerhavekuboty == true && playerhavehelmet == false)
            {
                playerBitmap = Properties.Resources.goldenchunguskuboty;
            }
            else if (playereatgold == true && playerhavekuboty == false && playerhavehelmet == false)
            {
                playerBitmap = Properties.Resources.goldenchungus;
            }
            else if (playerhavekuboty == true && playereatgold == false && playerhavehelmet == false)
            {
                playerBitmap = Properties.Resources.chunguskuboty;
            }
            else if (playerhavehelmet == true && playerhavekuboty == false && playereatgold == false)
            {
                playerBitmap = Properties.Resources.Chungushelmet;
            }
            else if (playerhavehelmet == true && playereatgold == true && playerhavekuboty == false)
            {
                playerBitmap = Properties.Resources.goldenchungushelmet;
            }
            else if (playerhavehelmet == true && playereatgold == true && playerhavekuboty == true)
            {
                playerBitmap = Properties.Resources.goldenchunguskubotyhelmet;
            }
            else if (playerhavehelmet == true && playerhavekuboty == true && playereatgold == false)
            {
                playerBitmap = Properties.Resources.chunguskubotyhelmet;
            }
            else
                playerBitmap = Properties.Resources.Chungus;


            //check czy sprintuje
            if (sprinting)
                maxPlayerSpeed = 12;
            else if (playerhavekuboty)
                maxPlayerSpeed = 15;
            else
                maxPlayerSpeed = 7;

            playerSpeed = maxPlayerSpeed;



            // podstawowy ruch
            if (faceLeft)                            // zmienia zwrot postaci
            {
                playerBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            gPlayer.DrawImage(playerBitmap, playerBox.X, playerBox.Y);

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
                playerBox.Y -= jumpSpeed / 10;
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
                w.screenScrollSpeed = w.boostedScrollSpeed;

            }
            else
            {
                w.screenScrollSpeed = w.actualScrollSpeed;
            }
        }

        public void PlatformPlayerCollision()                                                                                   //kolizja gracza z platformą
        {
            playerSideCollison = false;
            foreach (Rectangle hb in w.PlatformHB)
            {
                // kolizja gracza z gorna krawedzia zatrzymuje opadanie //dziala
                if (playerBox.Y + playerBox.Height < hb.Y + 20 && playerBox.Y + playerBox.Height > hb.Y - 4 &&
                    playerBox.X < hb.X + hb.Width + 20 && playerBox.X > hb.X - 50)
                {
                    playerBox.Y = hb.Y - playerBox.Height;
                    fallSpeed = 0;
                    jumpSpeed = maxJumpSpeed;
                    isGrounded = true;
                }
                //kolizja z dolna krawedzia zatrzymuje skok // dziala
                if (playerBox.Y < hb.Y + hb.Height + 40 && playerBox.Y > hb.Y + hb.Height &&
                    playerBox.X < hb.X + hb.Width + 5 && playerBox.X > hb.X - 40)
                {
                    jumpSpeed = 0;
                }
                //kolizja z bocznymi krawedziami 
                if (playerBox.Contains(hb.X - 5, hb.Y + 20) || playerBox.Contains(hb.X + hb.Width + 10, hb.Y + 20))
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
            if (!toDelete.IsEmpty)
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
                maxPlayerSpeed = 15;
                playerhavekuboty = true;
                await Task.Delay(5000);                          //daje przyspieszenie na 5 sekund
                playerhavekuboty = false;
                maxPlayerSpeed = 7;
            }
        }

        public void HelmetPlayerCollision()                                                                          //kolizja gracza z hełmem
        {
            Rectangle toDeleteg = new Rectangle();
            foreach (Rectangle hm in w.helmets)
            {
                if (playerBox.Contains(hm))
                {
                    toDeleteg = hm;
                    helmeton = true;                                //tymczasowo regeneruje życia i daje +1
                }
            }
            if (!toDeleteg.IsEmpty)
            {
                w.helmets.Remove(toDeleteg);
                playerhavehelmet = true;
            }
        }

        public void MeteorPlayerCollision()                                                                          //kolizja gracza z meteorytem
        {
            Rectangle toDelete = new Rectangle();
            foreach (Rectangle mt in w.meteorites)
            {
                Rectangle temp = new Rectangle(mt.X + 12, mt.Y + 8, 25, 25);
                if (playerBox.IntersectsWith(temp))
                {
                    if (helmeton == false)
                    {
                        hearts -= 1;
                    }
                    else
                        helmeton = false;

                    if (hearts < 1)
                        overlay.GameOver();
                    toDelete = mt;
                    playerhavehelmet = false;
                }
            }
            if (!toDelete.IsEmpty)
            {
                w.meteorites.Remove(toDelete);
            }
        }
    }
}

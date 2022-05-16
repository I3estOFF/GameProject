using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class World
    {
        public List<Rectangle> PlatformHB = new List<Rectangle>(); // hitboxy platform
        public List<Rectangle> clouds = new List<Rectangle>(); // hitboxy platform
        public List<Rectangle> carrots = new List<Rectangle>();
        public List<Rectangle> emptys = new List<Rectangle>();
        public List<Rectangle> gcarrots = new List<Rectangle>();
        public List<Rectangle> kuboty = new List<Rectangle>();
        public List<Rectangle> meteorites = new List<Rectangle>();
        public Point explosion = new Point();
        public Boolean boomed = false;

        int kubotyWidth = Properties.Resources.Kuboty.Width;
        int carrotWidth = Properties.Resources.carrotnew.Width;
        int platWidth = Properties.Resources._311.Width;
        private Graphics gBackground;
        private int resolutionWidth;
        private int resolutionHeight;
        
        public int screenScrollSpeed = 0;
        public int screenScrollSpeed2 = 0;
        public int screenScrollSpeed3 = 6;
        public int xplat;
        public int yplat;
        public int platw;

        Player p;

        public World(Graphics gBackground, int resoultionWidth, int resolutionHeight)
        {
            this.gBackground = gBackground;
            this.resolutionWidth = resoultionWidth;
            this.resolutionHeight = resolutionHeight;
        }

        public void SetPlayer(Player p) { this.p = p; }

        public void generateGround(int posX, int posY, int _width)                                               //generowanie platformy startowej
        {
            gBackground.DrawImage(Properties.Resources._311, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, platWidth * _width, 1);
            PlatformHB.Add(rect);
            Bitmap plat = Properties.Resources._311;
            int len = 0;
            for (int i = 0; i < _width; i++)
            {
                gBackground.DrawImage(plat, posX + len, posY);
                len += platWidth;
            }
        }

        //adds new platform to the list

        public void RenderPlatforms()                                                                                 //renderowanie platform
        {
            //przygotowac zestaw platform o roznych rozmiarach
            Rectangle temp;

            Bitmap plat = Properties.Resources._311;
            int dlen;
            int begLen;
            for (int i = 0; i < PlatformHB.Count; i++)
            {

                temp = PlatformHB[i];
                PlatformHB[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    PlatformHB.RemoveAt(i);
                if (temp != null)
                {
                    dlen = 0;
                    begLen = temp.Width / platWidth;
                    for (int j = 0; j < begLen; j++)
                    {

                        gBackground.DrawImage(plat, temp.Left + dlen, temp.Top);
                        dlen += platWidth;
                    }
                }
            }
            p.playerBox.Y += screenScrollSpeed;
        }

        public void generatePlatformRandom(int numberOf)                                                            //generowanie platform
        {
            Random rand = new Random();

            int ranX = 0;
            int ranY = 0;
            int ranWidth = 0;

            int prevRanX = 0;
            int prevRanY = 0;
            int prevRanWidth = 0;
            xplat = PlatformHB.LastOrDefault().X;
            yplat = ranY = PlatformHB.LastOrDefault().Y;
            platw = PlatformHB.LastOrDefault().Width / platWidth;
            if (PlatformHB.Count > 1)
            {
                ranX = PlatformHB.LastOrDefault().X;
                ranY = PlatformHB.LastOrDefault().Y;
                ranWidth = PlatformHB.LastOrDefault().Width/platWidth;
            }
            else
            {
                ranX = rand.Next(50, resolutionWidth/2);
                ranY = resolutionHeight - 100;
                ranWidth = 10;
            }
            if (ranY < -200)
                return;

            int jumpDistance = 70;

            int ranXRight;
            int minY = 120; // ponad glowa gracza
            int maxY = 270; //wysokosc skoku

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
                    if (prevRanX - jumpDistance > ranX)
                        ranXRight = rand.Next(prevRanX - jumpDistance, prevRanX + (prevRanWidth - 3) * platWidth - jumpDistance);
                    else
                    {
                        ranXRight = rand.Next(ranX, prevRanX - jumpDistance + (prevRanWidth - 3) * platWidth);
                    }
                    ranWidth = (ranXRight - ranX) / platWidth + 5;
                }
                else if (ranX >= prevRanX) // jesli zaczyna sie na poprzedniej platformie lub za nia
                {
                    //ranX = ranX + 100;
                    if (ranX < prevRanX + prevRanWidth * platWidth + jumpDistance)
                    {
                        ranXRight = rand.Next(ranX, prevRanX + prevRanWidth * platWidth + jumpDistance);

                        ranWidth = (ranXRight - ranX) / platWidth + 5;
                    }
                    else
                    {
                        ranX = prevRanX + prevRanWidth * platWidth + jumpDistance;
                        ranWidth = rand.Next(7, 10);
                    }
                }
                for (int n = 0; n < 10; n++)
                {
                    if (ranX + ranWidth * platWidth >= resolutionWidth)
                    {
                        ranX -= 2 * (ranX + ranWidth * platWidth - resolutionWidth);
                    }
                }
                Rectangle rect = new Rectangle(ranX, ranY, ranWidth*platWidth, 1);
                PlatformHB.Add(rect);
            }
        }

        public void RenderCarrots()                                                                                         //renderowanie marchewek
        {
            Rectangle temp;
            Bitmap carrot = Properties.Resources.carrotnew;
            for (int i = 0; i < carrots.Count; i++)
            {
                temp = carrots[i];
                carrots[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    carrots.RemoveAt(i);
                if (temp != null)
                {
                   gBackground.DrawImage(carrot, temp.Left, temp.Top);
                }
            }
        }

        public void generateCarrotRandom()                                                                                 //generowanie marchewek
        {
            Random rand = new Random();
            int ranHunit;
            ranHunit = rand.Next(1, 400);
            int ranX = 0;

            if (carrots.Count > 1)
            {
                ranX = carrots.LastOrDefault().X;             
            }
            else
            {
                ranX = rand.Next(xplat, xplat + platw);
            }
                ranX = rand.Next(xplat, xplat + platw);
                Rectangle rect = new Rectangle(xplat + ranHunit, yplat - 30,carrotWidth, 1);
                carrots.Add(rect);
        }

        public void RenderGoldenCarrots()                                                                                  //renderowanie złotych marchewek
        {
            Rectangle temp;
            Bitmap gcarrot = Properties.Resources.goldencarrot;
            for (int i = 0; i < gcarrots.Count; i++)
            {
                temp = gcarrots[i];
                gcarrots[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    gcarrots.RemoveAt(i);
                if (temp != null)
                {
                    gBackground.DrawImage(gcarrot, temp.Left, temp.Top);
                }
            }
        }

        public void generateGoldenCarrotRandom()                                                                          //generowanie złotych marchewek
        {
            Random rand = new Random();

            int ranX = 0;
            int ranHunit;
            ranHunit = rand.Next(30, 350);
            if (gcarrots.Count > 1)
            {
                ranX = gcarrots.LastOrDefault().X;
            }
            else
            {
                ranX = rand.Next(xplat, xplat + platw);
            }

            ranX = rand.Next(xplat, xplat + platw);
            Rectangle rect = new Rectangle(xplat + ranHunit, yplat - 30, carrotWidth, 1);
            gcarrots.Add(rect);
        }

        public void RenderKubots()                                                                                  //renderowanie złotych marchewek
        {
            Rectangle temp;
            Bitmap kubot = Properties.Resources.Kuboty;
            for (int i = 0; i < kuboty.Count; i++)
            {
                temp = kuboty[i];
                kuboty[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    kuboty.RemoveAt(i);
                if (temp != null)
                {
                    gBackground.DrawImage(kubot, temp.Left, temp.Top);
                }
            }
        }

        public void generateKubotsRandom()                                                                          //generowanie złotych marchewek
        {
            Random rand = new Random();

            int ranX = 0;
            int ranHunit = 0;
            ranHunit = rand.Next(20, 400);
            if (kuboty.Count > 1)
            {
                ranX = kuboty.LastOrDefault().X;
            }
            else
            {
                ranX = rand.Next(xplat, xplat + platw);
            }

            ranX = rand.Next(xplat, xplat + platw);
            Rectangle rect = new Rectangle(xplat + ranHunit, yplat - 30, carrotWidth, 1);
            kuboty.Add(rect);
        }


        public void RenderClouds()                                                                                        //renderowanie chmur
        {
            Rectangle temp;
            Bitmap cloud = Properties.Resources.cloudsT;
            for (int i = 0; i < clouds.Count; i++)
            {
                temp = clouds[i];
                clouds[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed2, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + cloud.Height)
                {
                    clouds.RemoveAt(i);
                }
                if (temp.Top < resolutionHeight+cloud.Height && temp!=null)
                    gBackground.DrawImage(cloud, temp.Left, temp.Top - cloud.Height);
            }
        }
        public void generateCloud()                                                                                         //generowanie chmur
        {
            int cloudWidth = Properties.Resources.cloudsT.Width;
            int cloudHeight = Properties.Resources.cloudsT.Height;
            Random rand = new Random();
            int ranX = rand.Next(-50, (resolutionWidth + 50)/2-cloudWidth);
            int ranY = -cloudHeight;

           Rectangle rect = new Rectangle(ranX, ranY, cloudWidth, 1);
           clouds.Add(rect);
           
           ranX = rand.Next((resolutionWidth + 50) / 2 - cloudWidth, (resolutionWidth + 50) / 2+ (resolutionWidth + 50) / 2);
           ranY = rand.Next(-2 * cloudHeight, -3*cloudHeight/2);  
           
           rect = new Rectangle(ranX, ranY, cloudWidth, 1);
           clouds.Add(rect);
        }

        public void RenderMeteorite()                                                                                        //renderowanie chmur
        {
            Rectangle temp;
            Bitmap meteorite = Properties.Resources.Meteoritesmall;
            for (int i = 0; i < meteorites.Count; i++)
            {
                temp = meteorites[i];
                meteorites[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed3, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + meteorite.Height)
                {
                    meteorites.RemoveAt(i);
                }
                if (temp.Top < resolutionHeight + meteorite.Height && temp != null)
                    gBackground.DrawImage(meteorite, temp.Left, temp.Top - meteorite.Height);
            }
        }
        public void generateMeteorite()                                                                                         //generowanie chmur
        {
            int meteoriteWidth = Properties.Resources.Meteoritesmall.Width;
            int meteoriteHeight = Properties.Resources.Meteoritesmall.Height;
            Random rand = new Random();
            int ranX = rand.Next(50, (resolutionWidth - 50));
            int ranY = -meteoriteHeight;

            Rectangle rect = new Rectangle(ranX, ranY, meteoriteWidth, 1);
            meteorites.Add(rect);

            ranX = rand.Next(50, (resolutionWidth - 50));
            ranY = rand.Next(-2 * meteoriteHeight, -3 * meteoriteHeight / 2);

            rect = new Rectangle(ranX, ranY, meteoriteWidth, 1);
            meteorites.Add(rect);
        }

        public void renderExplosion()
        {
            Bitmap boom = Properties.Resources.exploooosion;


            gBackground.DrawImage(boom, explosion.X - 70,explosion.Y - 70);
        }
    }
}

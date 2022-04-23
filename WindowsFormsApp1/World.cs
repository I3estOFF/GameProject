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

        int platWidth = Properties.Resources._311.Width;
        int platHeight = Properties.Resources._311.Height;
        private Graphics gBackground;
        private int resolutionWidth;
        private int resolutionHeight;

        private int screenScrollSpeed = 3;

        Player p;




        public World(Graphics gBackground, int resoultionWidth, int resolutionHeight)
        {
            this.gBackground = gBackground;
            this.resolutionWidth = resoultionWidth;
            this.resolutionHeight = resolutionHeight;
        }

        public void SetPlayer(Player p) { this.p = p; }

        public void generateGround(int posX, int posY, int _width)
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
        public void RenderPlatforms()
        {
            //przygotowac zestaw platform o roznych rozmiarach
            Rectangle temp;
            for (int i = 0; i < PlatformHB.Count; i++)
            {
                temp = PlatformHB[i];
                PlatformHB[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
            }
            p.playerBox.Y += screenScrollSpeed;

            Bitmap plat = Properties.Resources._311;

            int dlen;
            int begLen;
            foreach (Rectangle platform in PlatformHB)
            {
                dlen = 0;
                begLen = platform.Width / platWidth;
                for (int i = 0; i < begLen; i++)
                {
                    gBackground.DrawImage(plat, platform.Left + dlen, platform.Top);
                    dlen += platWidth;
                }
            }


        }
        //adds new platform to the list
        public void generatePlatformRandom(int numberOf)
        {
            Random rand = new Random();

            int ranX = 0;
            int ranY = 0;
            int ranWidth = 0;

            int prevRanX = 0;
            int prevRanY = 0;
            int prevRanWidth = 0;

            if (PlatformHB.Count > 1)
            {
                ranX = PlatformHB.LastOrDefault().X;
                ranY = PlatformHB.LastOrDefault().Y;
                ranWidth = PlatformHB.LastOrDefault().Width/platWidth;
            }
            else
            {
                ranX = rand.Next(50, resolutionWidth - 200);
                ranY = resolutionHeight - 100;
                ranWidth = 10;
            }



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
                    if (ranX + ranWidth * platWidth >= resolutionWidth)
                    {

                        System.Diagnostics.Debug.WriteLine("too far right");
                        ranX -= 2 * (ranX + ranWidth * platWidth - resolutionWidth);
                    }

                }


                Rectangle rect = new Rectangle(ranX, ranY, ranWidth*platWidth, 1);
                PlatformHB.Add(rect);
            }




        }
    }
}

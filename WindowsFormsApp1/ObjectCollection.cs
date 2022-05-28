using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class ObjectCollection
    {
        public Timer timer1;
        public Timer timer2;
        public Timer timer3;
        public Timer timer4;
        public Timer timer5;
        public Timer timer6;
        public Graphics Background;
        public Graphics Player;
        public Label label;
        public PictureBox picturebox2;
        public int resolutionHeight;
        public int resolutionWidth;
        public Player player;
        public World world;
        public Label scoreLabel;

        public ObjectCollection(Timer timer1, Timer timer2, Timer timer3, Timer timer4, Timer timer5, Timer timer6, Graphics backgroundgraphics, Graphics playerGraphics, Label label1, PictureBox pictureBox2, int width, int height, Label scoreboard)
        {
            this.timer1 = timer1;
            this.timer2 = timer2;
            this.timer3 = timer3;
            this.timer4 = timer4;
            this.timer5 = timer5;
            this.timer6 = timer6;
            this.Background = backgroundgraphics;
            this.Player = playerGraphics;
            this.label = label1;
            this.picturebox2 = pictureBox2;
            this.resolutionHeight = height;
            this.resolutionWidth = width;
            scoreLabel = scoreboard;
        }

        public void SetPlayer(Player p)
        {
            player = p;
        }
        public void SetWorld(World w)
        {
            world = w;
        }
    }
}

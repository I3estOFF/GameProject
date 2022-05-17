using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Overlay
    {
        private PictureBox OverlayLayer;
        private ObjectCollection objectCollection;

        public Overlay(PictureBox OverlayLayer, ObjectCollection objectCollection)
        {
            this.OverlayLayer = OverlayLayer;
            this.objectCollection = objectCollection;
        }


        public void GameOver()
        {
            OverlayLayer.Image = Properties.Resources.game_over1;
            OverlayLayer.Visible = true;
            objectCollection.timer1.Enabled = false;
            objectCollection.timer2.Enabled = false;
            objectCollection.timer3.Enabled = false;
        }



    }
}

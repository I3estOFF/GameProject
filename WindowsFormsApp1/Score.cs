using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Score
    {
        public int points;
        public int carrots;
        public int goldenCarrots;
        public int summary;

        public Score(int points, int carrots, int goldenCarrots)
        {
            this.points = points;
            this.carrots = carrots;
            this.goldenCarrots = goldenCarrots;
            summary = points + carrots * 10 + goldenCarrots * 100;

        }
    }
}

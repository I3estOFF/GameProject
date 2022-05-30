using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{

    internal class ScoreBoard
    {
        public List<Score> scores = new List<Score>();

        public List<String> scoresCSV = new List<String>();

        public void AddScore(int points, int carrots, int goldenCarrots)
        {
            scores.Add(new Score(points, carrots, goldenCarrots));
            scores.Sort(delegate (Score s1, Score s2) { return s2.summary.CompareTo(s1.summary); });

            scoresCSV.Add(points + "," + carrots + "," + goldenCarrots);

            if(scores.Count > 6)
            {
                scores.RemoveAt(6);
            }
        }

        override public String ToString()
        {

            String toPrint = " \n\n \n \n \n";
            int i = 0;
            foreach (Score score in scores)
            {
                toPrint += "                                   " + score.points.ToString() + "             " + score.carrots.ToString() + "                 " + score.goldenCarrots.ToString() + "\n\n\n";
                i++;

                if (i > 7)
                    break;

            }

            return toPrint;
        }


    }
}

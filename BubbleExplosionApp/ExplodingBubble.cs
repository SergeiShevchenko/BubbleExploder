using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BubbleExplosionApp
{
    class ExplodingBubble
    {
        public float x, y;
        public int color;
        public int maxradius=30, curradius;
        public bool imploding;
        public ExplodingBubble(float X, float Y, int cur, int COL)
        {
            x = X;
            y = Y;
            curradius = cur;
            color = COL;
        }
    }
}

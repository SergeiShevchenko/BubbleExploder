using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BubbleExplosionApp
{
    class Bubble
    {
        public float x;
        public float y;
        public float vx, vy;
        public int rad;
        public int color;
        public int mass;
        static Random r = new Random();
        public Bubble(Playground p)
        {
            x = r.Next(p.rx-p.lx-2*rad)+rad;
            y = r.Next(p.ry - p.ly - 2 * rad) + rad;
            vx = r.Next(3)+1;
            if (!p.SameSizedBall) rad = r.Next(10) + 7;
            else rad = 10;
            if (r.Next(2) == 0) vx = -vx;
            vy = (float)(Math.Tan(r.Next(180)) * vx); 
            while (vy>=10)
            {
                vy = (int)Math.Round(Math.Tan(r.Next(180)) * vx); 
            }
            color = r.Next(9);
            mass = rad * rad;
        }

        public Bubble(float x1, float y1, int rad1, Playground p, int c)
        {
            x = x1; y = y1; rad = rad1;
            vx = r.Next(3) + 1;
            if (r.Next(2) == 0) vx = -vx;
            vy = (int)Math.Round(Math.Tan(r.Next(180)) * vx);
            while (vy >= 15)
            {
                vy = (int)Math.Round(Math.Tan(r.Next(180)) * vx);
            }
            color = c;
            mass = rad * rad;
        }

        public Bubble()
        {
            x = 0;
            y = 0;
            rad = 10;
            color = 15;
            mass = rad * rad;
        }
        
    }
}

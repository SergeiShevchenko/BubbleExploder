using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace BubbleExplosionApp
{
    class Playground
    {
        public int lx, ly, rx, ry;
        public int ballstowin;
        public int ballsfromthestart;
        public List<Bubble> bubbles=new List<Bubble>();
        public List<ExplodingBubble> exbubbles = new List<ExplodingBubble>();
        public double BounceEnergy = 0.95;
        public int maxspeed=20;
        
        //rules

        public bool SameSizedBall = true;
        public bool ImprobabilityDrive = false;
        public bool EnergyBoost = false;

        public Playground(int x1, int y1, int x2, int y2)
        {
            lx = x1;
            ly = y1;
            rx = x2;
            ry = y2;            
        }
        
        public bool IsOn()
        {
            if ((bubbles.Count <= (ballsfromthestart-ballstowin))&&(exbubbles.Count==0)) return false;
            return true;
        }

        public Color CodeToColor(int x)
        {
            if (x == 0) return Color.White;
            if (x == 1) return Color.Beige;
            if (x == 2) return Color.Yellow;
            if (x == 3) return Color.Gold;
            if (x == 4) return Color.Blue;
            if (x == 5) return Color.Red;
            if (x == 6) return Color.DarkGreen;
            if (x == 7) return Color.Brown;
            if (x == 8) return Color.Black;
            if (x == 15) return Color.Purple;
            return Color.Maroon;            
        }

        public void Explode(Bubble a)
        {
            ExplodingBubble eb = new ExplodingBubble(a.x, a.y, a.rad, a.color);
            exbubbles.Add(eb);
            bubbles.Remove(a);
        }

        public double Distance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public bool Intersect(Bubble a, ExplodingBubble eb)
        {
            return (Distance(a.x, a.y, eb.x, eb.y) <= eb.curradius + a.rad);            
        }

        public bool Intersect(Bubble a, Bubble eb)
        {
            return (Distance(a.x, a.y, eb.x, eb.y) <= eb.rad + a.rad);
        }

        class Vector
        {
            public double x { get; set; } public double y{get;set;}
            public Vector Subtract(Vector v)
            {
                return new Vector
                {
                    x = x - v.x,
                    y = y - v.y
                };
            }
            public Vector Add(Vector v)
            {
                return new Vector
                {
                    x = x + v.x,
                    y = y + v.y
                };
            }
            public double GetLength()
            {
                return Math.Sqrt(x * x + y * y);
            }
            public Vector multiply(double k)
            {
                return new Vector
                {
                    x = x * k,
                    y = y * k
                };
            }
            public Vector normalize()
            {
                return multiply(1.0 / GetLength());
            }
            public double dot(Vector v)
            {
                return x * v.x + y * v.y;
            }
        }

        public void Collide(Bubble a, Bubble b)
        {
            Vector position = new Vector { x = a.x, y = a.y };
            Vector position2 = new Vector { x = b.x, y = b.y };

            Vector collision = position.Subtract(position2);
            double distance = collision.GetLength();
            if (distance == 0.0)
            {             
                collision = new Vector { x = 1.0, y = 0.0 };
                distance = 1.0;
            }

            Vector veloc1 = new Vector
            {
                x = a.vx,
                y = a.vy
            };
            Vector veloc2 = new Vector
            {
                x = b.vx,
                y = b.vy
            };

            // Get the components of the velocity vectors which are parallel to the collision.
            // The perpendicular component remains the same for both fish
            collision = collision.multiply( 1.0 / distance);
            double a_before = veloc1.dot(collision);
            double b_before = veloc2.dot(collision);

            // Solve for the new velocities using the 1-dimensional elastic collision equations.
            // Turns out it's really simple when the masses are the same.
            double a_after = a_before * a.mass + b.mass * b_before + b.mass * (b_before - a_before) * BounceEnergy;
            double b_after = b_before * b.mass + a.mass * a_before + a.mass * (a_before - b_before) * BounceEnergy;

            a_after /= a.mass + b.mass;
            b_after /= a.mass + b.mass;

            // Replace the collision velocity components with the new ones
            veloc1 = veloc1.Add(collision.multiply(a_after - a_before));
            veloc2 = veloc2.Add(collision.multiply(b_after - b_before));

            a.vx = (float)veloc1.x;
            a.vy = (float)veloc1.y;
            b.vx = (float)veloc2.x;
            b.vy = (float)veloc2.y;

            double d = Distance(a.x, a.y, b.x, b.y);
            float cx = (a.x + b.x) / 2;
            float cy = (a.y + b.y) / 2;

            a.x = (float)(cx + (a.rad) * (a.x - b.x) / d);
            a.y = (float)(cy + (a.rad) * (a.y - b.y) / d);

            b.x = (float)(cx + (b.rad) * (b.x - a.x) / d);
            b.y = (float)(cy + (b.rad) * (b.y - a.y) / d);

            b.x = (float)(b.x - b.vx / d);
            b.y = (float)(b.y - b.vy / d);

            a.x = (float)(a.x - a.vx / d);
            a.y = (float)(a.y - a.vy / d);

          //  double vel1 = Math.Sqrt(a.vx * a.vx + a.vy * a.vy);
          //  double vel2 = Math.Sqrt(b.vx * b.vx + b.vy * b.vy);

          //  double fi1 = Math.Atan2(a.vy, a.vx);
          //  double fi2 = Math.Atan2(b.vy, b.vx);

          //  var lambda1 = Math.Atan2(b.y - a.y, b.x - a.x);
          //  var lambda2 = Math.Atan2(a.y - b.y, a.x - b.x);

          //  /**
          //* Получаем проекции скоростей на соединяющую линию между шарами
          //**/
          //  var pv1 = vel1 * Math.Cos(lambda1 - fi1)/*(vel1*(a.mass-b.mass)+2*b.mass*vel2)/(a.mass+b.mass)*/*BounceEnergy;// Math.Abs((a.rad - b.rad) / (a.rad + b.rad));
          //  var pv2 = vel2 * Math.Cos(lambda2 - fi2)/*(vel2*(b.mass - a.mass) + 2 * a.mass * vel1) / (a.mass + b.mass)*/*BounceEnergy;//Math.Abs((a.rad - b.rad) / (a.rad + b.rad));

          //  /**
          //* Получаем новые скорости (векроты направления)
          //**/
          //  a.vx = (float)(a.vx - pv1 * Math.Cos(lambda1) + pv2 * Math.Cos(lambda2));
          //  a.vy = (float)(a.vy - pv1 * Math.Sin(lambda1) + pv2 * Math.Sin(lambda2));
          //  if (Math.Abs(a.vy) > maxspeed) a.vy = maxspeed;
          //  a.tangent = a.vy / a.vx;
          //  b.vx = (float)(b.vx - pv2 * Math.Cos(lambda2) + pv1 * Math.Cos(lambda1));
          //  b.vy = (float)(b.vy - pv2 * Math.Sin(lambda2) + pv1 * Math.Sin(lambda1));
          //  if (Math.Abs(b.vy) > maxspeed) b.vy = maxspeed;
          //  b.tangent = b.vy / b.vx;
        }
        List<ExplodingBubble> ebtd = new List<ExplodingBubble>();
        List<Bubble> bte = new List<Bubble>();

        public void Go()
        {
            ebtd.Clear();
            foreach (ExplodingBubble eb in exbubbles)
            {
                if (eb.imploding == false)
                {
                    eb.curradius++;
                    if (eb.curradius == eb.maxradius) eb.imploding = true;
                }
                else
                {
                    eb.curradius--;
                    if (eb.curradius == 0)
                    {
                        ebtd.Add(eb);
                    }
                }
            }

            foreach (ExplodingBubble eb in ebtd)
            {
                exbubbles.Remove(eb);
            }

            bte.Clear();

            foreach (Bubble b in bubbles)
                {
                    b.x = b.x + b.vx;
                    b.y = b.y + b.vy;
                    if (Math.Abs(b.vy) > maxspeed) 
                    {
                        if (b.vy > 0) b.vy = maxspeed;
                        else b.vy = -maxspeed;
                    }
                    if (b.y + b.rad >= ry)
                    {
                        b.y = ry - b.rad;
                        b.vy *= -1;
                    }
                    if (b.y - b.rad <= ly)
                    {
                        b.y = ly + b.rad;
                        b.vy *= -1;
                    }
                    if (b.x + b.rad >= rx)
                    {
                        b.vx = -b.vx;
                        b.x=rx-b.rad;
                        //b.vy *= -1;
                    }
                    if (b.x - b.rad <= lx)
                    {
                        b.vx = -b.vx;
                        b.x=lx+b.rad;
                       // b.vy *= -1;
                    }
                    foreach (ExplodingBubble eb in exbubbles)
                    {
                        if (Intersect(b, eb))
                            bte.Add(b);
                    }
                }
            for (int i = 0; i < bte.Count; i++)
            {
                Explode(bte[i]);
            }
            for (int i = 0; i < bubbles.Count; i++)
            {
                for (int j = i + 1; j < bubbles.Count; j++)
                {
                    if (Intersect(bubbles[i], bubbles[j]))
                    {
                        Collide(bubbles[i], bubbles[j]);
                    }
                }
            }            
        }

        public void GoNoExplosions()
        {
            foreach (Bubble b in bubbles)
            {
                b.x = b.x + b.vx;
                b.y = b.y + b.vy;
                if (Math.Abs(b.vy) > maxspeed)
                {
                    if (b.vy > 0) b.vy = maxspeed;
                    else b.vy = -maxspeed;
                }
                if (b.y + b.rad >= ry)
                {
                    b.y = ry - b.rad;
                    b.vy *= -1;
                }
                if (b.y - b.rad <= ly)
                {
                    b.y = ly + b.rad;
                    b.vy *= -1;
                }
                if (b.x + b.rad >= rx)
                {
                    b.vx = -b.vx;
                    b.x = rx - b.rad;
                    //b.vy *= -1;
                }
                if (b.x - b.rad <= lx)
                {
                    b.vx = -b.vx;
                    b.x = lx + b.rad;
                    // b.vy *= -1;
                }
            }
            for (int i = 0; i < bubbles.Count; i++)
            {
                for (int j = i + 1; j < bubbles.Count; j++)
                {
                    if (Intersect(bubbles[i], bubbles[j]))
                    {
                        Collide(bubbles[i], bubbles[j]);
                    }
                }
            }        
        }

    }
}

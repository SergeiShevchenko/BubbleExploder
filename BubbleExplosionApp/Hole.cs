using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BubbleExplosionApp
{
    public partial class Hole : Form
    {
        public Hole()
        {
            InitializeComponent();
        }


        void Draw(Graphics g)
        {
            g.Clear(Color.White);
            g.DrawRectangle(new Pen(Brushes.Black, 1), p.lx, p.ly, p.rx - p.lx-1, p.ry - p.ly-1);

            g.FillEllipse(new SolidBrush(Color.Red), bh.x - bh.rad, bh.y - bh.rad, 2 * bh.rad, 2 * bh.rad);
            g.DrawEllipse(new Pen(Brushes.Black, 1), bh.x - bh.rad, bh.y - bh.rad, 2 * bh.rad, 2 * bh.rad);
            g.FillEllipse(new SolidBrush(Color.Black), bh.x - bh.rad/2, bh.y - bh.rad/2, bh.rad, bh.rad);
            

            g.FillEllipse(new SolidBrush(Color.Yellow), gh.x - gh.rad, gh.y - gh.rad, 2 * gh.rad, 2 * gh.rad);
            g.DrawEllipse(new Pen(Brushes.Black, 1), gh.x - gh.rad, gh.y - gh.rad, 2 * gh.rad, 2 * gh.rad);
            g.FillEllipse(new SolidBrush(Color.White), gh.x - gh.rad / 2, gh.y - gh.rad / 2, gh.rad, gh.rad);
            
            
            foreach (Bubble b in p.bubbles)
            {
                g.FillEllipse(new SolidBrush(p.CodeToColor(b.color)), b.x - b.rad, b.y - b.rad, 2 * b.rad, 2 * b.rad);
                g.DrawEllipse(new Pen(Brushes.Black, 1), b.x - b.rad, b.y - b.rad, 2 * b.rad, 2 * b.rad);
            }


        }

        Playground p = new Playground(0, 0, 352, 407);
        
        GoodHole gh = new GoodHole();
        GoodHole bh = new GoodHole();

        private void Hole_Load(object sender, EventArgs e)
        {            
            trackBar1.Minimum = p.lx;
            trackBar1.Maximum = p.rx;            
            trackBar2.Minimum = p.ly;
            trackBar2.Maximum = p.ry;

            trackBar5.Minimum = p.ly;
            trackBar5.Maximum = p.ry;
            trackBar6.Minimum = p.lx;
            trackBar6.Maximum = p.rx;

            trackBar1.Value = 20;
            trackBar2.Value = 20;

            trackBar5.Value = 300;
            trackBar6.Value = 250;

            p.SameSizedBall = checkBox1.Checked;
            p.BounceEnergy = 0.97;
            bh.ReallyGood = false;

            refreshCoordinates();
        }

        Timer _timer = new Timer();
        Timer EnBoost = new Timer();
        bool play = false;
        bool weclicked = false;

        private void button1_Click(object sender, EventArgs e)
        {            
            if (play==false)
            {
                play = true;
                for (int i = 1; i <= numericUpDown1.Value; i++)
                {
                    Bubble b = new Bubble(p);
                    p.bubbles.Add(b);
                }
                _timer.Interval = 100 - trackBar7.Value;
                _timer.Enabled = true;
                numericUpDown1_ValueChanged(numericUpDown1, null);
                pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
                button1.Text = "Stop it!";                
                checkBox1_CheckStateChanged(checkBox1, null);
                checkBox2_CheckStateChanged(checkBox2, null);
                _timer.Tick += new EventHandler(_timer_Tick);
                EnBoost.Tick += new EventHandler(EnBoost_Tick);
                if (p.EnergyBoost)
                {
                    EnBoost.Interval = 1000;
                    EnBoost.Enabled = true;
                }
            }
            else
            {
                play = false;
                weclicked = false;
                _timer.Enabled = false;                
                EnBoost.Enabled = false;
                p.bubbles.Clear();
                p.exbubbles.Clear();
                pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
                pictureBox1.Invalidate();
                button1.Text = "Let's rock!";                
            }
        }


        private void _timer_Tick(object sender, EventArgs e)
        {
            p.GoNoExplosions();
            pictureBox1.Invalidate();
            if (weclicked&&play)
            {
                if (p.Distance(p.bubbles[p.bubbles.Count - 1].x, p.bubbles[p.bubbles.Count - 1].y, bh.x, bh.y) < bh.rad + p.bubbles[p.bubbles.Count-1].rad)
                {
                    play = false;
                    weclicked = false;
                    _timer.Enabled = false;
                    _timer.Stop();
                    MessageBox.Show("You have lost. The ball went in the wrong hole =)");
                    EnBoost.Enabled = false;
                    p.bubbles.Clear();
                    p.exbubbles.Clear();                    
                    pictureBox1.Invalidate();
                    button1.Text = "Let's rock!";                    
                }

                else if (p.Distance(p.bubbles[p.bubbles.Count - 1].x, p.bubbles[p.bubbles.Count - 1].y, gh.x, gh.y) < gh.rad + p.bubbles[p.bubbles.Count - 1].rad)
                {
                    play = false;
                    weclicked = false;
                    _timer.Enabled = false;
                    _timer.Stop();
                    MessageBox.Show("You have won =) The ball is just in the right hole.");
                    EnBoost.Enabled = false;
                    p.bubbles.Clear();
                    p.exbubbles.Clear();
                    pictureBox1.Invalidate();
                    button1.Text = "Let's rock!";
                }
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            p.ballsfromthestart = (int)numericUpDown1.Value;            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            p.SameSizedBall = checkBox1.Checked;
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            p.EnergyBoost  = checkBox2.Checked;
            EnBoost.Enabled = checkBox2.Checked;
        }

        private void refreshCoordinates()
        {
            gh.x = trackBar1.Value;
            gh.y = trackBar2.Maximum-trackBar2.Value;
            gh.rad = trackBar3.Value;

            bh.x = trackBar6.Value;
            bh.y = trackBar5.Maximum-trackBar5.Value;
            bh.rad = trackBar4.Value;
            pictureBox1.Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            refreshCoordinates();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            refreshCoordinates();
        }
        
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            refreshCoordinates();
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            refreshCoordinates();
        }

        private void trackBar5_ValueChanged(object sender, EventArgs e)
        {
            refreshCoordinates();
        }

        private void trackBar6_ValueChanged(object sender, EventArgs e)
        {
            refreshCoordinates();
        }

        Bubble ourbubble = new Bubble();

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!weclicked&&play)
            {
                ourbubble.x = e.X;
                ourbubble.y = e.Y;
                ourbubble.vx = 0;
                ourbubble.vy = 0;
                p.bubbles.Add(ourbubble);
                weclicked = true;
            }            
        }

        private void trackBar7_ValueChanged(object sender, EventArgs e)
        {
            _timer.Interval = 100 - trackBar7.Value;
        }
        
        private void EnBoost_Tick(object sender, EventArgs e)
        {
            if (p.bubbles.Count == 0) return;
            Random r = new Random();
            int prey = r.Next(p.bubbles.Count);
            if (p.bubbles[prey].vx < 0) p.bubbles[prey].vx -= 1;
            else p.bubbles[prey].vx += 1;
            //p.bubbles[prey].vy = (float)(p.bubbles[prey].tangent * p.bubbles[prey].vx);
        }
    }
}

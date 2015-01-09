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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _timer.Tick += new System.EventHandler(this._timer_Tick);
            _timer.Enabled = false;
            improTimer.Tick += new System.EventHandler(this.improTimer_Tick);
            EnBoost.Tick += new System.EventHandler(this.EnBoost_Tick);
        }


        void Draw(Graphics g)
        {

                g.Clear(Color.White);
                g.DrawRectangle(new Pen(Brushes.Black, 1), p.lx, p.ly, p.rx - p.lx, p.ry - p.ly);
                foreach (Bubble b in p.bubbles)
                {                   
                    
                    g.FillEllipse(new SolidBrush(p.CodeToColor(b.color)), b.x - b.rad, b.y - b.rad, 2 * b.rad, 2 * b.rad);
                    g.DrawEllipse(new Pen(Brushes.Black, 1), b.x - b.rad, b.y - b.rad, 2 * b.rad, 2 * b.rad);                    
                }
                foreach (ExplodingBubble eb in p.exbubbles)
                {
                    g.FillEllipse(new SolidBrush(p.CodeToColor(eb.color)), eb.x - eb.curradius, eb.y - eb.curradius, 2 * eb.curradius, 2 * eb.curradius);
                    g.DrawEllipse(new Pen(Brushes.Black, 1), eb.x - eb.curradius, eb.y - eb.curradius, 2 * eb.curradius, 2 * eb.curradius);
                    
                }
            //pictureBox1.Invalidate();
        }       
            
             

        Playground p = new Playground(0, 0, 300, 300);//pictureBox1.Left, pictureBox1.Top, pictureBox1.Width, pictureBox1.Height);
        bool weclicked = false;

        //timers

        Timer _timer = new Timer();
        Timer improTimer = new Timer();
        Timer EnBoost = new Timer();

        Random impR = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load(button1, null);
            if (!p.IsOn())
            {
                for (int i = 1; i <= numericUpDown1.Value; i++)
                {
                    Bubble b = new Bubble(p);
                    p.bubbles.Add(b);
                }
                _timer.Interval = 100 - trackBar1.Value;
                _timer.Enabled = true;
                numericUpDown1_ValueChanged(numericUpDown1, null);
                pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
                button1.Text = "Stop it!";
                panel1.Visible = true;
                checkBox1_CheckStateChanged(checkBox1, null);
                checkBox2_CheckStateChanged(checkBox2, null);
                
                if (p.ImprobabilityDrive)
                {
                    improTimer.Interval = impR.Next(3000);
                    improTimer.Enabled = true;
                }
                if (p.EnergyBoost)
                {
                    EnBoost.Interval = 1000;
                    EnBoost.Enabled = true;
                }
            }
            else
            {                
                _timer.Enabled=false;
                improTimer.Enabled = false;
                EnBoost.Enabled = false;
                p.bubbles.Clear();
                p.exbubbles.Clear();
                pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
                pictureBox1.Invalidate();
                button1.Text = "Go!";
                panel1.Visible = false;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!weclicked)
            {
                ExplodingBubble eb = new ExplodingBubble(e.X, e.Y, 1, 1);
                p.exbubbles.Add(eb);
                weclicked = true;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void improTimer_Tick(object sender, EventArgs e)
        {
            if (p.bubbles.Count == 0) return;
            Random r = new Random();
            int prey = r.Next(p.bubbles.Count);
            Bubble bn = new Bubble(p.bubbles[prey].x, p.bubbles[prey].y, p.bubbles[prey].rad, p, p.bubbles[prey].color);
            p.bubbles.Remove(p.bubbles[prey]);
            p.bubbles.Add(bn);
            improTimer.Interval = impR.Next(3000);
        }

        private void EnBoost_Tick(object sender, EventArgs e)
        {
            if (p.bubbles.Count == 0) return;
            Random r = new Random();
            int prey = r.Next(p.bubbles.Count);
            if (p.bubbles[prey].vx < 0) p.bubbles[prey].vx -= 2;
            else p.bubbles[prey].vx += 2;
            //p.bubbles[prey].vy = (float)(p.bubbles[prey].tangent * p.bubbles[prey].vx);
        }

        double SystemVelocity;

        private void _timer_Tick(object sender, EventArgs e)
        {
            p.Go();
            pictureBox1.Invalidate();
            label5.Text = "Untouched balls: " + p.bubbles.Count.ToString();
            label6.Text = "Exploded balls: " + (p.ballsfromthestart - p.bubbles.Count).ToString();
            label7.Text = "Exploding right now: " + p.exbubbles.Count.ToString();
            SystemVelocity = 0;
            /*foreach (Bubble a in p.bubbles)
            {
                SystemVelocity+=Math.Sqrt(a.vx * a.vx + a.vy * a.vy);
            }*/
            label8.Text = "System Velocity: " + SystemVelocity.ToString("F");
            if (p.IsOn() == false)
            {
                _timer.Enabled = false;
                p.bubbles.Clear();
                p.exbubbles.Clear();
                MessageBox.Show("You've won!");
                button1.Text = "Go!";
                weclicked = false;
            }
            if ((weclicked) && (p.exbubbles.Count == 0))
            {
                _timer.Enabled = false;
                p.bubbles.Clear();
                p.exbubbles.Clear();
                MessageBox.Show("You've lost!");
                button1.Text = "Go!";
                weclicked = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            p.rx = Convert.ToInt32(textBox1.Text);
            p.ry = Convert.ToInt32(textBox2.Text);
            pictureBox1.Height = p.ry - p.ly + 1;
            pictureBox1.Width = p.rx - p.lx + 1;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            p.ballsfromthestart = (int)numericUpDown1.Value;
            p.ballstowin = (int)((numericUpDown1.Value * 2) / 3);
            label4.Text = "So to win: " + p.ballstowin.ToString();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            p.SameSizedBall = checkBox1.Checked;
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            p.ImprobabilityDrive = checkBox2.Checked;
            improTimer.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            p.EnergyBoost = checkBox3.Checked;
            EnBoost.Enabled = checkBox3.Checked;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            p.BounceEnergy = trackBar2.Value / 100.0;
            label9.Text = "Bounce energy: " + trackBar2.Value;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _timer.Interval = 100 - trackBar1.Value;
        }


    }
}

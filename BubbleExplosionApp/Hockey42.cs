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
    public partial class Hockey42 : Form
    {
        public Hockey42()
        {
            InitializeComponent();
        }
        static Playground p = new Playground(0, 0, 536, 301);
        bool play = false;
        Timer _timer = new Timer();
        static int width=536, height=301;
        static int gateheight = 30;
        Rectangle blueGate = new Rectangle(width - 1, height / 2 - gateheight / 2, 10, gateheight);
        Rectangle redGate = new Rectangle(1, height / 2 - gateheight / 2, 10, gateheight);
        Bubble ball = new Bubble(width / 2, height / 2, 10, p, 1);
        Bubble blueTeam = new Bubble(width - 30, height / 2, 20, p, 4);
        Bubble redTeam = new Bubble(30, height / 2, 20, p, 5);

        private void Draw(Graphics g)
        {
            g.Clear(Color.White);
            g.DrawRectangle(new Pen(Brushes.Blue, 1), blueGate);
            g.DrawRectangle(new Pen(Brushes.Red, 1), redGate);

            g.FillEllipse(new SolidBrush(Color.Red), redTeam.x - redTeam.rad, redTeam.y - redTeam.rad, 2 * redTeam.rad, 2 * redTeam.rad);
            g.DrawEllipse(new Pen(Brushes.Black, 1), redTeam.x - redTeam.rad, redTeam.y - redTeam.rad, 2 * redTeam.rad, 2 * redTeam.rad);
            g.FillEllipse(new SolidBrush(Color.Black), redTeam.x - redTeam.rad / 2, redTeam.y - redTeam.rad / 2, redTeam.rad, redTeam.rad);

            g.FillEllipse(new SolidBrush(Color.Blue), blueTeam.x - blueTeam.rad, blueTeam.y - blueTeam.rad, 2 * blueTeam.rad, 2 * blueTeam.rad);
            g.DrawEllipse(new Pen(Brushes.Black, 1), blueTeam.x - blueTeam.rad, blueTeam.y - blueTeam.rad, 2 * blueTeam.rad, 2 * blueTeam.rad);
            g.FillEllipse(new SolidBrush(Color.Black), blueTeam.x - blueTeam.rad / 2, blueTeam.y - blueTeam.rad / 2, blueTeam.rad, blueTeam.rad);

            g.FillEllipse(new SolidBrush(Color.Beige), ball.x - ball.rad, ball.y - ball.rad, 2 * ball.rad, 2 * ball.rad);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!play)
            {
                width = 536;
                height = 301;
                play = true;
                _timer.Interval = 50;
                _timer.Enabled = true;
                _timer.Tick += new EventHandler(_timer_Tick);
                pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
                blueTeam.vx = 0;
                blueTeam.vy = 0;
                blueTeam.mass = 300;
                redTeam.vx = 0;
                redTeam.vy = 0;
                redTeam.mass = 300;
            }
        }

        int blueScore, redScore;

        private void _timer_Tick(object sender, EventArgs e)
        {
            p.GoNoExplosions();
            pictureBox1.Invalidate();
            if ((ball.x > blueGate.Left) && (ball.y > blueGate.Top) && (ball.y < blueGate.Bottom))
            {
                MessageBox.Show("Red Team scores!");
                blueScore++;
                Bubble ball1 = new Bubble(width / 2, height / 2, 10, p, 1);
                ball = ball1;
            }
            if ((ball.x < redGate.Right) && (ball.y > redGate.Top) && (ball.y < redGate.Bottom))
            {
                MessageBox.Show("Blue Team scores!");
                redScore++;
                Bubble ball1 = new Bubble(width / 2, height / 2, 10, p, 1);
                ball = ball1;
            }

            if (p.Intersect(ball, redTeam))
            {
                p.Collide(ball, redTeam);
            }

            if (p.Intersect(ball, blueTeam))
            {
                p.Collide(ball, blueTeam);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Breakout
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        Random rnd;
        Rectangle controller;
        Rectangle ball;
        Dictionary<int, Color> colors = new Dictionary<int, Color>();

        public class block
        {
            public int x;
            public int y;
            public Rectangle rect;
            public Color col;

            public block(int pozx, int pozy,Color culoare)
            {
                x = pozx;
                y = pozy;
                rect = new Rectangle(pozx, pozy, width, height);
                col = culoare;
            }
        }
        public block[] obs = new block[50];

        static int obs_per_row = 5, rows = 3, i, j, width = 100, height = 30, padding = 10, loaded = 0,dir=3,lives=3, score=0;

        void modifyBallPosition()
        {

            switch (dir)
            {
                case 1: //PE DIAGONALA INSPRE STANGA, IN SUS
                    ball.X -= 20;
                    ball.Y -= 20;
                    break;
                case 2: //PE DIAGONALA INSPRE DREAPTA, IN JOS
                    ball.X += 20;
                    ball.Y -= 20;
                    break;
                case 3: //DREPT IN SUS
                    ball.Y -= 20;
                    break;
                case -1: //PE DIAGONALA INSPRE STANGA, IN JOS
                    ball.X -= 20;
                    ball.Y += 20;
                    break;
                case -2: //PE DIAGONALA INSPRE DREAPTA, IN SUS
                    ball.X += 20;
                    ball.Y += 20;
                    break;
                case -3: //DREPT IN JOS
                    ball.Y += 20;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if(lives==0)
            {
                MessageBox.Show("GAME OVER!");
                Application.Exit();
            }
            if(score==obs_per_row*rows)
            {
                MessageBox.Show("U WON!");
                Application.Exit();
            }
            //verificam daca mingea a iesit din chenar prin partea de jos
            if (ball.Y>=Form1.ActiveForm.Height)
            {
                lives--;
                ball = new Rectangle(controller.X + controller.Width / 2, Form1.ActiveForm.Height - 100, 20, 20);
                dir = 3;
                label2.Text = String.Format("Lives:{0}", lives);
            }
           

            //daca mingea loveste marginea din stanga, din dreapta, sau superioara, ea ricoseaza
            if(ball.X<=10) //A LOVIT MARGINEA DIN STANGA->   stanga diagonala in  jos
                dir = 2;
            if(ball.X>=Form1.ActiveForm.Width-20) //A LOVIT MARGINEA DIN DREAPTA -> dreapta diagonala in jos
                dir = 1;
            if (ball.Y < 10) //A LOVIT MARGINEA SUPERIOARA -> drept in jos
                dir = (-1)*dir;

            //daca mingea intersecteaza un dreptunghi, el dispare si mingea ricoseaza
            for(i=1; i<=rows*obs_per_row; i++)
            {
                if(obs[i].col!=Color.Black)
                {
                    if(ball.IntersectsWith(obs[i].rect))
                    {
                        obs[i].col = Color.Black;
                        dir = (-1)*dir;
                        score++;
                        label1.Text =String.Format("Score:{0}" ,score);
                        break;
                    }
                }
            }

            //daca se intersecteaza cu controllerul, mingea ricoseaza
            if (ball.IntersectsWith(controller) && dir == -1) //face unghi 90
                dir = 2;
            else
                if (ball.IntersectsWith(controller) && dir == -2) //face unghi de 90
                dir = 1;
            else
               if (dir == -3 && ball.IntersectsWith(controller))
                dir = rnd.Next(1,3);

            modifyBallPosition();
            this.Invalidate();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 'a' || e.KeyValue == 'A')
            {
                controller.X -= 30;
            }
            if(e.KeyValue=='D' || e.KeyValue=='s')
            {
                controller.X += 30;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateColors();
            CreatePlayArea();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded == 0)
            {
                e.Graphics.Clear(Color.Black);
                Pen white = new Pen(Color.White);
                for (i = 1; i <= rows * obs_per_row; i++)
                {
                    e.Graphics.DrawRectangle(white, obs[i].rect);
                    SolidBrush brush = new SolidBrush(obs[i].col);
                    e.Graphics.FillRectangle(brush, obs[i].rect);
                    Thread.Sleep(10);
                }

                //controller
                controller = new Rectangle((obs[1].x + obs[rows * obs_per_row].x) / 2, Form1.ActiveForm.Height - 80, 100, 20);
                SolidBrush solidBrush = new SolidBrush(Color.White);
                e.Graphics.FillRectangle(solidBrush, controller);

                //ball
                ball = new Rectangle(controller.X + controller.Width / 2, Form1.ActiveForm.Height - 100, 20, 20);
                solidBrush = new SolidBrush(Color.Gray);
                e.Graphics.DrawEllipse(white, ball);
                e.Graphics.FillEllipse(solidBrush, ball);

                loaded = 1;
                timer1.Start();
                dir = 3;
            }
            else
            {
                Pen white = new Pen(Color.White);

                for (i = 1; i <= rows * obs_per_row; i++)
                {
                    if (obs[i].col != Color.Black)
                    {
                        e.Graphics.DrawRectangle(white, obs[i].rect);
                        SolidBrush brush = new SolidBrush(obs[i].col);
                        e.Graphics.FillRectangle(brush, obs[i].rect);
                    }
                }


                //controller
                SolidBrush solidBrush = new SolidBrush(Color.White);
                e.Graphics.FillRectangle(solidBrush, controller);

                //ball
                solidBrush = new SolidBrush(Color.Gray);
                e.Graphics.DrawEllipse(white, ball);
                e.Graphics.FillEllipse(solidBrush, ball);
            }
        }

        public void GenerateColors()
        {
            colors.Add(1, Color.Blue);
            colors.Add(2, Color.Pink);
            colors.Add(3, Color.Yellow);
            colors.Add(4, Color.Red);
            colors.Add(5, Color.Purple);
        }

        public void CreatePlayArea()
        {

            int xstart = 10,ystart = 10;
            int cnt = 1;
            for (i = 1; i <= rows; i++)
            {
                for (j = 1; j <= obs_per_row; j++)
                {
                    rnd = new Random();
                    int index = rnd.Next(1, 6);
                    Thread.Sleep(20);
                    block aux = new block(xstart, ystart, colors[index]);
                    obs[cnt] = aux;
                    xstart = xstart + width + padding;
                    cnt++;
                }
                ystart = ystart + height + padding;
                xstart = 10;
            }
            this.Invalidate();
        }
    }
}

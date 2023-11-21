using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rnd;
        Dictionary<int, Color> colors = new Dictionary<int, Color>();

        public class block
        {
            public int x;
            public int y;
            public Rectangle rect;

            public block(int pozx, int pozy)
            {
                x = pozx;
                y = pozy;
                rect = new Rectangle(pozx, pozy, width, height);
            }
        }
        public block[] obs = new block[50];

        static int obs_per_row = 5, rows = 3, i, j, width = 50, height = 20, xstart = 20, ystart = 20, padding = 10, loaded = 0;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded == 1)
            {
                MessageBox.Show("called");
                e.Graphics.Clear(Color.Black);
                Pen black = new Pen(Color.Black);
                for (i = 1; i <= rows * obs_per_row; i++)
                {
                    e.Graphics.DrawRectangle(black, obs[i].rect);
                    rnd = new Random();
                    int index = rnd.Next(1, 7);
                    SolidBrush brush = new SolidBrush(colors[index]);
                    e.Graphics.FillRectangle(brush, obs[i].rect);
                }
            }
        }

        public void GenerateColors()
        {
            colors.Add(1, Color.White);
            colors.Add(2, Color.Blue);
            colors.Add(3, Color.Pink);
            colors.Add(4, Color.Yellow);
            colors.Add(5, Color.Red);
            colors.Add(6, Color.Purple);
        }

        public void CreatePlayArea()
        {

            xstart = 20;
            ystart = 20;
            int cnt = 1;
            for (i = 1; i <= rows; i++)
            {
                for (j = 1; j <= obs_per_row; j++)
                {
                    block aux = new block(xstart, ystart + width + padding);
                    obs[cnt] = aux;
                    ystart = ystart + width + padding;
                    cnt++;
                }
                xstart = xstart + height + padding;
                ystart = 20;
            }
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loaded = 1;
            GenerateColors();
            CreatePlayArea();
        }
    }
}

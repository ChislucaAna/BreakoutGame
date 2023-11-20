using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int obs_per_row = 5, rows = 3, i, j , width = 50, height = 20, xstart=20, ystart=20, padding =10,loaded=0;
        Graphics g;
        Random rnd;
        Dictionary<int, Color> colors =new Dictionary<int, Color>();

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
        block[] obs = new block[50];

        public void GenerateColors()
        {
            colors.Add(1, Color.White);
            colors.Add(2, Color.Blue);
            colors.Add(3, Color.Pink);
            colors.Add(4, Color.Yellow);
            colors.Add(5, Color.Red);
            colors.Add(6, Color.Purple);
        }

        public void DrawObstacles()
        {
            Pen black = new Pen(Color.Black);
            for(i=1; i<=rows*obs_per_row; i++)
            {
                g.DrawRectangle(black, obs[i].rect);
                rnd = new Random();
                int index = rnd.Next(1, 7);
                SolidBrush brush = new SolidBrush(colors[index]);
                g.FillRectangle(brush,obs[i].rect);
            }
        }

        public void CreatePlayArea()
        {
            if(loaded==1)
                Form1.ActiveForm.Controls.Clear();

            xstart = 20;
            ystart = 20;
            for(i=1; i<=rows; i++)
            {
                for(j=1; j<=obs_per_row; j++)
                {
                    block aux = new block(xstart, ystart + width + padding);
                    obs[i +] = aux;
                    ystart = ystart + width + padding;
                }
                xstart = xstart + height + padding;
                ystart = 20;
            }
            DrawObstacles();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateColors();
            CreatePlayArea();
            loaded = 1;
        }
    }
}

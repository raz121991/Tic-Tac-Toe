using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe;

namespace MainMenu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
         
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 10,FontStyle.Bold))
            {
                e.Graphics.DrawString("Player Vs Player", myFont, Brushes.Black, new Point(4, 20));
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 10, FontStyle.Bold))
            {
                e.Graphics.DrawString("Player Vs PC", myFont, Brushes.Black, new Point(4, 20));
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void playerVsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void playerVsComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"   Tic Tac Toe desktop game   
    created by Raz in 2017","About",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}

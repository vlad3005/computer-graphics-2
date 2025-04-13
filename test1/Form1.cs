using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test1
{
    public partial class Form1: Form
    {
        View view = new View();
        Bin bin = new Bin();
        bool loaded = false;
        int currentLayer = 10;
        bool quads= false;
        bool texture = false;

        int FrameCount;
        DateTime NextFPSUUpdate = DateTime.Now.AddSeconds(1);


        public Form1()
        {
            InitializeComponent();
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 2000;
            trackBar1.Value = 1;

            trackBar2.Minimum = 1;
            trackBar2.Maximum = 4095;
            trackBar2.Value = 1;

            trackBar3.Minimum = 1;
            trackBar3.Maximum = 4095;
            trackBar3.Value = 255;
        }



        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps={0})", FrameCount);
                NextFPSUUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;

                bin.readBin(str);
                trackBar1.Maximum = Bin.Z - 1;
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
            }
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
        }


        bool needReload = false;
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                int minTF = trackBar2.Value;
                int widthTF = trackBar3.Value;
                if (quads)
                    view.DrawQuads(currentLayer, minTF, widthTF);
                if (texture)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer, minTF, widthTF);
                        view.Load2DTexture();
                        needReload = false;

                    }
                    view.DrawTexture();
                }
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
            glControl1.Invalidate();
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while(glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            quads = true;
            texture = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            texture = true;
            quads = false;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            needReload = true;
            glControl1.Invalidate();
        }

        


    }
}

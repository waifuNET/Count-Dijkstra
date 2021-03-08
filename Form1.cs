using graphWF.windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphWF
{
    public partial class Form1 : Form
    {
        public string ProjectName = "Default";
        public string saveDir = Application.StartupPath + "\\" + "Saves";
        public string pathToSave = null;

        public SelectProject selectProject = new SelectProject();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\" + "Saves"))
                Directory.CreateDirectory(Application.StartupPath + "\\" + "Saves");
            this.Enabled = false;
            GUI.Init();
            MouseWheel += Form1_MouseWheel;

            selectProject.Show();
        }

        public void LoadPath()
        {
            pathToSave = Application.StartupPath + "\\" + "Saves\\" + Program.form1.ProjectName + ".json";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void поискПутиToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GDEXControl.MathGraph(toolStripTextBox1.Text, toolStripTextBox2.Text);
            }
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GDEXControl.MathGraph(toolStripTextBox1.Text, toolStripTextBox2.Text);
            }
        }

        private void вершинуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GDEXControl.RemoveVertex();
        }

        private void весьГрафToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GDEXControl.RemoveAllVertexes();
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta > 10)
            {
                GUI.scaleOffset += 1;
            }
            else
            {
                GUI.scaleOffset -= 1;
            }

            GUI.UpdateGUI();
        }

        private void проектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            selectProject = new SelectProject();
            selectProject.Show();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLoad.Save();
        }

        private void найтиПутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GDEXControl.MathGraph(toolStripTextBox1.Text, toolStripTextBox2.Text);
        }

        private void ssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckSS();
        }

        public void CheckSS()
        {
            if (GUI.findPath)
            {
                GUI.findPath = false;
                ssToolStripMenuItem.Checked = false;
            }
            else
            {
                GUI.findPath = true;
                ssToolStripMenuItem.Checked = true;
            }
        }

        private void документацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Information inf = new Information();
            inf.Show();
        }

        private void обПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}

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

namespace graphWF.windows
{
    public partial class SelectProject : Form
    {
        public SelectProject()
        {
            InitializeComponent();
        }

        string[] saves = new string[0];
        private void SelectProject_Load(object sender, EventArgs e)
        {
            LoadFile();

            this.CenterToParent();

            this.TopMost = true;
            this.Focus();
            this.Activate();
        }

        public void LoadFile()
        {
            saves = new string[Directory.GetFiles(Program.form1.saveDir).Length];

            listBox1.Items.Clear();
            saves = Directory.GetFiles(Program.form1.saveDir);
            for (int i = 0; i < saves.Length; i++)
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(saves[i]));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Trim().Length >= 3)
            {
                Program.form1.Invoke(new Action(() =>
                {
                    GDEXControl.removeAll();
                    Program.form1.ProjectName = richTextBox1.Text.Trim();
                    Program.form1.pathToSave = Application.StartupPath + "\\" + "Saves\\" + Program.form1.ProjectName + ".json";
                    Program.form1.Enabled = true;
                }));
                this.Close();
            }
            else
            {
                MessageBox.Show("Название слишком короткое!");
            }
        }

        private void SelectProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form1.Invoke(new Action(() =>
                Program.form1.Enabled = true
            ));
            GUI.UpdateGUI();
        }

        int index = -1;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = listBox1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (index != -1)
            {
                SaveLoad.Load(saves[index]);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(index != -1)
            {
                File.Delete(saves[index]);
                LoadFile();
            }
        }
    }
}

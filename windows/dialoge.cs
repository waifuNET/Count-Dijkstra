using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphWF.windows
{
    public partial class dialoge : Form
    {
        string alph = "QWERTYUIOPLKJHGFDSAZXCVBNM";
        public string value = "DEF";
        public string Text = "NONE";
        public string default_rich = "";
        public int maxlen = 3;
        Random rand = new Random();

        public string type = "string";

        public dialoge()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.value = richTextBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.value = richTextBox1.Text;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dialoge_Load(object sender, EventArgs e)
        {
            if (default_rich == "")
            {
                default_rich = alph[rand.Next(0, alph.Length - 1)].ToString();
            }

            if (type == "int" && default_rich == "")
            {
                default_rich = rand.Next(0, 1000).ToString();
                richTextBox1.MaxLength = 16;
            }

            richTextBox1.MaxLength = maxlen;

            richTextBox1.Text = default_rich;

            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            label1.Text = Text;

            richTextBox1.SelectAll();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.value = richTextBox1.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private string lastText = "";
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (type == "int")
            {
                if (Int32.TryParse(richTextBox1.Text, out int result))
                {

                }
                else
                {
                    richTextBox1.Text = lastText;
                }
            }

            lastText = richTextBox1.Text;
            richTextBox1.Select(richTextBox1.Text.Length, 0);
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }
    }
}

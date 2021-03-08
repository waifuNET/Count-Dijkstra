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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            this.TopMost = true;
            this.Focus();
            this.Activate();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.TopMost = false;
            linkLabel1.Visible = true;
            System.Diagnostics.Process.Start("https://vk.com/hdesus");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.TopMost = false;
            linkLabel2.Visible = true;
            System.Diagnostics.Process.Start("https://github.com/waifuNET");
        }

        private void About_Activated(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
    }
}

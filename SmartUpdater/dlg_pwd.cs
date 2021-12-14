using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartUpdater
{
    public partial class dlg_pwd : Form
    {
        public dlg_pwd()
        {
            InitializeComponent();
        }

        private void dlg_pwd_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            textBox1.KeyDown += (o, args) =>
            {
                if (args.KeyCode == Keys.Enter )
                {
                    check();
                }
            };
            this.KeyDown += (o, args) =>
            {
                if (args.KeyCode == Keys.Enter )
                {
                    check();
                }
            };
        }

        public void check()
        {
            if (textBox1.Text.Equals("driver3"))
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("нет");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check();
        }
    }
}

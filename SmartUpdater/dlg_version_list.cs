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
    public partial class dlg_version_list : Form
    {
        public dlg_version_list(ProgramInfo p){
            InitializeComponent();

            listBox1.Items.AddRange(BuildInfo.DownloadAllVersionInfo(p).ToArray()); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Не выбрана ни одна версия!");
                return;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartUpdater
{
    public partial class dlg_new_prog : Form
    {
        public dlg_new_prog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e){

            ProgramInfo info = new ProgramInfo();
            info.Name = tb_name.Text.Trim();
            info.GUID= tb_uid.Text.Trim();
            info.InstallName = tb_install.Text.Trim();
            info.Path = tb_path_server.Text.Trim();
            info.AutoUpdate = cb_autoupdate.Checked;
            info.Visible = cb_visible.Checked;
            info.ExeFile = tb_exe.Text;
            info.Description = tb_description.Text;
            if (String.IsNullOrEmpty(info.Name) || String.IsNullOrEmpty(info.ExeFile) ||
                String.IsNullOrEmpty(info.InstallName) || String.IsNullOrEmpty(info.Path) ||
                String.IsNullOrEmpty(info.GUID))
            {
                MessageBox.Show("Заполнены не все поля!");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.FileName = "list.json";
            if(sfd.ShowDialog(this) != DialogResult.OK)
                return;
            List<ProgramInfo> list = new List<ProgramInfo>();
            if (cb_combine.Checked)
                list = ProgramInfo.DownloadList();
            list.Add(info);
            File.WriteAllText(sfd.FileName,Utils.toJSON(list));

            if (MessageBox.Show("Публикация завершена, открыть папку?", "Успех", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Utils.OpenDir(new FileInfo(sfd.FileName).DirectoryName);
            }
        }
    }
}

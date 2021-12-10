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
    public partial class dlg_install : Form
    {
        private ProgramInfo p;
        public dlg_install(ProgramInfo p)
        {
            InitializeComponent();
            this.p = p;
            tb_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + p.InstallName + "\\";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rb_all_CheckedChanged(object sender, EventArgs e)
        {

            tb_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + p.InstallName +"\\";
        }

        private void rb_current_CheckedChanged(object sender, EventArgs e){
            tb_path.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Programs\\" + p.InstallName + "\\";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.ProgramFiles;
            try
            {
                fbd.SelectedPath = tb_path.Text;
            }
            catch (Exception){
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb_path.Text = fbd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e){
            var _this = this;

            bool isOnlyCurrentUser = rb_current.Checked;
            bool createShortcutDesktop = cb_desk.Checked;
            var b = BuildInfo.DownloadActualVersionInfo(p);
            if (b != null)
                p.InstallProgramAsync(tb_path.Text,b, b1 =>{
                    if (b1){
                        string s = "";
                        string installPath = Utils.ConvertDirectory(tb_path.Text, false, true);
                        string uninstallPath = installPath  + "uninstall.exe";
                        string pathToExe = installPath + p.ExeFile;
                        string pathToIcon= installPath + p.IconPath;

                        if (!Utils.AddShortcut(installPath,uninstallPath,pathToExe,pathToIcon,p.Name,p.Description,p.getCompany(), true, createShortcutDesktop, isOnlyCurrentUser))
                            s += "Не удалось создать ярлыки.";
                        if(!Utils.AddUninstaller(installPath,uninstallPath,p.GUID,p.Name,b.Version,p.getCompany(),isOnlyCurrentUser))
                            s += "Не удалось добавить удаление программы";
                        if(!Utils.AddOrUpdateInstallInfo(p.InstallName,p.Name,b.Version,p.getCompany(),installPath,pathToExe, isOnlyCurrentUser))
                            s += "Не удалось добавить информацию о программе в реестр";

                        if(string.IsNullOrEmpty(s))
                            MessageBox.Show("Программа установлена!", "Успех", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Программа установлена установлена с ошибками.\n"+s, "Установка завершена", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        _this.DialogResult = DialogResult.OK;
                        _this.Close();
                    }
                });
        }
    }
}

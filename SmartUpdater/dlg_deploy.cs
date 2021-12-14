using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using SmartUpdater.Properties;

namespace SmartUpdater
{
    public partial class dlg_deploy : Form{

        List<FileDataInfo> files = new List<FileDataInfo>();
        List<ProgramInfo> programs = new List<ProgramInfo>(); 
        public dlg_deploy(){
            InitializeComponent();
            programs = ProgramInfo.DownloadList();
            comboBox1.Items.AddRange(programs.ToArray());
            gb_file_config.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e){
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (!String.IsNullOrEmpty(Settings.Default.last_dir_prog))
                fbd.SelectedPath= Settings.Default.last_dir_prog;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                tb_dir_build.Text = "";
                tb_path.Text = fbd.SelectedPath;
                files.Clear();
                treeView1.Nodes.Clear();
                fillFilesList(new DirectoryInfo(tb_path.Text));
                lbl_full_size.Text = Utils.SizeSuffix(files.Sum(info => info.Size));
                Settings.Default.last_dir_prog = tb_path.Text;
                Settings.Default.Save();
            }
        }

        public void fillFilesList(DirectoryInfo di,string root = "", TreeNode node = null){
            foreach (var fileInfo in di.GetFiles()){
                FileDataInfo fi = new FileDataInfo();
                fi.Filepath = root + fileInfo.Name;
                fi.Size= (int) fileInfo.Length;
                fi.Ignore = false;
                fi.Hash = Utils.getHashFile(fileInfo.FullName);
                files.Add(fi);
                if (programs.Any(info => Utils.ConvertDirectory(info.ExeFile,false,false) == Utils.ConvertDirectory(fileInfo.Name,false,false))) {
                    comboBox1.SelectedItem = null;
                    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(programs.FirstOrDefault(info => Utils.ConvertDirectory(info.ExeFile, false, false) == Utils.ConvertDirectory(fileInfo.Name, false, false)));
                }
                if (node == null)
                    treeView1.Nodes.Add(new TreeNode(fileInfo.Name) {Tag = fi});
                else
                    node.Nodes.Add(new TreeNode(fileInfo.Name) {Tag = fi});
            }
            foreach (var directoryInfo in di.GetDirectories()){
                files.Add(new FileDataInfo() { IsDirectory = true, Filepath = root + directoryInfo.Name });
                if (node == null)
                {
                    var n1 = new TreeNode(directoryInfo.Name);
                    treeView1.Nodes.Add(n1);
                    fillFilesList(directoryInfo,root+directoryInfo.Name+"/",n1);
                }
                else
                {
                    var n1 = new TreeNode(directoryInfo.Name);
                    node.Nodes.Add(n1);
                    fillFilesList(directoryInfo, root + directoryInfo.Name + "/", n1);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is FileDataInfo)
            {
                gb_file_config.Visible = true;
                tb_hash.Text = (treeView1.SelectedNode.Tag as FileDataInfo).Hash;
                lbl_size.Text = Utils.SizeSuffix((treeView1.SelectedNode.Tag as FileDataInfo).Size);
                cb_ignore.Checked = (treeView1.SelectedNode.Tag as FileDataInfo).Ignore;
                lbl_full_path.Text= (treeView1.SelectedNode.Tag as FileDataInfo).Filepath;
            }
            else
            {
                gb_file_config.Visible = false;
                tb_hash.Text = "";
                lbl_size.Text = "-";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null){
                if (files.Count > 0){
                    
                    check();
                }
            }
        }


        public bool check(){
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите программму", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }
            var sel = Utils.ConvertRoute((comboBox1.SelectedItem as ProgramInfo).ExeFile,false,false);
            var f = files.FirstOrDefault(info => info.Filepath.EndsWith(sel));
            if (f == null)
            {
                MessageBox.Show("Не найден основной файл программы " + sel, "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Utils.ConvertDirectory(tb_path.Text, false, true) +
                                                                 Utils.ConvertDirectory(f.Filepath, false, false));
            tb_ver.Text = fvi.FileVersion;
            if (tb_dir_build.Text.Trim() == "")
                tb_dir_build.Text = fvi.FileVersion;
            return true;
        }
        private void button3_Click(object sender, EventArgs e)
        {


        }

        private void cb_ignore_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is FileDataInfo){
                (treeView1.SelectedNode.Tag as FileDataInfo).Ignore = cb_ignore.Checked;
            }
        }

        private void button3_Click_1(object sender, EventArgs e){

            FolderBrowserDialog fbd = new FolderBrowserDialog();


            if (!String.IsNullOrEmpty(Settings.Default.last_dir_build))
                fbd.SelectedPath = Settings.Default.last_dir_build;

            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;

            Settings.Default.last_dir_build= fbd.SelectedPath;
            Settings.Default.Save();

            if (!check()){
                MessageBox.Show("Публикация отменена!", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            var p = comboBox1.SelectedItem as ProgramInfo;
            BuildInfo build = new BuildInfo();
            build.GUID = p.GUID;
            build.Version = tb_ver.Text;
            build.ChangeList = tb_changelist.Text.Split(new string[] {"\n"},StringSplitOptions.RemoveEmptyEntries).ToList();
            build.ClearAfterInstall = cb_clearall.Checked;
            build.UpdateOnlyChanges= cb_onlychange.Checked;
            build.NeedMinVersionSmartUpdateForUpdate = tb_min_upd.Text.Trim();
            build.ServerPath = tb_dir_build.Text.Trim();
            build.UpdateRequired = cb_req.Checked;
            build.Files = files;
            build.Name = p.Name + " обновление до " + tb_ver.Text;

            DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
            File.WriteAllText(di.FullName+"\\current.json",Utils.toJSON(build));
            List<BuildInfo> allBuilds = null;
            if (File.Exists(di.FullName + "\\all.json"))
                try {
                    allBuilds = Utils.toObject<List<BuildInfo>>(File.ReadAllText(di.FullName + "\\all.json"));
                }
                catch (Exception exception)
                {
                }
            if(allBuilds == null)
                allBuilds = new List<BuildInfo>();
            allBuilds.Add(build);
            File.WriteAllText(di.FullName + "/all.json", Utils.toJSON(allBuilds));

            var d = Directory.CreateDirectory(di.FullName+"/"+build.ServerPath);
            Utils.Copy(tb_path.Text,di.FullName+"/"+build.ServerPath);
            if (MessageBox.Show("Публикация завершена, открыть папку?", "Успех", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Utils.OpenDir(di.FullName);
            }
        }


    }
}

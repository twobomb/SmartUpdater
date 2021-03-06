using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using SmartUpdater.Properties;

namespace SmartUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dev_menu.Visible = false;

        }

        public void updateListPrograms(bool showHidden = false)
        {
            var list = ProgramInfo.DownloadList();
            listBox1.Items.Clear();
            foreach (var programInfo in list)
                if (!programInfo.Visible && !programInfo.isInstalled() && !showHidden)
                    continue;
                else
                    listBox1.Items.Add(programInfo);
        }


        public void setVisible(bool val){
            btn_delete.Enabled= val;
            btn_install.Enabled = val;
            btn_start.Enabled = val;
            btn_update.Enabled = val;
            btn_check_files.Visible = val;
            btn_restore.Visible = val;
            btn_open_dir.Visible = val;
            btn_select_version.Visible = val;
            lbl_cur_ver.Visible = val;
            lbl_installed.Visible = val;
            lbl_name.Visible = val;
            tb_path.Visible = val;
            lbl_update_info.Visible = val;
        }
        private void Form1_Load(object sender, EventArgs e){
            setVisible(false);
            updateListPrograms();
            
        }

        private void подключениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlg_settings d = new dlg_settings();
            d.ShowDialog(this);
            updateListPrograms();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setVisible(false);
            if (listBox1.SelectedItem == null)
                return;
            var p = listBox1.SelectedItem as ProgramInfo;
            lbl_name.Text = p.Name;
            lbl_name.Visible = true;
            if (p.isInstalled()){
                btn_start.Enabled = true;
                btn_delete.Enabled = true;
                lbl_installed.Visible = true;
                tb_path.Text = p.getInstallPath();
                tb_path.Visible = true;
                lbl_cur_ver.Visible = true;
                lbl_cur_ver.Text = "Текущая версия программы v" + p.installedVersion();
                btn_check_files.Visible = true;
                btn_restore.Visible = true;
                btn_open_dir.Visible = true;
                btn_select_version.Visible = true;
                lbl_update_info.Visible = true;
                if (p.canUpdate())
                {
                    btn_update.Enabled = true;
                    lbl_update_info.Text = "Доступна новая версия, нажмите обновить!";
                    lbl_update_info.ForeColor = Color.DarkRed;
                }
                else
                {
                    lbl_update_info.Text = "Установлена актуальная версия программы!";
                    lbl_update_info.ForeColor = Color.LimeGreen;
                }
            }
            else
                btn_install.Enabled = true;

        }

        private void обновитьСписокПрограммToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateListPrograms();
        }

        private void показатьВсеПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateListPrograms(true);
        }

        private void публикацияНовойВерсииНаСерверToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlg_deploy dd = new dlg_deploy();
            dd.ShowDialog(this);

        }

        private void созданиеКонфигаПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlg_new_prog dnp = new dlg_new_prog();
            dnp.ShowDialog(this);
        }

        private void btn_install_Click(object sender, EventArgs e){
            if (listBox1.SelectedItem == null)
                return;
            var p = listBox1.SelectedItem as ProgramInfo;

            dlg_install install = new dlg_install(p);
            if (install.ShowDialog(this) == DialogResult.OK){
                var inx = listBox1.SelectedIndex;
                listBox1.SelectedItem = null;
                listBox1.SelectedIndex = inx;
            }
        }

        private void btn_open_dir_Click(object sender, EventArgs e)
        {
            Utils.OpenDir(tb_path.Text);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {

            try
            {
                Process.Start((listBox1.SelectedItem as ProgramInfo).getInstallPath(true));

            }
            catch (Exception ex)
            {
                        MessageBox.Show("Ошибка запуска\n"+ex.Message , "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs ev){
            if (listBox1.SelectedItem != null){
                var p  = (listBox1.SelectedItem as ProgramInfo);
                if(MessageBox.Show("Вы уверены что хотите удалить "+p.Name,"Удалить?",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                if (!File.Exists(p.getInstallPath(false) + "uninstall.exe"))
                {
                    MessageBox.Show("Не найден uninstall.exe! Попробуйте сделать востановить целостность файлов и повторить попытку!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments= "skipconfirm";
                psi.FileName = p.getInstallPath(false) + "uninstall.exe";
                var process = Process.Start(psi);
                bool isDelete = false;
                process.WaitForExit(10000);
                
                var inx = listBox1.SelectedIndex;
                listBox1.SelectedItem = null;
                listBox1.SelectedIndex = inx;

                MessageBox.Show("Удаление завершено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_update_Click(object sender, EventArgs e){
            var p = (listBox1.SelectedItem as ProgramInfo);
            
            dlg_loader loader = new dlg_loader();
            CancellationTokenSource source = new CancellationTokenSource();
            var token = source.Token;
            loader.button1.Click += (o, args) =>{
                source.Cancel(false);
            };
            loader.Shown += (o, args) =>{
                var build = BuildInfo.DownloadActualVersionInfo(p);
                p.UpdateTo( build, (b,error) =>{

                    loader.Invoke(new Action(() =>
                    {
                        loader.Close();

                        var inx = listBox1.SelectedIndex;
                        listBox1.SelectedItem = null;
                        listBox1.SelectedIndex = inx;
                    }));
                    

                    if(b)
                        MessageBox.Show(
                            "Обновление завершено!",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            "Обновление завершено неудачей.\n"+error,
                            "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                },
                    (down, all) =>{

                        loader.BeginInvoke(new Action(() =>{
                            loader.progressBar1.Value = (int)(((double)down / (double)all) * 100);
                            loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                        }));
                    }, token);
            };
            loader.ShowDialog(this);
        }



        private void btn_check_files_Click(object sender, EventArgs e){
            var p = (listBox1.SelectedItem as ProgramInfo);
            if(!p.isInstalled())
                return;
            var versions = BuildInfo.DownloadAllVersionInfo(p);
            var build = versions.FirstOrDefault(info => info.Version == p.installedVersion());
            if (build == null){
                MessageBox.Show(
                    "Неудалось найти информацию об установленной версии! Попробуйте установить другую версию!",
                    "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var f = Utils.getDifferenceFiles(p, build);
            
            if(!File.Exists(p.getInstallPath(false)+"uninstall.exe"))
                f.Add(new FileDataInfo()
                {
                    Filepath = "uninstall.exe",
                    IsDirectory = false
                });
            if (!File.Exists(p.getInstallPath(false) + "uninstall.dat"))
                f.Add(new FileDataInfo()
                {
                    Filepath = "uninstall.dat",
                    IsDirectory = false
                });
            if (!File.Exists(p.getInstallPath(false) + "launcher.dat"))
                f.Add(new FileDataInfo()
                {
                    Filepath = "launcher.dat",
                    IsDirectory = false
                });
            if (!File.Exists(p.getInstallPath(false) + "launcher.exe"))
                f.Add(new FileDataInfo()
                {
                    Filepath = "launcher.exe",
                    IsDirectory = false
                });

            if (f.Count == 0)
            {
                MessageBox.Show(
                    "Все файлы проверены, ошибок не найдено!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else{
                MessageBox.Show(
                    "Найдены не соответствия установленной версии в следующих файлах:\n" + string.Join("\n", f.Select(info => (info.IsDirectory ? "[ПАПКА]" : "[ФАЙЛ]") + info.Filepath)) + "\nРекомендуем восстановить целостность файлов!",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
        }

        private void btn_restore_Click(object sender, EventArgs e){
            var p = (listBox1.SelectedItem as ProgramInfo);
            if (!p.isInstalled())
                return;
            var versions = BuildInfo.DownloadAllVersionInfo(p);
            var build = versions.FirstOrDefault(info => info.Version == p.installedVersion());
            if (build == null)
            {
                MessageBox.Show(
                    "Неудалось найти информацию об установленной версии! Попробуйте установить другую версию!",
                    "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            var f = Utils.getDifferenceFiles(p, build);
            var fAdv = new List<string>();

            if (!File.Exists(p.getInstallPath(false) + "uninstall.exe"))
                fAdv.Add("uninstall.exe");
            if (!File.Exists(p.getInstallPath(false) + "uninstall.dat"))
                fAdv.Add("uninstall.dat");
            if (!File.Exists(p.getInstallPath(false) + "launcher.dat"))
                fAdv.Add("launch.dat");
            if (!File.Exists(p.getInstallPath(false) + "launcher.exe"))
                fAdv.Add("launch.exe");

            if (f.Count == 0 && fAdv.Count == 0){
                MessageBox.Show(
                    "Нет необходимости восстановления! Все файлы проверены, ошибок не найдено!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else{

                dlg_loader loader = new dlg_loader();
                CancellationTokenSource source = new CancellationTokenSource();
                var token = source.Token;
                loader.button1.Click += (o, args) =>{
                    source.Cancel(false);
                };

                loader.Shown += (o, args) =>
                {
                    p.UpdateTo( build, (b,error) =>
                    {
                        loader.Invoke(new Action(() =>
                        {
                            loader.Close();
                        }));
                        if (b)
                            MessageBox.Show(
                            "Восстановление завершено!",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show(
                            "Неудалось восстановить файлы!\n"+error,
                            "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    },
                        (down, all) =>
                        {

                            loader.BeginInvoke(new Action(() =>
                            {
                                loader.progressBar1.Value = (int)(((double)down / (double)all) * 100);
                                loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                            }));
                        },token);
                };
                loader.ShowDialog(this);

            }
        }

        private void btn_select_version_Click(object sender, EventArgs e){
            var p = (listBox1.SelectedItem as ProgramInfo);
            if (!p.isInstalled())
                return;
            dlg_version_list dvl = new dlg_version_list(p);
            if (dvl.ShowDialog() == DialogResult.OK){
            var build = dvl.listBox1.SelectedItem as BuildInfo;
                ;
            if (build == null){
                MessageBox.Show(
                    "Неудалось найти информацию об установленной версии! Попробуйте установить другую версию!",
                    "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var f = Utils.getDifferenceFiles(p, build);
            if (f.Count == 0){
                MessageBox.Show(
                    "Выбранная версия соответствует текущей!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else{
                dlg_loader loader = new dlg_loader();
                CancellationTokenSource source = new CancellationTokenSource();
                var token = source.Token;
                loader.button1.Click += (o, args) =>{
                    source.Cancel(false);
                };
                loader.Shown += (o, args) =>{
                    p.UpdateTo( build, (b,error) =>
                    {
                        loader.Invoke(new Action(() =>
                        {
                            loader.Close();
                            
                                var inx = listBox1.SelectedIndex;
                                listBox1.SelectedItem = null;
                                listBox1.SelectedIndex = inx;
                        }));
                        if (b)
                            MessageBox.Show(
                            "Обновление завершено!",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show(
                            "Неудалось обновить файлы!\n"+error,
                            "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    },
                        (down, all) =>
                        {

                            loader.BeginInvoke(new Action(() =>
                            {
                                loader.progressBar1.Value = (int)(((double)down / (double)all) * 100);
                                loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                            }));
                        },token);
                };
                loader.ShowDialog(this);
            }
        }

    }

        private void включитьМенюРазработчикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlg_pwd pwd = new dlg_pwd();
            if (pwd.ShowDialog(this) == DialogResult.OK)
                dev_menu.Visible = true;
        }
}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using SmartUpdater.Properties;

namespace SmartUpdater
{
    public class ProgramInfo{

        public string Name { get; set; }
        public string GUID{ get; set; } // Уникальный идентификатор программы
        public bool Visible { get; set; }//Показывать в списке, если установлена в любом случае показывать
        public string Path{ get; set; } // путь на сервере к папке с билдами
        public bool AutoUpdate { get; set; } // необходимо ли автообновлять программу, даже скрытые
        public string ExeFile { get; set; } //путь файла входа в программу , обычно к Exe
        public string InstallName { get; set; } // название папки для установки
        public string IconPath { get; set; } // путь к иконке
        public string Description { get; set; } //описание

        public override string ToString()
        {
            return Name;
        }

        public string getInstallPath(bool withExe = false){
            string path = "";
            try{
                var v1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID);
                path = v1.GetValue("InstallLocation").ToString();
            }
            catch (Exception e){}
            try{
                var v1 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID);
                path = v1.GetValue("InstallLocation").ToString();
            }
            catch (Exception e){}

            try{
                var v1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + InstallName);
                path = v1.GetValue("InstallLocation").ToString();
            }
            catch (Exception e){}
            try{
                var v1 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + InstallName);
                path = v1.GetValue("InstallLocation").ToString();
            }
            catch (Exception e){}

            if (!string.IsNullOrEmpty(path))
                return Utils.ConvertDirectory(path, false, true) + (withExe ? ExeFile : "");

            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + InstallName + "\\" + (withExe ? ExeFile : "");
        }
        public bool isInstalled(){
            try{
                return File.Exists(getInstallPath(true));
            }
            catch (Exception e){
                Utils.pushCrashLog(e);
            }
            return false;
        }

        public bool canUpdate(){
            if (!isInstalled())
                return false;
            var build = BuildInfo.DownloadActualVersionInfo(this);
            if (build != null)
                return Utils.compareVersion(installedVersion(), build.Version) == 1;
            return false;
        }
        public string installedVersion(){
            if (isInstalled())
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(getInstallPath(true));
                return fvi.FileVersion;
            }
            return "0";
        }
        public string getUninstallPath(){
            return getInstallPath(false) + "Uninstall.exe";
        }
        public string getCompany(){
            if (isInstalled()){
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(getInstallPath(true));
                if (string.IsNullOrEmpty(fvi.CompanyName.Trim()))
                    return null;
                return fvi.CompanyName;
            }
            return null;
        }
        public static List<ProgramInfo> DownloadList(){
            try{
                HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(Settings.Default.host + @"/smartupdater/list.json");
                if (client.GetResponse().ContentType == "application/json"){
                    using (StreamReader sr = new StreamReader(client.GetResponse().GetResponseStream())){
                        string res = sr.ReadToEnd();
                        sr.Close();
                        return Utils.toObject<List<ProgramInfo>>(res);
                    }
                }
                else
                    throw new Exception("Неизвестный типа файла на сервере. Ожидается application/json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка при получении списка программ!\n "+ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new List<ProgramInfo>();
        }

        public bool InstallProgramAsync(string installPath,BuildInfo build, Action<bool> complete)
        {
            installPath = Utils.ConvertDirectory(installPath, false, true);
            if (isInstalled()) return false;
            dlg_loader loader = new dlg_loader();
            CancellationTokenSource source = new CancellationTokenSource();
            var token = source.Token;
            loader.button1.Click += (o, args) =>{
                source.Cancel(false);
            };

            loader.Shown += (sender, args) =>{
                Utils.DownloadBuildAsync(this, build, installPath, (b, error) =>
                {
                    loader.Invoke(new Action(() =>
                    {
                        if (!b)
                        {
                            MessageBox.Show("Во время скачивания произошла ошибка. Установка прервана\n" + error, "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        loader.Close();
                        complete(b);
                    }));
                }, (down, all) =>
                {
                    loader.BeginInvoke(new Action(() =>
                    {
                        loader.progressBar1.Value = (int) (((double) down/(double) all)*100);
                        loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                    }));
                },token);
            };
            loader.ShowDialog(Form1.ActiveForm);

            return false;
        }

    }
}

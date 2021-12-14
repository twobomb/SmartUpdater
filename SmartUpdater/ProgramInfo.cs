using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
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
        public bool AddAutoStartSystem { get; set; } //добавить в автозагрузку
        public bool SuggestAddAutoStartSystem { get; set; } //предлагать при установке добавить в автозагрузку

        public override string ToString()
        {
            return Name;
        }


        public string getProcessName()
        {
            var m = Regex.Match(ExeFile, @"([^/\\]+)\.exe\s*$");
            return m.Groups[1].Value;
        }
        public string getInstallPath(bool withExe = false){
            string path = "";
            path = Utils.tryGetRegisterValue(Registry.CurrentUser,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "InstallLocation");
            if (string.IsNullOrEmpty(path))
                path = Utils.tryGetRegisterValue(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "InstallLocation");
            if (string.IsNullOrEmpty(path))
                path = Utils.tryGetRegisterValue(Registry.CurrentUser, @"SOFTWARE\" + InstallName, "InstallLocation");
            if (string.IsNullOrEmpty(path))
                path = Utils.tryGetRegisterValue(Registry.LocalMachine, @"SOFTWARE\" + InstallName, "InstallLocation");
                
            if (!string.IsNullOrEmpty(path))
                return Utils.ConvertDirectory(path, false, true) + (withExe ? Utils.ConvertDirectory(ExeFile,false,false) : "");

            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + InstallName + "\\" + (withExe ? Utils.ConvertDirectory(ExeFile,false,false) : "");
        }


        public bool IsInstallForCurrentUser() {
            if (!string.IsNullOrEmpty(Utils.tryGetRegisterValue(Registry.CurrentUser,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion")))
                return true;
            if (!string.IsNullOrEmpty(Utils.tryGetRegisterValue(Registry.LocalMachine,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion")))
                return false;
            if (!string.IsNullOrEmpty(Utils.tryGetRegisterValue(Registry.CurrentUser, @"SOFTWARE\" + InstallName,
                "Version")))
                return true;
            if (!string.IsNullOrEmpty(Utils.tryGetRegisterValue(Registry.LocalMachine, @"SOFTWARE\" + InstallName,
                "Version")))
                return false;

            return true;
        }
        public bool isInstalled(){
            try {
                if (File.Exists(getInstallPath(true)))
                    return true;
                string version = null;
                version = Utils.tryGetRegisterValue(Registry.CurrentUser,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion");
                if (string.IsNullOrEmpty(version))
                    version = Utils.tryGetRegisterValue(Registry.LocalMachine,
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion");
                if (string.IsNullOrEmpty(version))
                    version = Utils.tryGetRegisterValue(Registry.CurrentUser, @"SOFTWARE\" + InstallName, "Version");
                if (string.IsNullOrEmpty(version))
                    version = Utils.tryGetRegisterValue(Registry.LocalMachine, @"SOFTWARE\" + InstallName, "Version");
                if (!string.IsNullOrEmpty(version)) {
                    var build = BuildInfo.DownloadBuildByVersion(this,version);
                    if (build != null)
                        return true;
                }
            }
            catch (Exception e){
                Utils.pushCrashLog(e);
            }
            return false;
        }

        public void KillProcesses(){
            var processes = Process.GetProcesses();
            foreach (var process1 in processes){//Убить открытые процессы
                try
                {
                    if (process1.MainModule != null)
                    {
                        var filePath = new FileInfo(process1.MainModule.FileName);
                        FileInfo inst = new FileInfo(getInstallPath(true));
                        if (filePath.FullName == inst.FullName)
                        {
                            process1.Kill();
                            process1.WaitForExit(5000);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        public void UpdateTo(BuildInfo build, Action<bool, string> complete, Action<int, int> process, CancellationToken cancelToken) {
            ProgramInfo p = this;
            if (!p.isInstalled())
            {
                complete(false, "Программа не установлена!");
                return;
            }
            p.KillProcesses();

            List<FileDataInfo> filesToUpdate = new List<FileDataInfo>();
            if (build.UpdateOnlyChanges && !build.ClearAfterInstall)
                filesToUpdate = Utils.getDifferenceFiles(p, build);
            else
                filesToUpdate = build.Files;

            if (cancelToken.IsCancellationRequested)
            {
                complete(false, "Процесс обновления отменен пользователем");
                return;
            }
            var temp = Utils.getEmptyTempDir();
            Utils.DownloadFilesAsync(build.GetServerRootPath(p), filesToUpdate, temp, (b, s) => {
                if (!b)
                    complete(false, s);
                else
                {
                    if (build.ClearAfterInstall)
                    {
                        try
                        {
                            var path = getInstallPath(false);
                            if (File.Exists(path + "uninstall.dat"))
                            {
                                UninstallInfo info =
                                    JsonConvert.DeserializeObject<UninstallInfo>(
                                        File.ReadAllText(getInstallPath(false) + "uninstall.dat"));
                                foreach (var infoFile in info.files)
                                {
                                    try
                                    {
                                        if (File.Exists(path + infoFile))
                                            File.Delete(path + infoFile);
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                                foreach (var infoDir in info.dirs)
                                {
                                    try
                                    {
                                        if (Utils.CountFilesInDirectoryRecursive(path + infoDir) == 0)
                                            Directory.Delete(path + infoDir, true);
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                            }
                        }
                        catch (Exception eex){
                            Utils.pushCrashLog(eex);
                        }

                    }
                    try
                    {
                        Utils.Copy(temp, p.getInstallPath(false));
                        Directory.Delete(temp, true);
                    }
                    catch (Exception ex)
                    {
                        Utils.pushCrashLog(ex);
                        complete(false, ex.Message);
                        return;
                    }

                    try
                    {
                        string installPath = Utils.ConvertDirectory(p.getInstallPath(false), false, true);
                        string uninstallPath = installPath + "uninstall.exe";
                        string pathToExe = installPath + Utils.ConvertDirectory(p.ExeFile, false, false);
                        string pathToIcon = installPath + Utils.ConvertDirectory(p.IconPath, false, false);
                        bool isOnlyCurrentUser = p.IsInstallForCurrentUser();
                        Utils.AddOrUpdateExeUninstallerInfo(p, build);
                        Utils.AddOrUpdateExeLauncherInfo(p);
                        Utils.AddUninstaller(installPath, uninstallPath, p.GUID, p.Name, build.Version, p.getCompany(),
                            isOnlyCurrentUser);
                        Utils.AddOrUpdateInstallInfo(p.InstallName, p.Name, build.Version, p.getCompany(), installPath,
                            pathToExe, isOnlyCurrentUser);
                    }
                    catch (Exception e)
                    {
                    }
                    complete(true, "Успех");
                }
            }, process, cancelToken);
        }

        public bool canUpdate(){//есть новая версия?
            if (!isInstalled())
                return false;
            var build = BuildInfo.DownloadActualVersionInfo(this);
            if (build != null)
                return Utils.compareVersion(installedVersion(), build.Version) == 1;
            return false;
        }
        public string installedVersion(){
            if (isInstalled()) {
                if (File.Exists(getInstallPath(true))) {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(getInstallPath(true));
                    return fvi.FileVersion;
                }
                string version = null;
                version = Utils.tryGetRegisterValue(Registry.CurrentUser,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion");
                if (string.IsNullOrEmpty(version))
                    version = Utils.tryGetRegisterValue(Registry.LocalMachine,
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion");
                if (string.IsNullOrEmpty(version))
                    version = Utils.tryGetRegisterValue(Registry.CurrentUser, @"SOFTWARE\" + InstallName, "Version");
                if (string.IsNullOrEmpty(version))
                    version = Utils.tryGetRegisterValue(Registry.LocalMachine, @"SOFTWARE\" + InstallName, "Version");
                if (!string.IsNullOrEmpty(version))
                    return version;
            }
            return "";
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

        public static ProgramInfo DownloadByGuid(string guid) {
            var list = DownloadList();
            foreach (var programInfo in list)
                if (programInfo.GUID.Equals(guid))
                    return programInfo;
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

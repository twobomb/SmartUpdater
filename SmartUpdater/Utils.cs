using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using Newtonsoft.Json;
using SmartUpdater.Properties;
using File = System.IO.File;

namespace SmartUpdater
{
    public static class Utils{

        public static string toJSON(object o){
            return JsonConvert.SerializeObject(o,Formatting.Indented); 
        }

        public static void OpenDir(string dir)
        {
            try{
                Process.Start("explorer.exe", dir);

            }
            catch (Exception)
            {
                
            }
        }


        public static void DownloadBuildAsync(ProgramInfo p, BuildInfo b, string dir, Action<bool,string> finished, Action<int, int> process, CancellationToken cancelToken){
            DownloadFilesAsync(b.GetServerRootPath(p), b.Files, dir, finished, process,cancelToken);
        }
        public static void DownloadFilesAsync(string buildRootPath, List<FileDataInfo> files ,string dir, Action<bool,string> finished, Action<int,int> process, CancellationToken cancelToken){
            string dirCopy = "";
            try{
                dirCopy = Directory.CreateDirectory(dir).FullName;
                foreach (var d in files.Where(info => info.IsDirectory))
                    Directory.CreateDirectory(dirCopy + Utils.ConvertDirectory(d.Filepath, true, false));
            }
            catch (Exception e){
                Utils.pushCrashLog(e);
                finished(false, e.Message);
                return;
            }

            if (cancelToken.IsCancellationRequested){
                finished(false, "Отменено пользователем");
                return;
            }
            Task.Factory.StartNew(() =>{
                try{
                    var fullsize = files.Sum(info => info.Size);
                    int downsize = 0;
                    Stopwatch sw = Stopwatch.StartNew();
                    foreach (var d in files.Where(info => !info.IsDirectory)){
                        WebClient client = new WebClient();
                        byte[] buffer = new byte[65536];//буффер на 64кб
                        int len = 0;
                        using (var fs = new BufferedStream(client.OpenRead(Utils.ConvertRoute(buildRootPath,false,false) + Utils.ConvertRoute(d.Filepath, true, false)), buffer.Length))
                        {
                            var filepath = dirCopy + Utils.ConvertDirectory(d.Filepath, true, false);
                            new FileInfo(filepath).Directory.Create();
                            using (var saveFile = File.Create(filepath, buffer.Length))
                            {
                                while (fs.CanRead)
                                {
                                    len = fs.Read(buffer, 0, buffer.Length);
                                    downsize += len;
                                    if (sw.ElapsedMilliseconds >=500){
                                        if (cancelToken.IsCancellationRequested)
                                        {
                                            finished(false, "Отменено пользователем");
                                            return;
                                        }
                                        sw.Restart();
                                        process(downsize, fullsize);
                                    }
                                    if (len == 0){
                                        saveFile.Close();
                                        fs.Close();
                                        break;
                                    }
                                    saveFile.Write(buffer, 0, len);
                                }
                            }
                        }
                    }
                    sw.Stop();
                    finished(true,"Успех");
                }
                catch (Exception e){
                    Utils.pushCrashLog(e);
                    finished(false,e.Message);
                }
            });
        }

        public static string ConvertRoute(string route, bool beginSlash = true, bool endSlash = true)
        {
            route = route.Trim();
            route = route.Replace(@"\","/");
            route = Regex.Replace(route, "^/","");
            route = Regex.Replace(route, "/$","");
            if (beginSlash)
                route = "/" + route;
            if (endSlash)
                route = route + "/";
            return route;
        }
        public static string ConvertDirectory(string route, bool beginSlash = true, bool endSlash = true)
        {
            if (route == null)
                return "";
            route = route.Trim();
            route = route.Replace( "/",@"\");
            route = Regex.Replace(route, @"^\\","");
            route = Regex.Replace(route, @"\\$","");
            if (beginSlash)
                route = @"\" + route;
            if (endSlash)
                route = route + @"\";
            return route;
        }
        public static void Copy(string sourceDir, string targetDir, bool replace = true){
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), replace);
            foreach (var directory in Directory.GetDirectories(sourceDir))
                Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
        public static T toObject<T>(string json){
            return JsonConvert.DeserializeObject<T>(json); 
        }
        static readonly string[] SizeSuffixes = { "Байт", "КБайт", "МБайт", "ГБайт", "TB", "PB", "EB", "ZB", "YB" };
        public static string SizeSuffix(int value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            int mag = (int)Math.Log(value, 1024);

            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
        public static string getHashFile(string filepath){
            using (var md5 = MD5.Create())
            {
                using (var stream = new BufferedStream(File.OpenRead(filepath), 1200000))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string getEmptyTempDir()
        {
            var temp = Path.GetTempPath() + "\\";
            string dirname = "";
            do{
                dirname = "temp_smart_updater_" + DateTime.Now.Millisecond;

            } while (Directory.Exists(temp + dirname));
            temp += dirname;
            Directory.CreateDirectory(temp);
            return temp;
        }
        /*public static void ObjectToFile(Object obj,string path) {
            if (obj == null)
               return;
            
            BinaryFormatter bf = new BinaryFormatter();
            bf.AssemblyFormat = FormatterAssemblyStyle.Simple;
            using (var fs = File.Create(path)) {
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }

        public static Object FileToObject(string path) {
            if (!File.Exists(path))
                return null;

            var fs= File.OpenRead(path);
            BinaryFormatter binForm = new BinaryFormatter();
            binForm.AssemblyFormat = FormatterAssemblyStyle.Simple;
            Object obj = (Object)binForm.Deserialize(fs);
            fs.Close();
            return obj;
        }*/
        public static List<FileDataInfo> getDifferenceFiles(ProgramInfo p, BuildInfo b){//Получить файлы у которых различается кеш
            if (!p.isInstalled())
                return null;
            List<FileDataInfo> list = new List<FileDataInfo>();
            foreach (var df in b.Files){
                var path = p.getInstallPath(false) + Utils.ConvertDirectory(df.Filepath, true, false);
                if (df.IsDirectory){
                    if(!Directory.Exists(path))
                        list.Add(df);
                }
                else if (!File.Exists(path) || (df.Hash != Utils.getHashFile(path) && !df.Ignore))
                    list.Add(df);
            }
            return list;
        }

        public static string GetGUID(){
            Assembly IsAssembly = Assembly.GetExecutingAssembly();
            var IsAttribute = (GuidAttribute)IsAssembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            var IsGUID = IsAttribute.Value;
            return "{" + IsGUID + "}";
        }
        public static int compareVersion(string ver1, string ver2)// -1 - ver1 больше, 0 - версии равны, 1 - версия 2 больше
        {
            if (ver1 == ver2)
                return 0;
            try{
                ver1 = ver1.Replace(".", "");
                ver2 = ver2.Replace(".", "");
                while (ver1.Length < 4)
                    ver1 += "0";
                while (ver2.Length < 4)
                    ver2 += "0";
                int v1 = Int32.Parse(ver1);
                int v2 = Int32.Parse(ver2);
                return v1 == v2 ? 0 : (v1 > v2 ? -1 : 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сравнении версий!\n"+ex.Message);
                Utils.pushCrashLog(ex);
            }
            return -1;
        }
        public static void pushCrashLog(Exception e)
        {
            Program.ExceptionHandler(e);
        }

        public static int CountFilesInDirectoryRecursive(string dir) {
            if (!Directory.Exists(dir))
                return 0;
            int sum = 0;
            var d = new DirectoryInfo(dir);
            sum += d.GetFiles().Length;
            foreach (var dr in d.GetDirectories())
                sum += CountFilesInDirectoryRecursive(dr.FullName);
            return sum;
        }
        public static void AddOrUpdateExeUninstallerInfo(ProgramInfo p, BuildInfo b) {
            if(!p.isInstalled())
                return;
            string uninstallerFile = AppDomain.CurrentDomain.BaseDirectory + "prefabuninstall.bin";// prefabuninstall.bin - это переименованный smart_uninstaller.exe проекта smart uninstaller
            if (!File.Exists(uninstallerFile))
                return;
            string fileUnistallDat = p.getInstallPath(false)+"uninstall.dat";
            UninstallInfo uninstallInfo = null;

            if (File.Exists(fileUnistallDat))
                try {
                    uninstallInfo = Utils.toObject<UninstallInfo>(File.ReadAllText(fileUnistallDat));
                }
                catch (Exception e)
                {
                    Utils.pushCrashLog(e);
                }

            if (uninstallInfo == null)
                uninstallInfo = new UninstallInfo();
            uninstallInfo.Name = p.Name;
            uninstallInfo.GUID= p.GUID;
            uninstallInfo.InstallName= p.InstallName;

            foreach (var fileDataInfo in b.Files) {
                if (fileDataInfo.IsDirectory)
                {
                    if(!uninstallInfo.dirs.Contains(fileDataInfo.Filepath))
                        uninstallInfo.dirs.Add(fileDataInfo.Filepath);
                }
                else
                {
                    if (!uninstallInfo.files.Contains(fileDataInfo.Filepath))
                        uninstallInfo.files.Add(fileDataInfo.Filepath);
                }
            }
            File.WriteAllText(fileUnistallDat,Utils.toJSON(uninstallInfo));
            File.Copy(uninstallerFile,p.getInstallPath(false)+"uninstall.exe",true);

        }
        public static void AddOrUpdateExeLauncherInfo(ProgramInfo p) {
            if(!p.isInstalled())
                return;
            string launcherFile = AppDomain.CurrentDomain.BaseDirectory + "prefablauncher.bin";// prefablauncher.bin - это переименованный smart_launcher.exe проекта smart launcher
            if (!File.Exists(launcherFile))
                return;
            string fileLauncherDat = p.getInstallPath(false)+ "launcher.dat";
            LauncherInfo launchInfo = new LauncherInfo();
            launchInfo.PathToExe= p.getInstallPath(true);
            launchInfo.Name = p.Name;
            launchInfo.ExeFile= p.ExeFile;
            launchInfo.GUID= p.GUID;
            launchInfo.InstallName= p.InstallName;
            
            File.WriteAllText(fileLauncherDat, Utils.toJSON(launchInfo));
            File.Copy(launcherFile, p.getInstallPath(false)+ "launcher.exe", true);
        }
        public static bool AddOrUpdateInstallInfo(string InstallName,string name, string version,string company, string installPath, string exePath,bool onlyForCurrentUser = false)
        {
            List<RegistryKey> tryKeys = new List<RegistryKey>();
            if (onlyForCurrentUser)
                tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
            else
            {
                tryKeys.Add(Microsoft.Win32.Registry.LocalMachine);
                tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
            }
            foreach (var registryKey in tryKeys){
                try
                {
                    RegistryKey IsRegKey = registryKey.CreateSubKey(@"SOFTWARE\" + InstallName);
                    IsRegKey.SetValue("DisplayName", name);
                    IsRegKey.SetValue("Version", version);
                    IsRegKey.SetValue("InstallDate", String.Format("{0:yyyyMMdd}", DateTime.Now));
                    IsRegKey.SetValue("InstallLocation", installPath);
                    IsRegKey.SetValue("ExecutePath", exePath);
                    if (!string.IsNullOrEmpty(company))
                        IsRegKey.SetValue("Publisher", company);
                    return true;

                }
                catch (Exception ex)
                {
                    Utils.pushCrashLog(ex);
                }
            }
            return false;
        }

        public static void StartApp(string path) {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.FileName = path;
            Process.Start(Info);
        }
        public static bool addToAutoStart(ProgramInfo p, bool onlyForCurrentUser = false){
            List<RegistryKey> tryKeys = new List<RegistryKey>();
            if (onlyForCurrentUser)
                tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
            else{
                tryKeys.Add(Microsoft.Win32.Registry.LocalMachine);
                tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
            }
            foreach (var registryKey in tryKeys){
                try{
                    Microsoft.Win32.RegistryKey myKey =
                        registryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                            true);
                    var appName = p.InstallName;
                    myKey.SetValue(appName, p.getInstallPath(true));
                    return true;

                }
                catch (Exception ex)
                {
                    Utils.pushCrashLog(ex);
                }
            }
            return false;
        }
        public static bool AddUninstaller(string installPath, string uninstallPath, string guid, string name, string version,string company ,bool onlyForCurrentUser = false)
        {
            List<RegistryKey> tryKeys = new List<RegistryKey>();
            if (onlyForCurrentUser)
                tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
            else
            {
                tryKeys.Add(Microsoft.Win32.Registry.LocalMachine);
                tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
            }

            DateTime IsDateNow = DateTime.Now;
            string IsDateInstall = String.Format("{0:yyyyMMdd}", IsDateNow);
            foreach (var registryKey in tryKeys){
                try{
                    RegistryKey IsRegKey = registryKey.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid);
                    IsRegKey.SetValue("DisplayName", name);
                    IsRegKey.SetValue("DisplayVersion", version);
                    IsRegKey.SetValue("InstallDate", IsDateInstall);
                    IsRegKey.SetValue("InstallLocation", installPath);
                    if (!string.IsNullOrEmpty(company))
                        IsRegKey.SetValue("Publisher", company);
                    IsRegKey.SetValue("UninstallString", uninstallPath);

                    return true;
                }
                catch (Exception IsError)
                {
                    Utils.pushCrashLog(IsError);
                }
            }
            return false;
        }

        public static string tryGetRegisterValue(RegistryKey key,string subName, string valueField) {
            try {
                var v1 = key.OpenSubKey(subName);
                if (v1 != null)
                    return v1.GetValue(valueField).ToString();
            }
            catch (Exception e) { }

            return null;
        }
        public static bool AddShortcut(string installPath,string uninstallPath,string launcherPath, string pathToExe,string installName,string iconPath, string name,string description, string company, bool addToStartMenu = true, bool addToDesktop = true, bool onlyForCurrentUser = false)
        {
            string startMenuDir = "";
            string shortcutDesktop = "";
            try{
                string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                if(onlyForCurrentUser)
                    commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", name);
                if (!string.IsNullOrEmpty(company))
                    appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", company, name);
                if (!Directory.Exists(appStartMenuPath))
                    Directory.CreateDirectory(appStartMenuPath);
                string shortcutLocation = Path.Combine(appStartMenuPath, name + ".lnk");
                WshShell shell = new WshShell();
                if (addToStartMenu)
                {
                    IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(shortcutLocation);
                    shortcut.Description = description;
                    if (!string.IsNullOrEmpty(iconPath))
                        shortcut.IconLocation = iconPath;
                    shortcut.TargetPath = launcherPath;
                    shortcut.Save();


                    shortcutLocation = Path.Combine(appStartMenuPath, "Проверить обновление.lnk");
                    shortcut = (IWshShortcut) shell.CreateShortcut(shortcutLocation);
                    shortcut.Description = "Проверить обновление " + name;
                    if (!string.IsNullOrEmpty(iconPath))
                        shortcut.IconLocation = iconPath;
                    shortcut.Arguments = "checkupdate";
                    shortcut.TargetPath = launcherPath;
                    shortcut.Save();

                    shortcutLocation = Path.Combine(appStartMenuPath, "Удалить программу.lnk");
                    shortcut = (IWshShortcut) shell.CreateShortcut(shortcutLocation);
                    shortcut.Description = "Удаление программы " + name;
                    if (!string.IsNullOrEmpty(iconPath))
                        shortcut.IconLocation = iconPath;
                    shortcut.TargetPath = uninstallPath;
                    shortcut.Save();

                    startMenuDir = appStartMenuPath;

                }
                if (addToDesktop) {
                    var deskLink =
                        Environment.GetFolderPath(onlyForCurrentUser
                            ? Environment.SpecialFolder.DesktopDirectory
                            : Environment.SpecialFolder.CommonDesktopDirectory);
                    deskLink = Path.Combine(deskLink, name + ".lnk");
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(deskLink);
                    shortcut.Description = description;
                    if (!string.IsNullOrEmpty(iconPath))
                        shortcut.IconLocation = iconPath;
                    shortcut.TargetPath = pathToExe;
                    shortcut.Save();
                    shortcutDesktop = deskLink;
                }

                //Добавить записи в реестр о размещение ярылыков
                List<RegistryKey> tryKeys = new List<RegistryKey>();
                if (onlyForCurrentUser)
                    tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
                else
                {
                    tryKeys.Add(Microsoft.Win32.Registry.LocalMachine);
                    tryKeys.Add(Microsoft.Win32.Registry.CurrentUser);
                }
                foreach (var registryKey in tryKeys){
                    try
                    {
                        RegistryKey IsRegKey = registryKey.CreateSubKey(@"SOFTWARE\" + installName);
                        if(!string.IsNullOrEmpty(shortcutDesktop))
                            IsRegKey.SetValue("DesktopShortcut", shortcutDesktop);
                        if(!string.IsNullOrEmpty(startMenuDir))
                            IsRegKey.SetValue("StartMenuDirectory", startMenuDir);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Utils.pushCrashLog(ex);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utils.pushCrashLog(ex);
            }
            return false;
        }
    }
}

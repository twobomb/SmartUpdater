using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace SmartLauncher
{
    class Program
    {
        static void Main(string[] args)
        {

            bool isOnlyCheckUpdate = (args != null && args.Length > 0 && args[0].Contains("checkupdate"));

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            if (!baseDir.EndsWith(@"\"))
                baseDir += @"\";

            string fileLaunchDat = baseDir + "launcher.dat";

            if (!File.Exists(fileLaunchDat)) {
                NativeMethods.MsgBox(IntPtr.Zero, "Не удается запустить. Не найден файл launcher.dat!",
                    "Ошибка запуска",
                    (int)(0x00000000L | 0x00000010L | 0x00001000L));
                return;
            }
            LauncherInfo info = JsonConvert.DeserializeObject<LauncherInfo>(File.ReadAllText(fileLaunchDat));

            string exeSmartUpdater = getExePathSmartUpdater();

            string expApp = getExePath(info);

            if (string.IsNullOrEmpty(exeSmartUpdater)) {
                NativeMethods.MsgBox(IntPtr.Zero, "Не удалось найти приложение SmartUpdater. Без него программа не будет обновляться, обратитесь к администратору!",
                    "Не найдено",
                    (int)(0x00000000L | 0x00000010L | 0x00001000L));
                if(isOnlyCheckUpdate)
                    return;
                if (string.IsNullOrEmpty(expApp))
                    NativeMethods.MsgBox(IntPtr.Zero, "Не удалось найти файл запуска приложения!",
                        "Не найдено",
                        (int)(0x00000000L | 0x00000010L | 0x00001000L));
                else
                    StartApp(info.PathToExe);
                return;
            }

            ProcessStartInfo psi= new ProcessStartInfo();
            //update=1 launch=1 guid="{18fb57ee-219c-4325-b679-6f2889d3f911}" exefile="C:\Program Files\New Program\new.exe"
            string type = isOnlyCheckUpdate? "update":"launch";
            psi.Arguments = String.Format("{0}=1 guid=\"{1}\" exefile=\"{2}\"",type,info.GUID, expApp);
            psi.FileName = exeSmartUpdater;
            Process.Start(psi);
            return;

        }

        public static string getExePath(LauncherInfo info) {
            string baseDir = ConvertDirectory(AppDomain.CurrentDomain.BaseDirectory,false,true);
            if (File.Exists(baseDir + ConvertDirectory(info.PathToExe, false, false)))
                return baseDir + ConvertDirectory(info.PathToExe, false, false);

            string path = "";
            path = tryGetRegisterValue(Registry.CurrentUser,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + info.GUID, "InstallLocation");
            if (string.IsNullOrEmpty(path))
                path = tryGetRegisterValue(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + info.GUID, "InstallLocation");
            if (string.IsNullOrEmpty(path))
                path = tryGetRegisterValue(Registry.CurrentUser, @"SOFTWARE\" + info.InstallName, "InstallLocation");
            if (string.IsNullOrEmpty(path))
                path = tryGetRegisterValue(Registry.LocalMachine, @"SOFTWARE\" + info.InstallName, "InstallLocation");
            if (!string.IsNullOrEmpty(path) && File.Exists(ConvertDirectory(path, false, true) + ConvertDirectory(info.ExeFile, false, false)))
                return ConvertDirectory(path, false, true) + ConvertDirectory(info.ExeFile, false, false);
            if (File.Exists(info.PathToExe))
                return info.PathToExe;
            path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\" + info.InstallName + "\\" +
                   ConvertDirectory(info.ExeFile, false, false);
            if(File.Exists(path))
                return path;

            return null;
        }
        public static void StartApp(string path) {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.FileName = path;
            Process.Start(Info);
        }

        public static string getExePathSmartUpdater() {
            string GUID_SMART_UPDATER = "{c2df9925-83ea-4fe2-af92-290eacd343c1}";
            string path = "";

            path = tryGetRegisterValue(Registry.CurrentUser, @"SOFTWARE\SmartUpdater" , "ExecutePath");
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                return path;
            
            path = tryGetRegisterValue(Registry.LocalMachine, @"SOFTWARE\SmartUpdater", "ExecutePath");
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                return path;

            path = tryGetRegisterValue(Registry.CurrentUser,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID_SMART_UPDATER, "InstallLocation");
            if (!string.IsNullOrEmpty(path) && File.Exists(ConvertDirectory(path, false, true) + "SmartUpdater.exe"))
                return ConvertDirectory(path, false, true) + "SmartUpdater.exe";

            path = tryGetRegisterValue(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID_SMART_UPDATER, "InstallLocation");

            if (!string.IsNullOrEmpty(path) && File.Exists(ConvertDirectory(path, false, true) + "SmartUpdater.exe"))
                return ConvertDirectory(path, false, true) + "SmartUpdater.exe";

            path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                   "\\SmartUpdater\\SmartUpdater.exe";
            if (File.Exists(path))
                return path;

            return null;
        }

        public static string tryGetRegisterValue(RegistryKey key, string subName, string valueField)
        {
            try
            {
                var v1 = key.OpenSubKey(subName);
                if (v1 != null)
                    return v1.GetValue(valueField).ToString();
            }
            catch (Exception e) { }

            return null;
        }

        public static string ConvertDirectory(string route, bool beginSlash = true, bool endSlash = true)
        {
            if (route == null)
                return "";
            route = route.Trim();
            route = route.Replace("/", @"\");
            route = Regex.Replace(route, @"^\\", "");
            route = Regex.Replace(route, @"\\$", "");
            if (beginSlash)
                route = @"\" + route;
            if (endSlash)
                route = route + @"\";
            return route;
        }
    }
}

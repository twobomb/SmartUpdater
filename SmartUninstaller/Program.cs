using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace SmartUninstaller
{

    class Program {
        static void Main(string[] args) {

            bool isSkipConfirm = (args != null && args.Length > 0 && args[0].Contains("skipconfirm"));

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            if (!baseDir.EndsWith(@"\"))
                baseDir += @"\";

            string fileUnistallDat = baseDir + "uninstall.dat";
            if (!File.Exists(fileUnistallDat)) {
                NativeMethods.MsgBox(IntPtr.Zero, "Не найден файл uninstall.dat!",
                    "Ошибка удаления",
                    (int)(0x00000000L | 0x00000010L | 0x00001000L));
                return;
            }
            UninstallInfo info = JsonConvert.DeserializeObject<UninstallInfo>(File.ReadAllText(fileUnistallDat)); 
            if (isSkipConfirm || NativeMethods.MsgBox(IntPtr.Zero, "Вы уверены что хотите удалить программу '"+ info .Name+ "'?", "Подтверждение удаления",
                (int)(0x00000003L | 0x00000030L | 0x00001000L)) == 6) {
                foreach (var infoFile in info.files) {
                    try {
                        if(File.Exists(baseDir+infoFile))
                            File.Delete(baseDir + infoFile);
                    }
                    catch (Exception e) { }
                }
                foreach (var infoDir in info.dirs) {
                    try {
                        if (CountFilesInDirectoryRecursive(baseDir + infoDir) == 0) 
                            Directory.Delete(baseDir + infoDir,true);
                    }
                    catch (Exception e) { }
                }

                try
                {
                    var sub = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + info.InstallName, false);
                    if (sub != null)
                    {
                        var menuDir = sub.GetValue("StartMenuDirectory");
                        if (menuDir != null && menuDir.ToString() != "")
                            Directory.Delete(menuDir.ToString(), true);

                        var desktop = sub.GetValue("DesktopShortcut");
                        if (desktop != null && desktop.ToString() != "" && File.Exists(desktop.ToString()))
                            File.Delete(desktop.ToString());
                    }
                }
                catch (Exception e) { }

                try
                {
                    var sub = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + info.InstallName, false);
                    if (sub != null)
                    {
                        var menuDir = sub.GetValue("StartMenuDirectory");
                        if (menuDir != null && menuDir.ToString() != "")
                            Directory.Delete(menuDir.ToString(), true);

                        var desktop = sub.GetValue("DesktopShortcut");
                        if (desktop != null && desktop.ToString() != "" && File.Exists(desktop.ToString()))
                            File.Delete(desktop.ToString());
                    }
                }
                catch (Exception e) { }

                try
                {
                    Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + info.GUID, false);
                }
                catch (Exception e) { }
                try
                {
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + info.GUID, false);
                }
                catch (Exception e) { }
                try
                {
                    Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\" + info.InstallName, false);
                }
                catch (Exception e) { }
                try
                {
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\" + info.InstallName, false);
                }
                catch (Exception e) { }

                string fileLauncher = baseDir + "launcher.exe";
                string fileLauncherDat = baseDir + "launcher.dat";

                File.Delete(fileLauncher);
                File.Delete(fileLauncherDat);
                File.Delete(fileUnistallDat);
                int m = 30;
                while (File.Exists(fileUnistallDat) || File.Exists(fileLauncher) || File.Exists(fileUnistallDat))
                {
                    Thread.Sleep(100);
                    if (--m <= 0)
                        break;
                }
                if (CountFilesInDirectoryRecursive(baseDir) == 1) {
                    //удалить с консоли отложенно
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C choice /C Y /N /D Y /T 3 & rd \"" + baseDir+ "\" /s /q";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process.Start(Info);
                }
                else
                {
                    //удалить с консоли отложенно
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Info.Arguments = "/C choice /C Y /N /D Y /T 3 & del \"" + baseDir + "uninstall.exe\"";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process.Start(Info);

                }
            }


        }
        public static int CountFilesInDirectoryRecursive(string dir)
        {
            if (!Directory.Exists(dir))
                return 0;
            int sum = 0;
            var d = new DirectoryInfo(dir);
            sum += d.GetFiles().Length;
            foreach (var dr in d.GetDirectories())
                sum += CountFilesInDirectoryRecursive(dr.FullName);
            return sum;
        }

    }
}

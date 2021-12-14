using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartCopy
{
    class Program
    {

        public static void Copy(string sourceDir, string targetDir, bool replace = true)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir)){
                try{
                    File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), replace);
                }
                catch (Exception){}
            }
            foreach (var directory in Directory.GetDirectories(sourceDir))
                Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        public static void KillProcesses(string path){
            var processes = Process.GetProcesses();
            foreach (var process1 in processes){//Убить открытые процессы
                try
                {
                    if (process1.MainModule != null)
                    {
                        var filePath = new FileInfo(process1.MainModule.FileName);
                        FileInfo inst = new FileInfo(path);
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
        static void Main(string[] args){
            //start - запустить по окончании
            //arguments - аргументы запуска
            //deletesource - удалить источник по окончании
            //killProcessPath - убить процесс путь к exe
            //isDebugError - писать ошибка в файл copyerror.log
            //source="C:\Program Files\New Program" destination="C:\Program Files\New Program2"  start="C:\test.exe" arguments="file=\"qwe.exe\"" killprocesspath="C:\qwe.exe" deletesource=1 isDebugError=1


            try
            {
                string source = "";
                string destination = "";
                string start = "";
                string arguments = "";
                string killProcessPath = "";
                bool isDeleteSource = false;
                bool isDebugError = false;
                foreach (var s in args)
                {
                    if (s.Contains("="))
                    {
                        var m = Regex.Match(s, "^([^=]+)=(.+)$");
                        if(!m.Success)
                            continue;
                        string param = m.Groups[1].Value;
                        string val = m.Groups[2].Value;
                        switch (param)
                        {
                            case "source":
                                source = val;
                                break;
                            case "destination":
                                destination = val;
                                break;
                            case "start":
                                start = val;
                                break;
                            case "arguments":
                                arguments = val;
                                break;
                            case "killprocesspath":
                                killProcessPath = val;
                                break;
                            case "deletesource":
                                isDeleteSource = (val.Trim().ToLower() == "1" || val.Trim().ToLower() == "true");
                                break;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(killProcessPath))
                    KillProcesses(killProcessPath);

                if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(destination) &&
                    (new DirectoryInfo(source)).Exists){
                    Copy(source, destination);
                    try{
                        if(isDeleteSource)
                            Directory.Delete(source,true);
                    }
                    catch (Exception){
                    }
                }

                if (!string.IsNullOrWhiteSpace(start) && File.Exists(start))
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    if (!string.IsNullOrWhiteSpace(arguments))
                        psi.Arguments = arguments;
                    psi.FileName = start;
                    Process.Start(psi);
                }
            }
            catch (Exception ex) {
                try
                {
                    File.WriteAllText(ex.ToString(),AppDomain.CurrentDomain.BaseDirectory+"copyerror.log");
                }
                catch (Exception)
                {
                    
                }
            }

            
        }
    }
}

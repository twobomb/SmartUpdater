using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SmartUpdater.Properties;

namespace SmartUpdater
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            Application.ThreadException += Application_ThreadException;

//            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory+"args.log","r\n\r\n\r\n"+string.Join("|",args));

            //launch обновить и запустить, update - только обновить,noupdate -не запускать обновление SmartUpdater'a
            //noupdate=1 update=1 launch=1 guid="{417010c9-0f53-4276-ab7c-0d50ca50bbce}" exefile="C:\Program Files\New Program\new.exe"
            bool isUpdate= false;
            bool isLauncher = false;
            bool isNoUpdate = false;
            string guiid = "";
            string exefile= "";
            foreach (var s in args) {
                if (s.Contains("=")){
                    var m = Regex.Match(s, "^([^=]+)=(.+)$");
                    if (!m.Success)
                        continue;
                    string param = m.Groups[1].Value;
                    string val = m.Groups[2].Value;
                    switch (param) {
                        case "launch":
                            val = val.ToLower();
                            isLauncher = (val == "1" || val == "true");
                            break;
                        case "update":
                            val = val.ToLower();
                            isUpdate = (val == "1" || val == "true");
                            break;
                        case "noupdate":
                            val = val.ToLower();
                            isNoUpdate = (val == "1" || val == "true");
                            break;
                        case "guid":
                            guiid = val;
                            break;
                        case "exefile":
                            exefile= val;
                            break;
                    }
                }
            }
            if(!isNoUpdate)
                SelfUpdate();


            

            if ((isLauncher || isUpdate) && !Utils.CheckConnect()){
                MessageBox.Show("Не удалось связаться с сервером обновлений! Программа не может быть обновлена, обратитесь к администратору!", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (!isLauncher)
                    return;
                if (!string.IsNullOrEmpty(exefile) && File.Exists(exefile))
                    Utils.StartApp(exefile);
                else
                    MessageBox.Show("Не удалось найти файл запуска", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (isLauncher) {
                if (!string.IsNullOrEmpty(guiid)) {
                    var prog = ProgramInfo.DownloadByGuid(guiid);
                    if (prog == null) {
                        if (!string.IsNullOrEmpty(exefile) && File.Exists(exefile)) 
                            Utils.StartApp(exefile);
                        else
                            MessageBox.Show("Не удалось найти файл запуска", "Предупреждение",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else {
                        if (prog.canUpdate())
                        {
                            var build = BuildInfo.DownloadActualVersionInfo(prog);
                            if (!build.UpdateRequired) {
                                if (MessageBox.Show("Доступна новая версия. Установить?", "Новая версия",
                                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes) {
                                        if (!string.IsNullOrEmpty(exefile) && File.Exists(exefile))
                                            Utils.StartApp(exefile);
                                        else if (File.Exists(prog.getInstallPath(true)))
                                            Utils.StartApp(prog.getInstallPath(true));
                                        else
                                            MessageBox.Show("Не удалось найти файл запуска", "Предупреждение",
                                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                }
                            }
                            dlg_loader loader = new dlg_loader();
                            CancellationTokenSource source = new CancellationTokenSource();
                            var token = source.Token;
                            loader.button1.Click += (o, args1) => {
                                source.Cancel(false);
                            };
                            loader.Shown += (o, args1) => {
                                prog.UpdateTo( build, (b, error) => {
                                        loader.Invoke(new Action(() => {
                                            loader.Close();
                                        }));

                                        if (!b)
                                            MessageBox.Show(
                                                "Обновление завершено неудачей.\n" + error,
                                                "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                        if (!string.IsNullOrEmpty(exefile) && File.Exists(exefile))
                                            Utils.StartApp(exefile);
                                        else
                                            MessageBox.Show("Не удалось найти файл запуска", "Предупреждение",
                                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                },
                                    (down, all) => {
                                        loader.BeginInvoke(new Action(() => {
                                            loader.progressBar1.Value = (int)(((double)down / (double)all) * 100);
                                            loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                                        }));
                                    }, token);
                            };
                            loader.ShowDialog();
                        }
                        else {
                            if (!string.IsNullOrEmpty(exefile) && File.Exists(exefile)) 
                                Utils.StartApp(exefile);
                            else if (File.Exists(prog.getInstallPath(true)))
                                Utils.StartApp(prog.getInstallPath(true));
                            else 
                                MessageBox.Show("Не удалось найти файл запуска", "Предупреждение",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else {
                    if (!string.IsNullOrEmpty(exefile) && File.Exists(exefile))
                        Utils.StartApp(exefile);
                    else
                        MessageBox.Show("Не удалось найти файл запуска", "Предупреждение",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return;
            }
            else if (isUpdate) {
                var prog = ProgramInfo.DownloadByGuid(guiid);
                if (prog.canUpdate()) {

                    var build = BuildInfo.DownloadActualVersionInfo(prog);
                    if (!build.UpdateRequired) {
                        if (MessageBox.Show("Доступна новая версия. Установить?", "Новая версия",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes) 
                            return;
                    }
                    dlg_loader loader = new dlg_loader();
                    CancellationTokenSource source = new CancellationTokenSource();
                    var token = source.Token;
                    loader.button1.Click += (o, args1) => {
                        source.Cancel(false);
                    };
                    loader.Shown += (o, args1) => {
                        prog.UpdateTo(build, (b, error) => {
                            loader.Invoke(new Action(() => {
                                loader.Close();
                            }));

                            if (!b)
                                MessageBox.Show(
                                    "Обновление завершено неудачей.\n" + error,
                                    "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                MessageBox.Show("Программа обновлена до версии "+build.Version, "Успех",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        },
                            (down, all) => {
                                loader.BeginInvoke(new Action(() => {
                                    loader.progressBar1.Value = (int)(((double)down / (double)all) * 100);
                                    loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                                }));
                            }, token);
                    };
                    loader.ShowDialog();
                }
                else {
                    MessageBox.Show("Программа не нуждается в обновлении.Установлена последняя версия!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        static void SelfUpdate(){
            try{
                var curDir = Utils.ConvertDirectory(AppDomain.CurrentDomain.BaseDirectory,false,true);
                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                var argsStart ="noupdate=1 "+ string.Join(" ", Environment.GetCommandLineArgs().Skip(1).Select(s => "\\\""+s+"\\\""));
                string copyUtils = curDir + "copy.exe";
                if (!File.Exists(copyUtils)){
                    throw new Exception("Утилита copy.exe не найдена. Обновление не возможно!");
                }
                var cur = Application.ProductVersion;
                BuildInfo lastBuild = null;
                HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(Settings.Default.host +@"/smartupdater/lastsmartupdater.json");
                if (client.GetResponse().ContentType == "application/json"){
                    using (StreamReader sr = new StreamReader(client.GetResponse().GetResponseStream()))
                    {
                        string res = sr.ReadToEnd();
                        sr.Close();
                        lastBuild = Utils.toObject<BuildInfo>(res);
                    }
                }
                if(Utils.compareVersion(cur,lastBuild.Version) != 1)
                    return;

                List<FileDataInfo> filesToUpdate = new List<FileDataInfo>();
                var temp = Utils.getEmptyTempDir();
                CancellationTokenSource cancel = new CancellationTokenSource();
                var token = cancel.Token;
                dlg_loader loader = new dlg_loader();
                loader.button1.Visible = false;
                loader.label2.Text = "Обновление приложения SmartUpdater";
                loader.Shown += (o, args) =>{
                    Utils.DownloadFilesAsync(Utils.ConvertRoute(Settings.Default.host, false, false) + "/smartupdater/last", lastBuild.Files, temp, (b, error) =>{
                        try{
                            ProcessStartInfo psi = new ProcessStartInfo();
                            psi.Arguments = String.Format("source=\"{0}\" destination=\"{1}\"  start=\"{2}\" arguments=\"{3}\" killprocesspath=\"{4}\" deletesource=1", Utils.ConvertDirectory(temp, false, false), Utils.ConvertDirectory(curDir,false,false), exePath, argsStart, exePath);
                            psi.FileName = copyUtils;
                            Process.Start(psi);
                        }
                        catch (Exception){
                            loader.Close();
                        }

                    },
                        (down, all) =>
                        {

                            loader.BeginInvoke(new Action(() =>
                            {
                                loader.progressBar1.Value = (int)(((double)down / (double)all) * 100);
                                loader.label1.Text = "Загружено " + Utils.SizeSuffix(down) + " из " + Utils.SizeSuffix(all);
                            }));
                        }, token);
                };
                loader.ShowDialog();
            }
            catch (Exception eex){
                Utils.pushCrashLog(eex);
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ExceptionHandler(e.Exception);

        }



        public static string DIR_CRASHLOG = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                            @"\SmartUpdater Crash Logs";
        public static void ExceptionHandler(Exception e){

            if (!Directory.Exists(DIR_CRASHLOG))
                Directory.CreateDirectory(DIR_CRASHLOG);

            var f = File.CreateText(DIR_CRASHLOG + @"\crash-" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".log");
            f.WriteLine("ВРЕМЯ");
            f.WriteLine(DateTime.Now.ToString("G"));
            f.WriteLine("СИСТЕМА");
            f.WriteLine(System.Environment.OSVersion.ToString());
            f.WriteLine("ИСКЛЮЧЕНИЕ В ИСХОДНОМ ВИДЕ");
            f.WriteLine(e.ToString());
            f.WriteLine("ТРАССИРОВКА СТЕКА");
            f.WriteLine(System.Environment.StackTrace);
            f.WriteLine("СООБЩЕНИЕ");
            f.WriteLine(e.Message);
            f.WriteLine("ВНУТРЕННЕЕ ИСКЛЮЧЕНИЕ");
            if (e.InnerException == null)
                f.WriteLine("Не найдено");
            else
                f.WriteLine(e.InnerException.ToString());

            f.Close();

            if (e.GetType() == typeof(DataException))
            {
                DataException ex = e as DataException;
                string text = ex.Message;
                if (ex.InnerException != null)
                    text = ex.InnerException.Message;
                MessageBox.Show(text, "Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            //launch обновить и запустить, update - только обновить
            //update=1 launch=1 guid="{18fb57ee-219c-4325-b679-6f2889d3f911}" exefile="C:\Program Files\New Program\new.exe"
            bool isUpdate= false;
            bool isLauncher = false;
            string guiid = "";
            string exefile= "";
            foreach (var s in args) {
                if (s.Contains("=")){
                    string param = s.Split('=')[0].Trim().ToLower();
                    string val= s.Split('=')[1].Trim().Replace("\"","");
                    switch (param) {
                        case "launch":
                            val = val.ToLower();
                            isLauncher = (val == "1" || val == "true");
                            break;
                        case "update":
                            val = val.ToLower();
                            isUpdate = (val == "1" || val == "true");
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

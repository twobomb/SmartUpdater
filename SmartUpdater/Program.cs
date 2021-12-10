using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        static void Main()
        {

            Application.ThreadException += Application_ThreadException;
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

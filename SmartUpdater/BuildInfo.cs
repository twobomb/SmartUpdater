using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SmartUpdater.Properties;

namespace SmartUpdater
{
    public class BuildInfo{
        public string Name { get; set; }
        public string NeedMinVersionSmartUpdateForUpdate { get; set; }// Минимальная необходимая версия апдейтера для установки этого апдейта
        public string GUID{ get; set; } // уникальный ид программы
        public string Version{ get; set; } // версия
        public bool UpdateOnlyChanges { get; set; } //true-  скачать файлы только с другими хешами, false -скачать все файлы
        public bool ClearAfterInstall{ get; set; } //Очищать папку перед обновлением
        public bool UpdateRequired { get; set; }  // обязательное обновление, если нет то будет спрашивать
        public string ServerPath{ get; set; }  // путь на сервере к папке с билдом, относительно папки с билдами
        public List<FileDataInfo> Files{ get; set; }  // файлы программы
        public List<string> ChangeList { get; set; } // список изменений




        public static BuildInfo DownloadActualVersionInfo(ProgramInfo info){
            try
            {
                info.Path = info.Path.Trim();
                if (!info.Path.EndsWith("/"))
                    info.Path += "/";
                HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(Settings.Default.host + info.Path+ "current.json");
                if (client.GetResponse().ContentType == "application/json"){
                    using (StreamReader sr = new StreamReader(client.GetResponse().GetResponseStream()))
                    {
                        string res = sr.ReadToEnd();
                        sr.Close();
                        return Utils.toObject<BuildInfo>(res);
                    }
                }
                else
                    throw new Exception("Неизвестный типа файла на сервере. Ожидается application/json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка получении информации о последней версии программы!\n " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public override string ToString()
        {
            return "v."+Version;
        }

        public string GetServerRootPath(ProgramInfo p = null){
            if (p == null)
                p = ProgramInfo.DownloadList().First(info => info.GUID == this.GUID);
            return Utils.ConvertRoute(Settings.Default.host,false,false) + Utils.ConvertRoute(p.Path) + Utils.ConvertRoute(ServerPath);
        }

        public static BuildInfo DownloadBuildByVersion(ProgramInfo info,string version)
        {
            var list = DownloadAllVersionInfo(info);
            foreach (var buildInfo in list)
                if (Utils.compareVersion(buildInfo.Version, version) == 0)
                    return buildInfo;
            return null;
        } 
        public static List<BuildInfo> DownloadAllVersionInfo(ProgramInfo info){
            try
            {
                info.Path = info.Path.Trim();
                if (!info.Path.EndsWith("/"))
                    info.Path += "/";
                HttpWebRequest client = (HttpWebRequest)HttpWebRequest.Create(Settings.Default.host + info.Path+ "all.json");
                if (client.GetResponse().ContentType == "application/json"){
                    using (StreamReader sr = new StreamReader(client.GetResponse().GetResponseStream()))
                    {
                        string res = sr.ReadToEnd();
                        sr.Close();
                        return Utils.toObject<List<BuildInfo>>(res);
                    }
                }
                else
                    throw new Exception("Неизвестный типа файла на сервере. Ожидается application/json");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка получении информации о всех версиях программы!\n " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartUpdater
{
    public class FileDataInfo{
        public bool IsDirectory { get; set; }
        public string Filepath { get; set; }
        public string Hash{ get; set; }
        public int Size{ get; set; } //byte
        public bool Ignore{ get; set; }//при обновлении даже если кеши не совпадают не качает, качает при установке или при отстутсвии, например для ini файлов
        public string Tag{ get; set; } // Какая нибудь доп инфа

        public FileDataInfo()
        {
            IsDirectory = false;
            Ignore = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmartUpdater
{

    public class UninstallInfo {
        public string Name { get; set; }
        public string InstallName { get; set; }
        public string GUID { get; set; }
        public List<string> files{ get; set; }
        public List<string> dirs{ get; set; }

        public UninstallInfo()
        {
            files = new List<string>();
            dirs= new List<string>();
        }
    }
}

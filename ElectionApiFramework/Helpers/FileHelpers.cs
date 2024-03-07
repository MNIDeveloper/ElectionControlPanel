using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ElectionApiFramework.Helpers
{
    public class FileHelpers
    {
        public string GetBasePath()
        {
            
            string path = AppDomain.CurrentDomain.BaseDirectory; 
            if (path.Contains("C:\\")) return path.Substring(0, path.IndexOf("\\bin\\"));
            return path;
        }
        
    }
}

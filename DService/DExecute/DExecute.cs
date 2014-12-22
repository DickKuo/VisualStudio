using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DExecute
{
    public class DExecute
    {

        public DExecute()
        { 
        
        }

        public virtual void Start()
        { 
                
        }

        public virtual void UpdateDll(string UpdatePath)
        {
            DirectoryInfo LocalDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            List<string> LocalFileList = new List<string>();
            foreach (FileInfo FiInfo in LocalDir.GetFiles())
            {
                LocalFileList.Add(FiInfo.Name);
            }
            DirectoryInfo RemoveDir = new DirectoryInfo(UpdatePath);
            foreach (FileInfo FiInfo in RemoveDir.GetFiles())
            {

                //FileVersionInfo SourceFile = FileVersionInfo.GetVersionInfo(fi.FullName);

            }
        }
    }
}

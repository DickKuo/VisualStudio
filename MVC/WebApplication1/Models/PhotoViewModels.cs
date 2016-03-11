using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code;

namespace WebApplication1.Models
{
    public class PhotoViewModels
    {

        public class PhotoViewModel {

            public Photo Photo { set; get; }


            public List<Photo> PhotoList { set; get; }

            public string PhotoJson { set; get; }
        }


        //public class FileUpLoadViewModel
        //{

        //    private List<string> _PhotoFileNames = new List<string>();

        //    public List<string> PhotoFileNames { get { return this._PhotoFileNames; } set { this._PhotoFileNames = value; } }
        
        //}

    }

    
}
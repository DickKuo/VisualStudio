using System.Collections.Generic;
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
    }    
}
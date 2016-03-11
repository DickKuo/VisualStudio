using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code;
using WebApplication1.Code.DAO;
using WebApplication1.Models.Code;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Controllers
{
    public class FileLoadController : BaseLoginController
    {

        private const string FtpPath = @"D:\Images";
        private string Url = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();
        private const string ApiController = "Carousel";
        private const string ApiAction = "Uploading";


        public ActionResult Index()
        {
            //WebApplication1.Models.PhotoViewModels.FileUpLoadViewModel Mode = new WebApplication1.Models.PhotoViewModels.FileUpLoadViewModel();
            WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Mode = new Models.EmployeeMgrViewModels.EmployeeManamgerViewModel();
            return View(Mode);
        }

        
        /// <summary>前端回傳檔案資訊 </summary>
        /// <param name="act"></param>
        /// <param name="Model"></param>
        /// <param name="myFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string act, WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model, HttpPostedFileBase myFile)
        {
        
            switch (act)
            {
                case "upload":
                    this.UpLoadPhoto(Model, myFile);
                    break;

                case "post":
                    this.LogWrite(Model);
                    return RedirectToAction("Index", "Carousel");
                  
            }
            return View(Model);
        }


        /// <summary>寫入檔案資料</summary>
        /// <param name="Model"></param>
        private void LogWrite(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            using (FileStream fs = new FileStream(@"D:\Logs\myLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                WebApi Api = new WebApi();

                StreamWriter sw = new StreamWriter(fs);
                foreach (string strPhotoFileName in Model.PhotoFileNames)
                {
                    sw.WriteLine(strPhotoFileName);
                    string strFilePath = Server.MapPath("~/UpLoadFiles/" + strPhotoFileName);
                    string DestFilePath =FtpPath+Path.DirectorySeparatorChar+strPhotoFileName;
                    System.IO.File.Move(strFilePath, DestFilePath);

                    string[] sp = strPhotoFileName.Split('.');
                    Photo photo = new Photo();
                    photo.ImageNo = sp[0];
                    photo.AttachedFileName = sp[1];
                    photo.Remark = Model.ImageRemark;
                    Operation Obj = new Operation();
                    Obj.Method = OperationMethod.Method.Add;
                    Obj.Entity = Entities.EntitiesList.Photo;
                    Obj.Obj = photo;
                    ApiOperation Oper = Init(Obj);
                    Api.ApiOperation(Oper);
                }
                sw.Close();


              
            }   
        
        }

        /// <summary>Api初始化</summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        private ApiOperation Init(Operation Obj)
        {
            ApiOperation Oper = new ApiOperation();
            string _url = string.Format("{0}/{1}/{2}", Url, ApiController, ApiAction);
            Oper.Uri = _url;
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Obj;
            return Oper;
        }


        /// <summary>放暫存檔案</summary>
        /// <param name="Model"></param>
        /// <param name="MyFile"></param>
        private void UpLoadPhoto(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model, HttpPostedFileBase MyFile)
        {
            if (MyFile != null && MyFile.ContentLength > 0)
            {
                string strFileName = Guid.NewGuid().ToString() + Path.GetExtension(MyFile.FileName);
                string strFilePath = Server.MapPath("~/UpLoadFiles/" + strFileName);
                MyFile.SaveAs(strFilePath);
                Model.PhotoFileNames.Add(strFileName);
            }
        }

      
    }
}

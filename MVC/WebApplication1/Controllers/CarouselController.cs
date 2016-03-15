using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code;
using WebApplication1.Code.DAO;
using WebApplication1.Models;
using WebRoutingTest.Models.Code;
using System.IO;
using WebApplication1.Models.Code;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class CarouselController : BaseLoginController
    {

        private const string FtpPath = @"D:\Images";
        private string WebUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();
        private const string ApiController = "Carousel";
        private const string ApiAction = "Uploading";


        //
        // GET: /Carousel/
        public ActionResult Index()
        {           
            return View();
        }

                
        /// <summary>載入圖片管理列表</summary>
        /// <returns></returns>
        public ActionResult LoadingPartial()
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.Photo;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            Operation Result = JsonConvert.DeserializeObject<Operation>(Api.ApiOperation(Oper));

            PhotoViewModels.PhotoViewModel Model = new PhotoViewModels.PhotoViewModel();
            Model.PhotoList = new List<Photo>();
            foreach (var Item in Result.ObjList)
            {
                Photo photo = JsonConvert.DeserializeObject<Photo>(Item.ToString());
                string imagePath = Path.Combine(FtpPath, photo.ImageNo + "." + photo.AttachedFileName);


                Model.PhotoList.Add(photo);
            }
          

            return PartialView("_PhotoListPartialView", Model);
        }


        /// <summary>Api初始化</summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        private ApiOperation Init(Operation Obj)
        {
            ApiOperation Oper = new ApiOperation();
            string _url = string.Format("{0}/{1}/{2}", WebUrl, ApiController, ApiAction);
            Oper.Uri = _url;
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Obj;
            return Oper;
        }


        /// <summary>載入上傳畫面</summary>
        /// <returns></returns>
        //public ActionResult LoadingUploadPage()
        //{
        //    PhotoViewModels.FileUpLoadViewModel Model = new PhotoViewModels.FileUpLoadViewModel();

        //    return PartialView("_UpLoadPartialView", Model);
        //}



        /// <summary>上傳檔案</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                Photo photo = new Photo();
                photo.ImageNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                string[] sp = fileName.Split('.');
                string FinalFileName=string.Empty;
                if (sp.Length > 0)
                {                    
                    photo.AttachedFileName = sp[1];
                    FinalFileName = photo.ImageNo + "." + photo.AttachedFileName;
                }
                var path = Path.Combine(Server.MapPath("~/FileUploads"), FinalFileName);
                file.SaveAs(path);

                Operation Obj = new Operation();
                Obj.Method = OperationMethod.Method.Add;
                Obj.Entity = Entities.EntitiesList.Photo;
                Obj.Obj = photo;
                ApiOperation Oper = Init(Obj);
                WebApi Api = new WebApi();
                string temp = Api.ApiOperation(Oper);
            }
            return RedirectToAction("Index");
        
        }





        /// <summary>刪除動作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeletePhotoAction(WebApplication1.Models.PhotoViewModels.PhotoViewModel Model)
        {
            Photo photo = JsonConvert.DeserializeObject<Photo>(Model.PhotoJson);
            var path = Path.Combine(Server.MapPath("~/FileUploads"), photo.ImageNo+"."+photo.AttachedFileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            Model.Photo = photo;

            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Delete;
            Obj.Entity = Entities.EntitiesList.Photo;
            Obj.Obj = photo;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Oper);
            DeleteFile(photo);
            return Json(Model.Photo);
        }


        /// <summary>刪除實際檔案</summary>
        private void DeleteFile(Photo photo)
        {
            string FilePath = Path.Combine(FtpPath, photo.ImageNo + "." + photo.AttachedFileName);
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
        }

        
    }
}

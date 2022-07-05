using AjaxHW_17_MSIT141.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AjaxHW_17_MSIT141.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;

        public ApiController(DemoContext conetxt, IWebHostEnvironment hostEnvironment)
        {
            _context = conetxt;
            _host = hostEnvironment;
        }

        public IActionResult Index(User user)
        {
            var users = _context.Members.Where(m => m.Name == user.name);
            if (users.Count()==0)
            {
                return Content("");
            }
            else
                return Content("此帳號已被註冊", "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult CheckName(string name)
        {
            if (!_context.Members.Any(m => m.Name == name))
            {
                return Content("");
            }
            else 
            {
                return Content("此帳號已被註冊", "text/plain", System.Text.Encoding.UTF8);
            }
        }
        public IActionResult Register(Member member, IFormFile file)
        {
            //string info = $"{file.FileName} - {file.ContentType} - {file.Length}";
            //return Content(info, "text/plain", System.Text.Encoding.UTF8);

            //檔案上傳要有實際路徑
            //C:\俐欣\資展國際微軟班課程\13.Restful&Ajax\AjaxHW_17_MSIT141-\AjaxHW_17_MSIT141\wwwroot\uploads
            //string path = _host.ContentRootPath; //會取得專案資料夾的實際路徑
            string path = Path.Combine(_host.WebRootPath,"uploads",file.FileName); //(路徑1檔案位址,路徑2資料夾,路徑3檔名)
            using(var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);//儲存到uploads資料夾
            }

            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            member.FileName = file.FileName;
            member.FileData = imgByte;

            _context.Members.Add(member);
            _context.SaveChanges();

            string info = $"{file.FileName} - {file.ContentType} - {file.Length}";
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult City()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(a => a.City == city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }
        public IActionResult Roads(string siteid)
        {
            var roads = _context.Addresses.Where(a => a.SiteId == siteid).Select(a => a.Road).Distinct();
            return Json(roads);
        }
        public IActionResult GetImageBytes(int id = 1)
        {
            Member member = _context.Members.Find(id);
            byte[] img = member.FileData;
            return File(img, "image/jpeg");

        }
        //public IActionResult members()
        //{

        //}
        
    }
}

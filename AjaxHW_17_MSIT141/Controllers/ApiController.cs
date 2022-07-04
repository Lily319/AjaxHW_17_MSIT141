using AjaxHW_17_MSIT141.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AjaxHW_17_MSIT141.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;

        public ApiController(DemoContext conetxt)
        {
            _context = conetxt;
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
    }
}

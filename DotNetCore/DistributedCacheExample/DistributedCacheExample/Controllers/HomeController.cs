using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DistributedCacheExample.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DistributedCacheExample.Controllers
{
    public class HomeController : Controller
    {
        IDistributedCache cache;
        public HomeController(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public IActionResult Index()
        {
            //var data = cache.GetString("data");
            //if (!string.IsNullOrEmpty(data))
            //{
            //    ViewBag.Message = data;
            //}
            //else
            //{
            //    ViewBag.Message = "Nothing in cache";
            //    cache.SetString("data", "This is cache data");
            //}
            var data = cache.Get("data");
            if (data != null)
            {
                var mgs = Encoding.UTF8.GetString(data);
                ViewBag.Message = mgs;
            }
            else
            {
                ViewBag.Message = "Nothing in cache";
                var text = "This is sample text data";
                var value = Encoding.UTF8.GetBytes(text);
                cache.Set("data", value);
            }
            var msg = Encoding.UTF8.GetBytes("This is Session data");
            HttpContext.Session.Set("message", msg);
            HttpContext.Session.SetInt32("count", 150);
            HttpContext.Session.SetString("name", "Apple");

            ViewBag.Flag = HttpContext.Items["flag"].ToString();

            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Message = Encoding.UTF8.GetString(HttpContext.Session.Get("message"));
            ViewBag.Count = HttpContext.Session.GetInt32("count");
            ViewBag.Name = HttpContext.Session.GetString("name");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

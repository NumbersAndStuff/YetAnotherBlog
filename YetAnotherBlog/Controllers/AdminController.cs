using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YetAnotherBlog.Data;
using YetAnotherBlog.Models;
using YetAnotherBlog.Models.AccountViewModels;
using YetAnotherBlog.Services;

namespace YetAnotherBlog.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private IHostingEnvironment environment;

        public AdminController(ApplicationDbContext context, IHostingEnvironment _environment)
        {
            dbContext = context;
            environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<RedirectToActionResult> UploadPage(UploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("UploadPage");
            }

            foreach (var f in model.File)
            {
                FileStream target = new FileStream(Path.Combine(Hosting.WebRootPath, "static", f.FileName), FileMode.Create);
                await f.CopyToAsync(target);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<RedirectToActionResult> UploadFiles(UploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("UploadFiles");
            }

            foreach (var f in model.File)
            {
                FileStream target = new FileStream(Path.Combine(Hosting.WebRootPath, "files", f.FileName), FileMode.Create);
                await f.CopyToAsync(target);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Options()
        {
            return View();
        }
    }
}

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using YetAnotherBlog.Data;
using YetAnotherBlog.Models;

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
                FileStream target = new FileStream(Path.Combine(environment.WebRootPath, "static", f.FileName), FileMode.Create);
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
                FileStream target = new FileStream(Path.Combine(environment.WebRootPath, "files", f.FileName), FileMode.Create);
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

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using YetAnotherBlog.Data;
using YetAnotherBlog.Models;
using Newtonsoft.Json;

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

            if (model.File != null)
            {
                foreach (var file in model.File)
                {
                    if (file.Length > 0)
                    {
                        FileStream target = new FileStream(Path.Combine(environment.WebRootPath, "static", file.FileName), FileMode.Create);
                        await file.CopyToAsync(target);
                    }
                }
            }
            else
            {
                return RedirectToAction("UploadPage");
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

            if (model.File != null)
            {
                foreach (var file in model.File)
                {
                    if (file.Length > 0)
                    {
                        FileStream target = new FileStream(Path.Combine(environment.WebRootPath, "files", file.FileName), FileMode.Create);
                        await file.CopyToAsync(target);
                    }
                }
            }
            else
            {
                return RedirectToAction("UploadFiles");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Options()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Options(AdminSettings model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Options");
            }

            string optionsJSON = JsonConvert.SerializeObject(model, Formatting.Indented);
            StreamWriter optionsFile = new StreamWriter(Path.Combine(environment.WebRootPath, "json", "options.json"));

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;

            using (JsonWriter writer = new JsonTextWriter(optionsFile))
            {
                serializer.Serialize(writer, model);
            }

            return RedirectToAction("Index");
        }
    }
}

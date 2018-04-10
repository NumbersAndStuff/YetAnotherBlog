using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using YetAnotherBlog.Data;
using YetAnotherBlog.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace YetAnotherBlog.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private IHostingEnvironment environment;

        private string filePath = "files";
        private string staticPath = "static";

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
                        FileStream target = new FileStream(Path.Combine(environment.WebRootPath, staticPath, file.FileName), FileMode.Create);
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
                        FileStream target = new FileStream(Path.Combine(environment.WebRootPath, filePath, file.FileName), FileMode.Create);
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

        [HttpGet]
        public IActionResult ViewPages()
        {
            FilesViewModel pages = new FilesViewModel { Files = new List<Models.File>() };
            DirectoryInfo pagesDirectory = new DirectoryInfo(Path.Combine(environment.WebRootPath, staticPath));
            FileInfo[] files = pagesDirectory.GetFiles();

            foreach (FileInfo file in files)
            {
                Models.File f = new Models.File() { Name = file.Name.Split('.')[0], Type = file.Extension.ToUpper().Split('.')[1] };

                pages.Files.Add(f);
            }

            return View(pages);
        }

        [HttpGet]
        public IActionResult DeletePage(string file, string type)
        {
            if (System.IO.File.Exists(Path.Combine(environment.WebRootPath, staticPath, file + '.' + type)))
                System.IO.File.Delete(Path.Combine(environment.WebRootPath, staticPath, file + '.' + type));

            return RedirectToAction(nameof(ViewFiles));
        }

        [HttpGet]
        public IActionResult ViewFiles()
        {
            FilesViewModel pages = new FilesViewModel { Files = new List<Models.File>() };
            DirectoryInfo pagesDirectory = new DirectoryInfo(Path.Combine(environment.WebRootPath, filePath));
            FileInfo[] files = pagesDirectory.GetFiles();

            foreach (FileInfo file in files)
            {
                Models.File f = new Models.File() { Name = file.Name.Split('.')[0], Type = file.Extension.ToUpper().Split('.')[1] };

                pages.Files.Add(f);
            }

            return View(pages);
        }

        [HttpGet]
        public IActionResult DeleteFile(string file, string type)
        {
            if (System.IO.File.Exists(Path.Combine(environment.WebRootPath, filePath, file + '.' + type)))
                System.IO.File.Delete(Path.Combine(environment.WebRootPath, filePath, file + '.' + type));

            return RedirectToAction(nameof(ViewFiles));
        }
    }
}

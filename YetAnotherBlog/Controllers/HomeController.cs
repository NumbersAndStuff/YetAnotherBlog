using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YetAnotherBlog.Data;
using YetAnotherBlog.Models;

namespace YetAnotherBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.Posts = _context.PostModel.Take(10).ToList();
            viewModel.Posts.Sort((x, y) => -DateTime.Compare(x.TimePosted, y.TimePosted));

            /*
             * Only for development
             * TODO:
             * Get rid of this and read options from JSON
             * 
             * */
            viewModel.Options = new OptionsViewModel();
            viewModel.Options.PostsPerPage = 10;
            viewModel.Options.EnableRegistration = false;

            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "A minimalist blogging platform.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

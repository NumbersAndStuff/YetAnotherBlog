using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YetAnotherBlog.Data;
using YetAnotherBlog.Models;
using Microsoft.AspNetCore.Identity;

namespace YetAnotherBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PostModels
        [Authorize(Roles = "Admin,Poster")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostModel.ToListAsync());
        }

        // GET: PostModels/Details/5
        [Authorize(Roles = "Admin,Poster")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.PostModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Poster,Member")]
        public async Task<IActionResult> Reply(ResponseModel response)
        {
            response.DatePosted = DateTime.Now;
            response.PostedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            response.Hidden = false;

            await _context.Responses.AddAsync(response);

            // Redirect back to post
            var post = await _context.PostModel.SingleOrDefaultAsync(m => m.Id == response.ResponseTo);
            if (post == null)
            {
                return NotFound();
            }

            post.ResponseCount++;
            _context.PostModel.Update(post);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(View) + "/" + response.ResponseTo);
        }

        // GET: PostModels/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Poster")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Poster")]
        public async Task<IActionResult> Create([Bind("Id,Title,Post,Tags,AllowResponses")] PostModel postModel)
        {
            if (ModelState.IsValid)
            {
                postModel.Id = Guid.NewGuid();
                postModel.TimePosted = DateTime.Now;
                postModel.ResponseCount = 0;
                postModel.Edited = false;
                postModel.LastEdited = DateTime.Now;
                _context.Add(postModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postModel);
        }

        // GET: PostModels/Edit/5
        [Authorize(Roles = "Admin,Poster")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.PostModel.SingleOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }
            return View(postModel);
        }

        // POST: PostModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Poster")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Post,Tags,AllowResponses")] PostModel postModel)
        {
            if (id != postModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var post = await _context.PostModel.FirstOrDefaultAsync(p => p.Id == id);
                    post.Title = postModel.Title;
                    post.Post = postModel.Post;
                    post.Tags = postModel.Tags;
                    post.Edited = true;
                    post.AllowResponses = postModel.AllowResponses;
                    post.LastEdited = DateTime.Now;

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostModelExists(postModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(postModel);
        }

        // GET: PostModels/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.PostModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: PostModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var postModel = await _context.PostModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.PostModel.Remove(postModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var pageModel = new ViewPostViewModel();
            pageModel.UserResponse = new ResponseModel();

            pageModel.Post = await _context.PostModel.FirstAsync(p => p.Id == id);
            pageModel.UserResponse.ResponseTo = pageModel.Post.Id;
            pageModel.UserResponse.PostedByName = User.Identity.Name;

            // Retrieve and sort all responses
            pageModel.Responses = await _context.Responses.Where(r => r.ResponseTo.Equals(id)).ToListAsync();
            pageModel.Responses.Sort((x, y) => DateTime.Compare(x.DatePosted, y.DatePosted));

            return View(pageModel);
        }

        private bool PostModelExists(Guid id)
        {
            return _context.PostModel.Any(e => e.Id == id);
        }
    }
}

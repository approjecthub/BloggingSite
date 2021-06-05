using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mvc1.Models;

namespace mvc1.Controllers
{
    
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly BlogDataContext _db;

        public BlogController(BlogDataContext db)
        {
            _db = db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(int page = 0)
        {
            float pageSize = 2;
            float totalPosts = (float)_db.Posts.Count();
            float totalPages = (float)Math.Ceiling(totalPosts / pageSize);
            var previousPage = page - 1;
            var nextPage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;

            var posts =
                _db.Posts
                    .OrderByDescending(x => x.Posted)
                    .Skip((int)pageSize * page)
                    .Take((int)pageSize)
                    .ToArray();

            if(Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView(posts);

            return View(posts);
        }
        [AllowAnonymous]
        [Route(@"{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key) 
        {
            var post = _db.Posts.FirstOrDefault(x => x.Key == key);
            return View(post);
            
          
        }
        [Authorize]
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
                return View();

            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;
            
            //UserBlog obj = new UserBlog();
            
            //obj.User = post.Author;
            
            
            //_db.userblog.Add(obj);
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Post", "Blog", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });
        }

        
        [Authorize]
        [Route(@"delete/{id}")]
        public IActionResult Delete(long id){
            var post = _db.Posts.Find(id);
            if (post != null)
            {
                _db.Posts.Remove(post);
                _db.SaveChanges();
            }
            return RedirectToAction("index");
        }

        [Authorize]
        [Route(@"update/{id}")]
        public IActionResult Update(long id){
            var post = _db.Posts.Find(id);
            //return RedirectToAction("Edit", "Blog", post);
            return View(post);
        }


        [Authorize]
        [HttpPost, Route(@"update/{id}")]
        public IActionResult Update(Post updatedpost){
            updatedpost.Author = User.Identity.Name;
            updatedpost.Posted = DateTime.Now;
            var post = _db.Posts.Attach(updatedpost);
            post.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Post", "Blog", new
            {
                year = updatedpost.Posted.Year,
                month = updatedpost.Posted.Month,
                key = updatedpost.Key
            });
        }

        
    }
}

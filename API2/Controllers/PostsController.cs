using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API2.Data;
using API2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        //-----------------------------------------------------------------------------
        private UserManager<ApplicationUser> _usermanager;
        private readonly ApplicationDbContext _context;
        //-----------------------------------------------------------------------------
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }
        //-----------------------------------------------------------------------------
        [HttpGet]
        [Route("GetCateogriesIdName")]
        public IActionResult GetCateogriesIdName()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_context.Categories);
        }
        //-----------------------------------------------------------------------------
        [HttpGet]
        [Route("GetUsersId")]
        public IActionResult GetUsersId()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_context.Users);
        }
        //-----------------------------------------------------------------------------
        //public IActionResult GetUserid(user)
        //{
        //    _usermanager.GetUserId(User);
        //}
        //-----------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var applicationDbContext = new object();
                applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.User);
                return Ok(await ((IQueryable<Post>)applicationDbContext).ToListAsync());
            }catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------
        // GET: Posts
        [HttpGet("{id}")]
        public async Task<ActionResult> Index(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var applicationDbContext = new object();
                applicationDbContext = _context.Posts
                    .Include(n => n.Category)
                    .Include(n => n.User)
                    .Where(n => n.CategoryId == id)
                    .OrderBy(n => n.PublicationDate);

                return Ok(await ((IQueryable<Post>)applicationDbContext).ToListAsync());
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------
        // GET: Posts
        [HttpGet]
        [Route("GetPostById/{id:int}")]
        public async Task<ActionResult> GetPost(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var applicationDbContext = new object();
                var post = await _context.Posts.FindAsync(id);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            } catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------
        [HttpGet]
        [Route("GetPostDetailsById/{id:int}")]
        public async Task<ActionResult> GetPostWithRelationsItem(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var applicationDbContext = new object();
                applicationDbContext = _context.Posts
               .Include(n => n.Category)
                       .Include(n => n.User)
                       .Where(n => n.Id == id)
                       .OrderBy(n => n.PublicationDate);

                return Ok(await ((IQueryable<Post>)applicationDbContext).ToListAsync());
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------
        // GET: Posts/Create
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (post == null)
                {
                    return NotFound();
                }

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------
        // POST: Posts/Edit/5
        [HttpPut]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Put([FromBody]Post post)
        {
            if (!ModelState.IsValid)
            {
                 BadRequest(ModelState);
            }

            try
            {
                if (post == null)
                {
                    return NotFound();
                }
                 _context.Posts.Update(post);// .Entry(category).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
        //-----------------------------------------------------------------------------
        // GET: Posts/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}
        //-----------------------------------------------------------------------------
        // POST: Categories/Delete/5
        [HttpDelete("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //--------------------------------------------
                if (id == null)
                {
                    return NotFound();
                }
                //--------------------------------------------
                Post post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                //--------------------------------------------
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                //--------------------------------------------
                return Ok();
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //------------------------------------------------------------------------------
    }
}
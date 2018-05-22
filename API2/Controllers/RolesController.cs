using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API2.Data;
using API2.Models;
using Newtonsoft.Json;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        //-----------------------------------------------------------------------------------------------------------------------------------
        //[Authorize(Roles ="Admins")]
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //-----------------------------------------------------------------------------------------------------------------------------------
        public RolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        // GET: Roles
        [HttpGet]
        public  ActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_context.Roles);
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        // GET api/<controller>/5
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(String id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }
            try
            {
                IdentityRole roleObject = await _context.Roles.FindAsync(id);

                if (roleObject == null)
                {
                    return NotFound();
                }

                return Ok(roleObject);
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        // GET: Roles/Details/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(String id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id == null)
                {
                    return NotFound();
                }

                var theRole = await _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
                if (theRole == null)
                {
                    return NotFound();
                }

                return View(theRole);
                //return View();
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        // POST: Roles/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Save")]
        public async Task<ActionResult> Post([FromBody] RoleViewModel formData )
        {
            try
            {
                //----------------------------------------
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //----------------------------------------
                if (formData == null)
                {
                    return NotFound();
                }
                //----------------------------------------
                IdentityRole oneRole = new IdentityRole();
                oneRole.Name = formData.Name;
                if (!await _roleManager.RoleExistsAsync(formData.Name))
                {
                    try
                    {
                        var result = await _roleManager.CreateAsync(oneRole);


                        if (result.Succeeded)
                            return Ok();
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Error, Can not complete your request");
                    }
                }
                //----------------------------------------
                return Ok();
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        // POST: Roles/Edit/5
        [HttpPut]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Put([FromBody] RoleViewModel theRole)
        {
            try
            {
                //----------------------------------------
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //----------------------------------------
                if (theRole == null)
                {
                    return NotFound();
                }
                //----------------------------------------
                if (theRole.Id == null)
                {
                    return NotFound();
                }
                //----------------------------------------
                IdentityRole oneRole = await _context.Roles.SingleOrDefaultAsync(m => m.Id == theRole.Id);
                //----------------------------------------
                if (oneRole == null)
                {
                    return NotFound();
                }
                //----------------------------------------
                oneRole.Name = theRole.Name;
                //----------------------------------------
                _context.Roles.Update(oneRole);// .Entry(category).State=EntityState.Modified;
                await _context.SaveChangesAsync();
                //----------------------------------------
                return Ok();
                //return RedirectToAction(nameof(Index));
            }catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        // POST: Roles/Delete/5
        [HttpDelete]
        // [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(String id)
        {
            try
            {
                // TODO: Add delete logic here
                IdentityRole theRole = await _roleManager.FindByIdAsync(id); //  FindByIdAsync _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
                await _roleManager.DeleteAsync(theRole);
                return Ok();
            }
            catch
            {
                return  BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        private bool RoleExists(String id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
    }
}
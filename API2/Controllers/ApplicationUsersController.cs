using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API2.Data;
using API2_MVC_Interface.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //------------------------------------------------------------
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(_context.ApplicationUser);
            }
            //------------------------------------------------------------
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
            //------------------------------------------------------------
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
        {
            //    ([FromBody] ApplicationUser applicationUser)
            //{
            //------------------------------------------------------------
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //------------------------------------------------------------
            try
            {
                if ( model == null)
                {
                    return NotFound();
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                user.Address = "-";

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Error, Can not complete your request");
                }
                //await _context.SaveChangesAsync();

                //return Ok();
            }
            //------------------------------------------------------------
            catch(Exception e)
            {
                var e1 = e.Message;
                return BadRequest("Error, Can not complete your request");
            }
            //------------------------------------------------------------
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details (String id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var  applicationUser = await _context.ApplicationUser.FindAsync(id);
                if (applicationUser == null)
                {
                    return NotFound();
                }
                return Ok (applicationUser);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
            }

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                if (user.Address is null)
                {
                    user.Address = "-";
                }
                _context.ApplicationUser.Update(user);// .Entry(category).State=EntityState.Modified;

                await _context.SaveChangesAsync(); //.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var cc = e.Message;
                return BadRequest("Error, Can not complete your request");
            }
            return Ok();
        }

        [HttpDelete("{id}")]

        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(String id)
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
                ApplicationUser user = await _context.ApplicationUser.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                //--------------------------------------------
                _context.ApplicationUser.Remove(user);
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
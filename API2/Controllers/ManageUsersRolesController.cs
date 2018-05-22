using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [NotMapped]
    public class ManageUsersRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //=========================================================================================================
        public ManageUsersRolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //=========================================================================================================
        // GET: ManageUsersRoles
        [HttpGet]
        [Route("GetAllRoles")]
        public IList<String> GetAllRoles()
        {
            //---------------------------------------------
            //prepopulat roles for the view dropdown
            List<String> list = new List<String>();
            //---------------------------------------------
            try
            {
                //---------------------------------------------
                //Get roles order by name
                var roles = _context.Roles.OrderBy(r => r.Name);
                //---------------------------------------------
                //Fill list with roles names
                foreach (var role in roles)
                {
                    list.Add((role.Name));
                }
                //---------------------------------------------
                return list;
                //---------------------------------------------
            }
            catch
            {
                list.Add("Error, Can not complete your request");
                return list;
            }
        }
        //=========================================================================================================
        [HttpGet]
        [Route("{UserName}/{RoleName}")]
        public async Task<ActionResult> PostUserRoles(String UserName, String RoleName)
        {
            //Get the user object
            ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //We found the user
            if (user != null)
            {

                //------------------------------------------------------------------------
                //Add user object and RoleName
                try
                {
                    var result = await _userManager.AddToRoleAsync(user, RoleName);
                    if (result.Succeeded)
                    {
                        return Ok(); // ViewBag.ResultMessage = "Role removed from this user successfully !";
                    }
                    else
                    {
                        return NotFound();  //ViewBag.ResultMessage = "Role couldn't remove from this user  !";
                    }
                } catch(Exception e)
                {
                  return BadRequest(ModelState);
                }

                //------------------------------------------------------------------------

            }
            return NotFound();
        }
        //===================================================================================================
        [HttpGet]
        [Route("GetUserRoles/{UserName}")]
        public async Task<IList<String>> GetRoles(String UserName)
        {
            //List<IdentityRole> list = new List<IdentityRole>(); ;
            IList<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (user != null)
                {
                    // ViewBag.RolesForThisUser = await _userManager.GetRolesAsync(user);
                    list = await _userManager.GetRolesAsync(user);
                }
            }
            return list;
        }
        //===================================================================================================
        [HttpDelete]
        [Route("DeleteUserRole/{UserName}/{RoleName}")]
        public async Task<IActionResult> Delete(string UserName, string RoleName)
        {
            ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, RoleName))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
                    if (result.Succeeded)
                    {
                        ViewBag.ResultMessage = "Role removed from this user successfully !";
                    }
                    else
                    {
                        ViewBag.ResultMessage = "Role couldn't remove from this user  !";
                    }
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }
            }
            return Ok();
        }
        //===================================================================================================
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API2_MVC_Interface.Data;
using API2_MVC_Interface.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace API2_MVC_Interface.Controllers
{
    public class ManageUsersRolesController : Controller
    {
        StartTheConnection newConnection;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //==============================================================================================
        public   ManageUsersRolesController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _environment = environment;
            _usermanager = userManager;
            newConnection = new StartTheConnection();
        }
        public async  Task<ActionResult> ManageUsersRoles()
        {
            try
            {
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                //---------------------------------------------------------------------------------------
                //Request all the userroles 
                IEnumerable<SelectListItem> rResult = await GetAllRoleAsItemListsAsync(client);
                ViewBag.Roles = rResult;
                //---------------------------------------------------------------------------------------
                return View();
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //==============================================================================================
        // GET: ManageUsersRoles/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleAddToUser(string UserName, string RoleName)
        {
            try
            {
                //Add user and roles 
                //-------------------------------------------------------------------------------------------
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/ManageUsersRoles/{UserName}/{RoleName}");
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    IEnumerable<SelectListItem> rResult = await GetAllRoleAsItemListsAsync(client);
                    ViewBag.Roles = rResult;
                    //---------------------------------------------------------------------------------------
                    return View("ManageUsersRoles");
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //---------------------------------------------------------------------------------------
                //-------------------------------------------------------------------------------------------
                //return RedirectToAction(nameof(ManageUsersRoles));
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }


        //==============================================================================================
        public async Task<IActionResult> GetRoles(string UserName)
        {
            try
            {
                List<String> identityRoles = new List<String>();
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                //-------------------------------------------------------------------------------------------
                HttpResponseMessage res = await client.GetAsync($"api/ManageUsersRoles/GetUserRoles/{UserName}");
                //-------------------------------------------------------------------------------------------
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //----------------------------------------------------------------------------------------
                    identityRoles = DeserializingStringObject(res);
                    //Create SelectList Item

                    ViewBag.RolesForThisUser = identityRoles;
                    //-----------------------------------------------------------------------------------------
                    //Get all the Roles
                    IEnumerable<SelectListItem> rResult = await GetAllRoleAsItemListsAsync(client);
                    ViewBag.Roles = rResult;
                    //------------------------------------------------------------------------------------------
                    return View("ManageUsersRoles");
                    //-----------------------------------------------------------------------------------------
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //return RedirectToAction(nameof(ManageUsersRoles));
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //==============================================================================================
        // POST: ManageUsersRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            try
            {
                //---------------------------------------------------------------------------------------
                // prepopulat roles for the view dropdown
                // prepopulat roles for the view dropdown
                List<IdentityRole> identityRoles = new List<IdentityRole>();
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.DeleteAsync($"api/ManageUsersRoles/DeleteUserRole/{UserName}/{RoleName}");
                //---------------------------------------------------------------------------------------
                if (res.IsSuccessStatusCode)
                {
                    //Request all the userroles 
                    IEnumerable<SelectListItem> rResult = await GetAllRoleAsItemListsAsync(client);
                    ViewBag.Roles = rResult;
                    return View("ManageUsersRoles");
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //---------------------------------------------------------------------------------------
                //return RedirectToAction(nameof(ManageUsersRoles));
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //==============================================================================================
        public List<String> DeserializingStringObject(HttpResponseMessage res)
        {
            try
            {
                // prepopulat roles for the view dropdown
                List<String> stringContent = new List<String>();
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list    
                stringContent = JsonConvert.DeserializeObject<List<String>>(result);

                return stringContent;
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetAllRoleAsItemListsAsync (HttpClient client)
        {
            try
            {
                // prepopulat roles for the view dropdown
                List<String> identityRoles = new List<String>();
                //Request all the userroles 
                HttpResponseMessage res1 = await client.GetAsync($"api/ManageUsersRoles/GetAllRoles");
                identityRoles = DeserializingStringObject(res1);
                //Create SelectList Item
                IEnumerable<SelectListItem> list = identityRoles.Select(rr => new SelectListItem { Value = rr.ToString(), Text = rr.ToString() }).ToList();
                //add it to ViewBag
                return list;
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
    }
}
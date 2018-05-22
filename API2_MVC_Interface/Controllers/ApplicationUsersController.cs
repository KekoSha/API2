using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API2_MVC_Interface.Data;
using API2_MVC_Interface.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using API2_MVC_Interface.Models.AccountViewModels;

namespace API2_MVC_Interface.Controllers
{
    public class ApplicationUsersController : Controller
    {
        //----------------------------------------------------------------------------------------------------------------------
        StartTheConnection newConnection;
        private UserManager<ApplicationUser> _usermanager;
        //----------------------------------------------------------------------------------------------------------------------    
        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _usermanager = userManager;
            newConnection = new StartTheConnection();
        }        // GET: ApplicationUsers

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                //---------------------------------------------------------------------------------------
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                List<ApplicationUser> users = new List<ApplicationUser>();
                //Get all the posts to specific categories
                //---------------------------------------------------------------------------------------
                HttpResponseMessage res = await client.GetAsync($"api/ApplicationUsers");
                //---------------------------------------------------------------------------------------
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    users = JsonConvert.DeserializeObject<List<ApplicationUser>>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //returning the employee list to view    
                return View(users);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // GET: ApplicationUsers/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(String id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ApplicationUser user = new ApplicationUser();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/ApplicationUsers/Details/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<ApplicationUser>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
            //return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Email,Password,ConfirmPassword")] RegisterViewModel applicationUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = newConnection.InitializeClient();

                    var content = new StringContent(JsonConvert.SerializeObject(applicationUser), Encoding.UTF8, "application/json");

                    HttpResponseMessage res = client.PostAsync($"api/ApplicationUsers", content).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                }

                return View(applicationUser);
            }
            //------------------------------------------------------------
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
            //------------------------------------------------------------
        }

        // GET: ApplicationUsers/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(String id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ApplicationUser user = new ApplicationUser();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/ApplicationUsers/Details/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<ApplicationUser>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
            //return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Edit(String id, ApplicationUser user)
        {
                try
                {
                    //---------------------------------------------------------------------------------------------------------
                    if (id != user.Id)
                    {
                        return NotFound();
                    }
                    //---------------------------------------------------------------------------------------------------------
                    if (ModelState.IsValid)
                    {
                        HttpClient client = newConnection.InitializeClient();

                        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                        HttpResponseMessage res = client.PutAsync("api/ApplicationUsers", content).Result;
                        //-----------------------------------------------------------------------------------------------------
                        if (res.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            throw new Exception(res.ReasonPhrase);
                        }
                        //-----------------------------------------------------------------------------------------------------
                    }
                    //---------------------------------------------------------------------------------------------------------
                    return View(user);
                }
                catch
                {
                    throw new Exception("Can't complete the process");
                }
        }

        [HttpGet]
        // GET: ApplicationUsers/Delete/5
        public async  Task<ActionResult> Delete(String id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ApplicationUser user = new ApplicationUser();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/ApplicationUsers/Details/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<ApplicationUser>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
            //return View(applicationUser);
        }

            // POST: ApplicationUsers/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(String id, IFormCollection collection)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = client.DeleteAsync($"api/ApplicationUsers/{id}").Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
    }
}
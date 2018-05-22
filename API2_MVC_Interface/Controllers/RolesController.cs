using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API2_MVC_Interface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API2_MVC_Interface.Controllers
{
    public class RolesController : Controller
    {
        StartTheConnection newConnection = new StartTheConnection();
        //------------------------------------------------------------------------------------------------------
        // GET: Roles
        public async Task<ActionResult> Index()
        {
            try
            {
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                HttpResponseMessage res = await client.GetAsync("api/Roles");

                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    roles = JsonConvert.DeserializeObject<List<RoleViewModel>>(result);

                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //returning the employee list to view    
                return View(roles);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //------------------------------------------------------------------------------------------------------
        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                RoleViewModel roleObject = new RoleViewModel();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/Roles/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    roleObject = JsonConvert.DeserializeObject<RoleViewModel>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (roleObject == null)
                {
                    return NotFound();
                }

                return View(roleObject);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name")]RoleViewModel roleObject)
        {
            try
            {
                String Name = roleObject.Name;

                HttpClient client = newConnection.InitializeClient();

                var content = new StringContent(JsonConvert.SerializeObject(roleObject), Encoding.UTF8, "application/Json");

                HttpResponseMessage res = client.PostAsync($"api/Roles/Save", content).Result;

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                //return View(roleObject);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        
        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                RoleViewModel roleObject = new RoleViewModel();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/Roles/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    roleObject = JsonConvert.DeserializeObject<RoleViewModel>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (roleObject == null)
                {
                    return NotFound();
                }

                return View(roleObject);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // POST: Roles/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind("Id,Name")]RoleViewModel roleObject)
        {
            try
            {
                String Name = roleObject.Name;

                HttpClient client = newConnection.InitializeClient();

                var content = new StringContent(JsonConvert.SerializeObject(roleObject), Encoding.UTF8, "application/Json");

                HttpResponseMessage res = client.PutAsync($"api/Roles", content).Result;

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
            //return View(roleObject);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                RoleViewModel roleObject = new RoleViewModel();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/Roles/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    roleObject = JsonConvert.DeserializeObject<RoleViewModel>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (roleObject == null)
                {
                    return NotFound();
                }

                return View(roleObject);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // POST: Roles/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(string id,RoleViewModel roleObject )
        {
            try
            {
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = client.DeleteAsync($"api/Roles/Delete/{id}").Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                // return NotFound();             
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
    }
}
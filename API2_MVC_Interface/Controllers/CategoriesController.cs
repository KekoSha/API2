using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API2_MVC_Interface.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace API2_MVC_Interface.Controllers
{
    public class CategoriesController : Controller
    {
        //----------------------------------------------------------------------------------------------------------------------
        StartTheConnection newConnection = new StartTheConnection();
        //----------------------------------------------------------------------------------------------------------------------        
        // GET: Categories
        public async Task<ActionResult> Index()
        {
            try
            {
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                List<Category> categories = new List<Category>();
                HttpResponseMessage res = await client.GetAsync("api/Categories");

                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    categories = JsonConvert.DeserializeObject<List<Category>>(result);

                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //returning the employee list to view    
                return View(categories);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Category category = new Category();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/Categories/Details/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    category = JsonConvert.DeserializeObject<Category>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------------------------------
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Description")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = newConnection.InitializeClient();

                    var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                    HttpResponseMessage res = client.PostAsync("api/Categories", content).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                }
                return View(category);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("Id,Name,Description")]Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    HttpClient client = newConnection.InitializeClient();

                    var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                    HttpResponseMessage res = client.PutAsync("api/Categories", content).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                }
                return View(category);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // GET: Students/Edit/1  
        public async Task<IActionResult> Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Category category = new Category();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/Categories/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    category = JsonConvert.DeserializeObject<Category>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Category category = new Category();
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/Categories/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    category = JsonConvert.DeserializeObject<Category>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = client.DeleteAsync($"api/Categories/{id}").Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

                //return NotFound();
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void AddErrors(IdentityResult result)
        {
            try
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
    }
}
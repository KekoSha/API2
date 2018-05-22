using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API2_MVC_Interface.Data;
using API2_MVC_Interface.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCore2_With_Auth.Helpers;
using Newtonsoft.Json;

namespace API2_MVC_Interface.Controllers
{
    public class PostsController : Controller
    {
        //----------------------------------------------------------------------------------------------------------------------
        StartTheConnection newConnection;
        private IHostingEnvironment _environment;
        private UserManager<ApplicationUser> _usermanager;
        //----------------------------------------------------------------------------------------------------------------------    
        public PostsController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _environment = environment;
            _usermanager = userManager;
            newConnection = new StartTheConnection();
        }
        // GET: Posts
        [HttpGet]
        public async Task<ActionResult> Index(int? id)
        {
            try
            {
                //---------------------------------------------------------------------------------------
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                List<Post> posts = new List<Post>();
                if (id > 0)
                {
                    //Get all the Posts
                    //---------------------------------------------------------------------------------------
                    HttpResponseMessage res = await client.GetAsync($"api/Posts/{id}");
                    //---------------------------------------------------------------------------------------
                    //Checking the response is successful or not which is sent using HttpClient    
                    if (res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api     
                        var result = res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list    
                        posts = JsonConvert.DeserializeObject<List<Post>>(result);
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                }
                else
                {
                    //Get all the posts to specific categories
                    //---------------------------------------------------------------------------------------
                    HttpResponseMessage res = await client.GetAsync($"api/Posts");
                    //---------------------------------------------------------------------------------------
                    //Checking the response is successful or not which is sent using HttpClient    
                    if (res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api     
                        var result = res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list    
                        posts = JsonConvert.DeserializeObject<List<Post>>(result);
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                }
                //---------------------------------------------------------------------------------------
                //returning the employee list to view    
                return View(posts);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // GET: Posts/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                HttpClient client = newConnection.InitializeClient();
                List<Post> posts = new List<Post>();

                //Get all the Posts
                //---------------------------------------------------------------------------------------
                HttpResponseMessage res = await client.GetAsync($"api/Posts/GetPostDetailsById/{id}");
                //---------------------------------------------------------------------------------------
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    posts = JsonConvert.DeserializeObject<List<Post>>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //---------------------------------------------------------------------------------------
                return View(posts);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // GET: Posts/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                HttpClient client = newConnection.InitializeClient();
                var result = "";
                //--------------------------------------------------------------------------------
                HttpResponseMessage res = await client.GetAsync("api/Posts/GetCateogriesIdName");
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(result);
                    ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", 0);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //--------------------------------------------------------------------------------
                res = await client.GetAsync("api/Posts/GetUsersId");
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    result = res.Content.ReadAsStringAsync().Result;
                    List<ApplicationUser> users = JsonConvert.DeserializeObject<List<ApplicationUser>>(result);
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    ViewData["UserId"] = new SelectList(users, "Id", "Id", 0);
                    //--------------------------------------------------------------------------------
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                return View();
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create([Bind("Id,CategoryId,Subject,Body,AutherName,ImageUrl")] Post post, IFormFile myfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //----------------------------------------------------------------------------------------------------
                    post.ImageUrl = await UserFile.UploadeNewImageAsync(post.ImageUrl, myfile, _environment.WebRootPath, Properties.Resources.ImgFolder, 100, 100);
                    post.PublicationDate = DateTime.Today.Date;
                    //----------------------------------------------------------------------------------------------------
                    HttpClient client = newConnection.InitializeClient();
                    //----------------------------------------------------------------------------------------------------
                    var content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
                    //----------------------------------------------------------------------------------------------------
                    HttpResponseMessage res = client.PostAsync("api/Posts", content).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                    //----------------------------------------------------------------------------------------------------
                }

                return View(post);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        // GET: Posts/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                Post postObject = new Post();
                HttpClient client = newConnection.InitializeClient();
                //--------------------------------------------------------------------------------
                //Get Post by id
                HttpResponseMessage res = await client.GetAsync($"api/Posts/GetPostById/{id}");

                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    postObject = JsonConvert.DeserializeObject<Post>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //--------------------------------------------------------------------------------
                //Get the category name 
                res = await client.GetAsync("api/Posts/GetCateogriesIdName");
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(result);
                    ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", 0);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //--------------------------------------------------------------------------------
                res = await client.GetAsync("api/Posts/GetUsersId");
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    List<ApplicationUser> users = JsonConvert.DeserializeObject<List<ApplicationUser>>(result);
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    ViewData["UserId"] = new SelectList(users, "Id", "Id", 0);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //--------------------------------------------------------------------------------
                return View(postObject);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }

        //// POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                if (id != post.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    HttpClient client = newConnection.InitializeClient();

                    var content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

                    HttpResponseMessage res = client.PutAsync("api/Posts", content).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw new Exception(res.ReasonPhrase);
                    }
                }
                return View(post);
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

                HttpClient client = newConnection.InitializeClient();
                List<Post> posts = new List<Post>();

                //Get all the Posts
                //---------------------------------------------------------------------------------------
                HttpResponseMessage res = await client.GetAsync($"api/Posts/GetPostDetailsById/{id}");
                //---------------------------------------------------------------------------------------
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    posts = JsonConvert.DeserializeObject<List<Post>>(result);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //--------------------------------------------------------------------------------
                return View(posts);
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        // POST: Posts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                HttpClient client = newConnection.InitializeClient();
                HttpResponseMessage res = client.DeleteAsync($"api/Posts/{id}").Result;
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
    }
}
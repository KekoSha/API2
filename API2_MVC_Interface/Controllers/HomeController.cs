using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API2_MVC_Interface.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace API2_MVC_Interface.Controllers
{
    public class HomeController : Controller
    {
        StartTheConnection newConnection = new StartTheConnection();
        public async Task<IActionResult> Index()
        {
            try
            {
                //----------------------------------------------------------------------------------------------
                //Open the connection with the Restful
                HttpClient client = newConnection.InitializeClient();
                List<Post> posts = new List<Post>();
                HttpResponseMessage res = await client.GetAsync("api/Home/GetAllPosts");
                //----------------------------------------------------------------------------------------------
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    posts = JsonConvert.DeserializeObject<List<Post>>(result);

                }
                //----------------------------------------------------------------------------------------------
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //----------------------------------------------------------------------------------------------
                //returning the employee list to view    
                return View(posts);
                //----------------------------------------------------------------------------------------------
            }
            catch
            {
                throw new Exception("Can't complete the process");
            }
            return View();
        }

        public async Task<ActionResult> Details(int? id)
        {
            //------------------------------------------------
            try
            {
                //---------------------------------------------------------------------------------------
                if (id == null)
                {
                    return NotFound();
                }
                //---------------------------------------------------------------------------------------
                HttpClient client = newConnection.InitializeClient();
                Post post = new Post();
                //---------------------------------------------------------------------------------------
                //Get all the Posts
                //---------------------------------------------------------------------------------------
                HttpResponseMessage res = await client.GetAsync($"api/Home/GetPostbyId/{id}");
                //---------------------------------------------------------------------------------------
                //Checking the response is successful or not which is sent using HttpClient    
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api     
                    var result = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list    
                    post = JsonConvert.DeserializeObject<Post>(result);

                    if (post == null)
                    {
                        return NotFound();
                    }
                    return View(post);
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
                //---------------------------------------------------------------------------------------
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

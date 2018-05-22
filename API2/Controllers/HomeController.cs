using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API2.Data;
using API2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        //-----------------------------------------------------------------------------------------------------------------------------------
        //[Authorize(Roles ="Admins")]
        private readonly ApplicationDbContext _context;
        //-----------------------------------------------------------------------------------------------------------------------------------
        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }
        [HttpGet]
        [Route("GetAllPosts")]
        public IActionResult Get()
        {

            return Ok( _context.Posts);
            //return View();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        [Route("GetPostbyId/{id}")]
        public async Task<IActionResult> GetPostbyId(int? id)
        {
            //------------------------------------------------
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //------------------------------------------------
            try
            {
                //--------------------------------------------
                if (id == null)
                {
                    return NotFound();
                }
                //--------------------------------------------
                var applicationDbContext = new object();
                var post = await _context.Posts.FindAsync(id);
                //--------------------------------------------
                if (post == null)
                {
                    return NotFound();
                }
                //-------------------------------------------
                updateDB(id);
                //--------------------------------------------
                return Ok(post);
                //--------------------------------------------
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public IActionResult updateDB (int? id)
        {
            try
            {
                //------------------------------------------
                //-Update the db, Increase the reader number
                var con = _context.Database.GetDbConnection();
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "IncreaseReader";
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("Id", id));
                comm.ExecuteNonQuery();
                con.Close();
                //------------------------------------------
                return Ok();
                //------------------------------------------
            }
            catch (Exception e)
            {
                //------------------------------------------
                return BadRequest("Error, Can not complete your request");
                //------------------------------------------
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
    }
}
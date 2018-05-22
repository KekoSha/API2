using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API2.Data;
using API2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

using Microsoft.EntityFrameworkCore;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API2.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        //------------------------------------------------------------------------------------------
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------------------------
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_context.Categories);
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //------------------------------------------------------------------------------------------
        // GET api/<controller>/5
        [HttpGet("{id}")]
        
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        //------------------------------------------------------------------------------------------
        // GET: Categories/Details/5
        [HttpGet]
        [Route("Details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        //------------------------------------------------------------------------------------------
        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategory", new { id = category.Id }, category);
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
        }
        //------------------------------------------------------------------------------------------
        // PUT api/<controller>/5
        [HttpPut]
        public async Task<ActionResult> Put([FromBody]Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _context.Categories.Update(category);// .Entry(category).State=EntityState.Modified;

                await _context.SaveChangesAsync(); //.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
            return Ok();
        }
        //------------------------------------------------------------------------------------------
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _context.Categories.Remove(category);
                 await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Error, Can not complete your request");
            }
            return Ok();
        }
        //------------------------------------------------------------------------------------------
        public bool CategoryExists(int? id)
        {
            if (id == null)
            {
                return false;
            }

            return _context.Categories.Any(e => e.Id == id);
        }
        //------------------------------------------------------------------------------------------

    }
}

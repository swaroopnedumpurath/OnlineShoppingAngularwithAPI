using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.API.Data;

namespace OnlineShopping.API.Controllers
{
     //http://localhost:5000/api/country
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly DataContext _context;
        public CountryController(DataContext context)
        {
            _context = context;

        }
        // GET api/country
        [HttpGet]        
        [AllowAnonymous]
        public async Task<IActionResult> GetCountries()
        {
            var countries=await _context.tbl_Country.ToListAsync();
            return Ok(countries);
        }

        [AllowAnonymous]
        // GET api/country/4
        [HttpGet("{id}")]        
        public async Task<IActionResult> GetCoutry(int id)
        {
            var country=await _context.tbl_Country.FirstOrDefaultAsync(x=>x.Id==id);
            return Ok(country);
        }

        // POST api/country
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/country/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/country/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
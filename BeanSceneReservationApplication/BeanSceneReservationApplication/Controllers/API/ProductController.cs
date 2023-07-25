using BeanSceneReservationApplication.Models.APIModel;
using BeanSceneReservationApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeanSceneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDBService _mongoDBService;

        public ProductController(ProductDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Product>> Get()
        {
            return await _mongoDBService.GetAsync();
        }

        // Sets route as /api/product/x -- then requests for a Name and based on name it
        // pulls up the data info on the products db.
        // Useful for later when we assign orders to products
        [HttpGet("{name}")]
        public async Task<ActionResult<Product>> Get(string name)
        {
            var product = await _mongoDBService.GetAsync(name);
            if(product is null)
            {
                return NotFound();
            }
            return product;
        }

        // GET based on Sitting

        [HttpGet("sitting/{sitting}")]

        public async Task<ActionResult<IEnumerable<Product>>> GetSitting(string sitting)
        {
            return await _mongoDBService.GetAsyncSitting(sitting);
        }

        // Creating data
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await _mongoDBService.CreateAsync(product);
            return Ok(product);
        }

        //Editing Data

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] string name)
        {
            await _mongoDBService.UpdateAsync(id, name);
            return Ok();
        }

        //Delete data
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mongoDBService.DeleteAsync(id);
            return Ok();
        }
    }
}


//orderDB {

//table,
//timestamp,
//items[{name, ~price, qty}{ 2}
//{ 3}
//...],

//}






//return< List > Object for fetching Menu

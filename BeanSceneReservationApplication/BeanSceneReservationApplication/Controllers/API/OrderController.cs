using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.APIModel;
using BeanSceneReservationApplication.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDBService _mongoDBService;

        public OrderController(OrderDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order order) 
        {
            await _mongoDBService.CreateAsync(order);
            return Ok();
        }


    }
}
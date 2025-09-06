using efcoreApi.BasketService;
using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Claims;

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly efContext dbContext;
        IBasketService basketService;
        IOrderService orderService;
        public OrderController(efContext _dbContext,IBasketService _basketService, IOrderService _orderService)
        {
            dbContext = _dbContext;
            basketService = _basketService;
            orderService = _orderService;
        }
        [HttpGet("Orders")]
        public async Task<IActionResult> Get([FromQuery] GoodsParameters goodsParameters)
        {
            var orders =await orderService.OrdersAsync();
            return Ok(orders);
        }
        [HttpGet("OrderById/{Id}")]
        public async Task<IActionResult> GetById( string Id)
        {
            var order = await orderService.OrderByIdAsync(Id);
            return Ok(order);
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> PostAsync()
        {
            try
            {
                var context = HttpContext.Request.HttpContext;
              var order=  await orderService.ProcessOrderAsync(context);
                return Ok(order);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet("UpdateOrder/{Id}")]
        public async Task<IActionResult> PutAsync(string Id)
        {
            try
            {
               await orderService.UpdateOrderAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("CheckOut")]
        public async Task<IActionResult> CheckOrderAsync()
        {
            var existUser = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var context = HttpContext.Request.HttpContext;

           var basket= basketService.GetBasket(context,false);
          var basketitems=await  dbContext.basketItems.Include(p=>p.Goods).Where(p=>p.BasketId==basket.Result.BasketId&&p.RegisterId.Equals(existUser)).ToListAsync();
            if (basketitems != null)
            {

                return Ok(basketitems);
            }
            return BadRequest();
        }

    }
}

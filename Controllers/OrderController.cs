using efcoreApi.BasketService;
using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Models.VM;
using efcoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Stripe;
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
       private readonly IDistributedCache distributedCache;
        public OrderController(efContext _dbContext,IBasketService _basketService, IOrderService _orderService, IDistributedCache _distributedCache)
        {
            dbContext = _dbContext;
            basketService = _basketService;
            orderService = _orderService;
            distributedCache = _distributedCache;
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

        [HttpGet("OrderDistributedCache")]
        public async Task<IActionResult> OrderDistributedCache()
        {
            var cachedData = await distributedCache.GetStringAsync("DistributedOrder");
            if (cachedData != null) {
                return Ok(JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(cachedData));
            }
            var expirationTime = TimeSpan.FromMinutes(5.0);
            var orders = await dbContext.Order.Select(g=>new OrderDto {Id=g.Id,FirstName=g.FirstName,Surname=g.Surname,
                OrderDateTime=g.OrderDateTime,OrderStatus=g.OrderStatus,OrdSeq=g.OrdSeq
                ,RegisterId=g.RegisterId,Username=g.Username
            }).ToListAsync();
            //This avoids circular references because DTOs don’t contain navigation properties.
            cachedData =JsonConvert.SerializeObject(orders);
            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expirationTime);
            await distributedCache.SetStringAsync("DistributedOrder", cachedData, cacheOptions);
                
            return Ok(orders);
        }
        [HttpGet("OrderDistributedCache2")]
        public async Task<IActionResult> OrderDistributedCache2()
        {
            var cachedData = await distributedCache.GetStringAsync("DistributedOrder2");
            if (cachedData != null)
            {
                return Ok(JsonConvert.DeserializeObject<IEnumerable<Orders>>(cachedData));
            }
            var expirationTime = TimeSpan.FromMinutes(5.0);
            //var orders = await dbContext.Order.Include(s => s.OrderItems).ThenInclude(g => g.Goods)
            //    .Where(p => p.Id == string.)
            //    .Select(p => new OrderDto
            //    {
            //        FirstName = p.FirstName,
            //        OrderDateTime = p.OrderDateTime,
            //        OrderStatus = p.OrderStatus,
            //        OrdSeq = p.OrdSeq
            //        , RegisterId = p.RegisterId,
            //        Username = p.Username,
            //        Surname = p.Surname,
            //        OrderItemsDto = it.
            //        o = p.OrderItems.Select(new OrderItemDto { GoodsId = i.})
            //    }));
            var orders = (from o in dbContext.Order
                          join i in dbContext.OrderItems
                         on o.Id equals i.OrderId
                          select new OrderDto
                          {
                              FirstName = o.FirstName,
                              Id = o.Id,
                              Surname = o.Surname,
                              OrderDateTime = o.OrderDateTime,
                              OrderStatus = o.OrderStatus,
                              OrdSeq = o.OrdSeq,
                              Username = o.Username,
                              RegisterId = o.RegisterId,
                              OrderItemsDto = new List<OrderItemDto>
                           {
                               new OrderItemDto{GoodsId=i.GoodsId,GoodName=i.Goods.GoodsName,Quantity=i.Quantity,OrderId=i.OrderId}
                           }
                          });
             //select new OrderItemDto { GoodsId = i.GoodsId,GoodName=i.Goods.GoodsName, OrderId = i.OrderId, Quantity = i.Quantity });
             //This avoids circular references because DTOs don’t contain navigation properties.
             cachedData = JsonConvert.SerializeObject(orders);
            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expirationTime);
            await distributedCache.SetStringAsync("DistributedOrder2", cachedData, cacheOptions);

            return Ok(orders);
        }
        [HttpGet("OrderResponseCache")]
       // [ResponseCache(Location = ResponseCacheLocation.Any,Duration=10000)]
        public async Task<IActionResult> OrderResponseCache()
        {
          
            var orders = await dbContext.Order.ToListAsync();
        

            return Ok(orders);
        }
    }
}

using efcoreApi.BasketService;
using efcoreApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IBasketService basketService;
        efContext dbContext;
        IOrderService orderService;
        IConfiguration configuration;
        public PaymentController(IBasketService _basketService, efContext _dbContext, IOrderService _orderService, IConfiguration _configuration)
        {
            basketService = _basketService;
            dbContext = _dbContext;
            orderService = _orderService;
            configuration = _configuration;
        }

        [HttpPost("create-checkout-session/{Id}")]
        public IActionResult CreateCheckoutSession(string Id)
        {
            StripeConfiguration.ApiKey =configuration["Stripe:StripeKey"]; // secret key

            //var context = HttpContext.Request.HttpContext;

            //var basket = basketService.GetBasket(HttpContext.Request.HttpContext,false);
            //var basketItem = dbContext.basketItems.Include(p=>p.Goods).Where(p => p.BasketId == basket.BasketId).ToList();
            var order = orderService.OrderByIdAsync(Id);
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in order.Result.OrderItems)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Goods.Price*100,//(long) item.Goods.Price,
                        Currency = "sar",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Goods.GoodsName,
                        },
                    },
                    Quantity = item.Quantity,
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/success?Id=" + order.Result.Id,
                CancelUrl = "http://localhost:3000/cancel",
            };
          

            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new { url = session.Url });
        }
    }
}

using efcoreApi.Data;
using efcoreApi.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ess;
using System.Security.Claims;

namespace efcoreApi.BasketService
{
    public class OrderService : IOrderService
    {
        efContext dbContext;
        IBasketService basketService;
        public OrderService(efContext _dbContext,IBasketService _basketService)
        {
            dbContext = _dbContext;
            basketService = _basketService;
        }

        public async Task<List<Orders>> OrdersAsync() {
          var orders=await  dbContext.Order.Include(p=>p.OrderItems).OrderBy(p=>p.OrderDateTime).ToListAsync();

            return orders;
        }

        public async Task<Orders> OrderByIdAsync(string Id)
        {
            var order = await dbContext.Order.Include(p => p.OrderItems).Where(p=>p.Id.Equals(Id)).OrderBy(p => p.OrderDateTime).FirstOrDefaultAsync();

            return order;
        }

        public async Task<Orders> ProcessOrderAsync(HttpContext httpContext)
        {
            var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var basket = basketService.GetBasket(httpContext,false);
            var basketItems = await dbContext.basketItems.Where(p => p.RegisterId.Equals(existUser)&&p.BasketId==basket.Result.BasketId).ToListAsync();
            Orders order = new Orders();
            if (basketItems != null)
            {
                
                order.RegisterId = existUser;
                var orderItems = new List<OrderItems>();
                foreach (var item in basketItems)
                {
                    orderItems.Add(new OrderItems()
                    {
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        GoodsId = item.GoodsId,
                    });
                }
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await dbContext.Order.AddAsync(order);
                        await dbContext.OrderItems.AddRangeAsync(orderItems);
                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                     //  await basketService.ClearBasket(httpContext);
                       
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                       


                    }
                }

                return order;
            }

            return new Orders();
        }

        public async Task UpdateOrderAsync(string Id)
        {
         var orderToUpdate= await dbContext.Order.Where(p=>p.Id==Id).FirstOrDefaultAsync();
            if (orderToUpdate != null)
            {
                orderToUpdate.OrderStatus = "Paid";
                await dbContext.SaveChangesAsync();
            }
        
        }
        }
}

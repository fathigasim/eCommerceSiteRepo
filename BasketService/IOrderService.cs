using efcoreApi.Models;

namespace efcoreApi.BasketService
{
    public interface IOrderService
    {
        Task<List<Orders>> OrdersAsync();
        Task<Orders> OrderByIdAsync(string Id);
     Task <Orders> ProcessOrderAsync(HttpContext httpContext);
        Task UpdateOrderAsync(string Id);
    }
}

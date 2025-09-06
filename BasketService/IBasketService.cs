using efcoreApi.Models;
using efcoreApi.Models.VM;

namespace efcoreApi.BasketService
{
    public interface IBasketService
    {
        Task AddBasket(HttpContext httpContext, int prodId,int inputQnt);
       Task  <BasketSummeryViewModel> BasketSummary(HttpContext httpContext);
       Task< Basket> GetBasket(HttpContext httpContext, bool createifnull);
       Task< List<BasketItemViewModel>> GetBasketItems(HttpContext httpContext);
        Task removefrombasket(HttpContext httpContext, int prodId);
        Task removeAllfrombasket(HttpContext httpContext);
        Task ClearBasket(HttpContext httpContext);

    }
}

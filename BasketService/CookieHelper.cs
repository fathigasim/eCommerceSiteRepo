using efcoreApi.Models;

namespace efcoreApi.BasketService
{
    public static class CookieHelper
    {
        //public static List<BasketItem> GetBasket(HttpContext context)
        //{
        //    var cookie = context.Request.Cookies["basket"];
        //    if (string.IsNullOrEmpty(cookie))
        //        return new List<BasketItem>();

        //    return System.Text.Json.JsonSerializer.Deserialize<List<BasketItem>>(cookie);
        //}

        //public static void SaveBasket(HttpContext context, List<BasketItem> basket)
        //{
        //    var options = new CookieOptions
        //    {
        //        Expires = DateTime.Now.AddDays(7),
        //        HttpOnly = false, // frontend needs to read sometimes
        //        Secure = true,
        //        SameSite = SameSiteMode.None
        //    };

        //    var json = System.Text.Json.JsonSerializer.Serialize(basket);
        //    context.Response.Cookies.Append("basket", json, options);
        //}
    }

}

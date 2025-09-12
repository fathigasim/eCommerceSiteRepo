using Castle.Core.Internal;
using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Models.VM;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace efcoreApi.BasketService
{
    public class BasketService : IBasketService
    {
        
        efContext goodsContext;
        public const string BasketSessionName = "ShoppingCommerceBasket";
        
        public BasketService(efContext _goodsContext)
        {
            //httpContext = _httpContext;
           goodsContext = _goodsContext;
        }

        public async Task<Basket> GetBasket(HttpContext httpContext, bool createifnull)
        {
           
           // var userClaim = "4a14066-a0";//httpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var cookie = httpContext.Request.Cookies[BasketSessionName];
            Basket basket = new Basket();
            if (cookie != null)
            {
                string BaskId = cookie;
                if (!string.IsNullOrEmpty(BaskId))
                {


                    basket=await goodsContext.basket.Include(p=>p.BasketItems).Where(p => p.BasketId.Equals(BaskId)).FirstOrDefaultAsync();
                    //basket = goodsContext.basket.Where(p => p.BasketId == BaskId).FirstOrDefault();
                }
                else
                {
                    if (createifnull)
                    {
                        basket = createnewbasket(httpContext);

                    }
                }
            }
            else
            {
                if (createifnull)
                {
                    basket = createnewbasket(httpContext);

                }
                return basket;
            }
            return basket;
        }

        private Basket createnewbasket(HttpContext httpContext)
        {
            Basket basket = new Basket();
            goodsContext.basket.Add(basket);
            goodsContext.SaveChanges();
            //basketRepo.Insert(basket);
            //basketRepo.Commit();

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.HttpOnly = false;
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            cookieOptions.Secure = true;
            cookieOptions.SameSite = SameSiteMode.None;
            httpContext.Response.Cookies.Append(BasketSessionName, basket.BasketId, cookieOptions);
            return basket;
        }

        public async Task AddBasket(HttpContext httpContext, int prodId, int inputQnt=0)
        {
            var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Basket basket =await GetBasket(httpContext, true);
        
                var basketItems = basket.BasketItems.Where(p => p.BasketId == basket.BasketId && p.GoodsId == prodId
            && p.RegisterId == existUser
            ).FirstOrDefault();
            if (basketItems == null)
            {
                if (inputQnt == 0) {
                    basketItems = new BasketItems()
                    {
                        BasketId = basket.BasketId,
                        GoodsId = prodId,
                        Quantity = 1,
                        RegisterId = existUser
                    };
                    }
                else
                {
                    basketItems = new BasketItems()
                    {
                        BasketId = basket.BasketId,
                        GoodsId = prodId,
                        Quantity =int.Parse(inputQnt.ToString()),
                        RegisterId = existUser
                    };
                }
                basket.BasketItems.Add(basketItems);
                //implementing Checkup Service
                //try
                //{
                //    if (string.IsNullOrEmpty(inputQnt))
                //    {
                //        procedureService.CheckQantity(basketItems.productId);
                //    }
                //    else
                //    {
                //        procedureService.CheckQantityInput(basketItems.productId, inputQnt);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message);
                //}
                //
                goodsContext.SaveChanges();




            }
            else
            {
                if (inputQnt!=0)
                {
                    basketItems.Quantity = basketItems.Quantity + inputQnt;
                }
                else
                {
                    basketItems.Quantity = basketItems.Quantity + 1;
                }
                #region CheckupProcedure
                //try
                //{
                //    if (string.IsNullOrEmpty(inputQnt))
                //    {
                //        procedureService.CheckQantity(basketItems.productId);
                //    }
                //    else
                //    {
                //        procedureService.CheckQantityInput(basketItems.productId, inputQnt);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message);
                //}
                #endregion
                goodsContext.SaveChanges();
            }
        }

        public async Task removefrombasket(HttpContext httpContext, int Id)
        {
            var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Basket basket =await GetBasket(httpContext, true);
            //BasketItems item = ItemsRepo.Query().Where(p => p.BasketId == basket.BasketId && p.productId == Id
            //&& p.UserAuthsId == int.Parse(httpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value)).FirstOrDefault();
            BasketItems item =await goodsContext.basketItems.Where(p => p.BasketId == basket.BasketId && p.GoodsId == Id
          && p.RegisterId ==existUser ).FirstOrDefaultAsync();

            if (item != null)
            {
                item.Quantity = item.Quantity - 1;
                if (item.Quantity == 0)
                {
                    goodsContext.basketItems.Remove(item);
                   // ItemsRepo.Delete(item);
                }
                else
                {
                    goodsContext.basketItems.Update(item);
                   // ItemsRepo.Update(item);
                }
                goodsContext.SaveChanges();
                //basketRepo.Commit();
               // procedureService.CheckRemoveQantity(item.productId);
            }
        }


        public async Task removeAllfrombasket(HttpContext httpContext)

        {
            var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Basket basket =await GetBasket(httpContext, true);
            //var itemstodelete = ItemsRepo.Query().Where(p => p.BasketId == basket.BasketId
            //&& p.UserAuthsId == int.Parse(httpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value)).ToList();
            var itemstodelete = goodsContext.basketItems.Where(p => p.BasketId == basket.BasketId
            && p.RegisterId == existUser).ToList();
            //foreach (var item in itemstodelete)
            //{
            //    procedureService.CheckRemoveAllQantity(item.productId);
            //}
             goodsContext.basketItems.RemoveRange(itemstodelete);
            // ItemsRepo.DeleteAll(itemstodelete);
            goodsContext.SaveChanges();
           // basketRepo.Commit();
        }

        public async Task<  List<BasketItemViewModel>> GetBasketItems(HttpContext httpContext)
        {
            var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Basket basket =await GetBasket(httpContext, false);
            if (basket != null)
            {

                //var mo = (from b in basketRepo.Query()
                //          join i in ItemsRepo.Query()
                //          on b.BasketId equals i.BasketId
                //          join pr in productRepo.Query()
                //          on i.productId equals pr.ProductId
                //          where i.UserAuthsId == int.Parse(httpContext.User.Claims.FirstOrDefault(x => x.Type
                //              == System.Security.Claims.ClaimTypes.NameIdentifier).Value)
                var mo = (from b in goodsContext.basket
                          join i in goodsContext.basketItems
                          on b.BasketId equals i.BasketId
                          join pr in goodsContext.goods
                          on i.GoodsId equals pr.GoodsId
                          where i.RegisterId == existUser
                          select new BasketItemViewModel()
                          {
                              //Id = i.BasketitemId,
                              Id = i.GoodsId,
                              Productname = i.Goods.GoodsName,
                              Price = pr.Price,
                              Image = pr.Imgpath,
                              Quantity = i.Quantity
                          }

                        ).ToList();

                return mo;
            }

            return new List<BasketItemViewModel>();
        }

        public async Task< BasketSummeryViewModel> BasketSummary(HttpContext httpContext)
        {
            var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Basket basket =await GetBasket(httpContext, false);
            BasketSummeryViewModel model = new BasketSummeryViewModel(0, 0);
            if (basket != null)
            {
              //  var uid = int.Parse(httpContext.User.Claims.FirstOrDefault(p => p.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                //  int? basketCount = (from item in basket.BasketItems select item.Quantity).Sum();
                //int? basketCount = ItemsRepo.Query().Where(p => p.BasketId == basket.BasketId && p.UserAuthsId == uid).Select(p => p.Quantity).Sum();
                int? basketCount = goodsContext.basketItems.Where(p => p.BasketId == basket.BasketId && p.RegisterId == existUser).Select(p => p.Quantity).Sum();
                //decimal? basketTotal = (from item in basket.BasketItems
                //                        join
                //                        p in productRepo.Query() on item.productId equals p.ProductId
                //                        where item.UserAuthsId == uid
                //                        select item.Quantity * p.Price).Sum();
                decimal? basketTotal = (from item in basket.BasketItems
                                        join
                                        p in goodsContext.goods on item.GoodsId equals p.GoodsId
                                        where item.RegisterId == existUser

                                        select item.Quantity * p.Price).Sum();
                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }

        public async Task ClearBasket(HttpContext httpContext)
        {

            Basket basket =await GetBasket(httpContext, false);
            try
            {
                var existUser = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var BasketItems = goodsContext.basketItems.Where(p => p.BasketId == basket.BasketId && p.RegisterId == existUser).ToList();
                goodsContext.RemoveRange(BasketItems);

                goodsContext.SaveChanges();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
        }
    }
}

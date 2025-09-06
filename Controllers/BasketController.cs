using efcoreApi.BasketService;
using efcoreApi.Models.VM;
using efcoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace efcoreApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        IBasketService basketService;
        TripSender tripSender;
        IEmailSender emailSender;
        // IHttpContextAccessor httpContext;

        public BasketController(IBasketService _basketService,TripSender _tripSender,IEmailSender _emailSender)
        {
            basketService=_basketService;
            tripSender=_tripSender;
            emailSender=_emailSender;
           // httpContext = _httpContext;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var model =await basketService.GetBasketItems(HttpContext.Request.HttpContext);

            return Ok(model);

        }
        [HttpPost("AddToBasket")]
        public async Task<IActionResult> AddToBasket([FromBody] BasketItemDto basketItemDto)
        {


            try
            {
            await    basketService.AddBasket(HttpContext.Request.HttpContext, basketItemDto.prodId,basketItemDto.inputQnt);
                //return RedirectToAction(nameof(Index));
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
                //return RedirectToAction(nameof(Index), new { Proid = ViewBag.proresult });
            }


            

        }


        [HttpGet("RemoveFromBasket")]
        public async Task<IActionResult> RemoveFromBasket()
        {

            var model =await basketService.GetBasketItems(HttpContext.Request.HttpContext);

            return Ok(model);

        }


        [HttpGet("BasketSummery")]
        public async Task<IActionResult> BasketSummery()
        {

            var model =await basketService.BasketSummary(HttpContext.Request.HttpContext);

            return Ok(model);

        }

        [HttpDelete("RemoveFromBasket/{prodId}")]
        public async Task<IActionResult> RemoveFromBasket(int prodId)
        {
            try
            {
              await  basketService.removefrombasket(HttpContext.Request.HttpContext, prodId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpDelete("ClearBasket")]
        public async Task<IActionResult> ClearBasket()
        {
            try
            {
                var context=HttpContext.Request.HttpContext;
              await  basketService.ClearBasket(context);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //[HttpPost("SendEmail")]
        //public async Task<IActionResult> SendEmail()
        //{
        //    try
        //    {
        //        // tripSender.SendEmailAsync("mohammedfathi0810@gmail.com", "Order Placed", "Your order has been placed successfully.");
        //       await emailSender.SendEmailAsync("mohammedfathi0810@gmail.com", "test smtp", "See if i  successeded");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }

        //}

    }
}

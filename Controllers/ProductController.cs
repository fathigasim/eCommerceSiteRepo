using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly efContext efContext;
        public ProductController(efContext _efContext)
        {
            efContext = _efContext;
        }

        [HttpGet]
        public async Task< ActionResult<PagedResult<Goods>>> Get(int pageNumber = 1, int pageSize = 10) {
            var products = await efContext.goods.Include(p=>p.Category).Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
            var result = new PagedResult<Goods>
            {
                Items = products,
                TotalItems = await efContext.goods.CountAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return Ok(result);
        }
        
        [HttpGet("ByCategory")]
        public async Task<ActionResult<PagedResult<Goods>>> ByCategory([FromQuery]string? categoryId,int pageNumber = 1, int pageSize = 10)
        {
            try {             var query = efContext.goods.Include(p=>p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                query = query.Where(p => p.Category.CategoryId.Equals(categoryId));
            }

            var totalItems = query.Count();
            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<Goods>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }


        }
        //[AllowAnonymous]
        [HttpGet("ProductFilter")]
        public async Task<ActionResult<PagedResult<Goods>>> ProductAdvance(string? search , [FromQuery] List<string> categories = null, int pageNumber = 1, int pageSize = 5)
        {

            var query = efContext.goods.Include(p=>p.Category).AsQueryable();
            
            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.GoodsName.Contains(search));
            }

            // Multiple categories filter
            if (categories != null && categories.Any())
            {
                query = query.Where(p => categories.Contains(p.Category.CategoryName));
            }

            // Price range filter
            //if (minPrice.HasValue)
            //    query = query.Where(p => p.Price >= minPrice.Value);

            //if (maxPrice.HasValue)
            //    query = query.Where(p => p.Price <= maxPrice.Value);
            foreach (var item in query)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", item.Imgpath);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = ContentType.GetContentType(item.Imgpath);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                item.ImgSrc = fullBase64;
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //  fileModel.Add(new FileModel() { url = fullBase64 });
            }
            var totalItems = query.Count();
            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<Goods>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
            
        }

        //[AllowAnonymous]
        [HttpGet("CustomFilter")]
        public async Task<ActionResult<PagedResult<Goods>>> ProductSearch(string? search, string? category = null, int pageNumber = 1, int pageSize = 5)
        {

            var query = efContext.goods.Include(p => p.Category).AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.GoodsName.Contains(search));
            }

            // Multiple categories filter
            if (!string.IsNullOrWhiteSpace(category)) { 
                query = query.Where(p => category.Contains(p.Category.CategoryId));

            }
            // Price range filter
            //if (minPrice.HasValue)
            //    query = query.Where(p => p.Price >= minPrice.Value);

            //if (maxPrice.HasValue)
            //    query = query.Where(p => p.Price <= maxPrice.Value);
            foreach (var item in query)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", item.Imgpath);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = ContentType.GetContentType(item.Imgpath);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                item.ImgSrc = fullBase64;
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //  fileModel.Add(new FileModel() { url = fullBase64 });
            }
            var result = new PagedResult<Goods>
            {
                Items = query,
                TotalItems = query.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("ParamsFilter")]
        public async Task<ActionResult<PagedResult<Goods>>> ProductParamsSearch(string? search, string? category = null, int pageNumber = 1, int pageSize = 4)
        {

            var query = efContext.goods.Include(p => p.Category).AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.GoodsName.Contains(search));
            }

            // Multiple categories filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => category.Contains(p.Category.CategoryId));

            }
            // Price range filter
            //if (minPrice.HasValue)
            //    query = query.Where(p => p.Price >= minPrice.Value);

            //if (maxPrice.HasValue)
            //    query = query.Where(p => p.Price <= maxPrice.Value);
            foreach (var item in query)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", item.Imgpath);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = ContentType.GetContentType(item.Imgpath);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                item.ImgSrc = fullBase64;
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //  fileModel.Add(new FileModel() { url = fullBase64 });
            }

            var totalItems = query.Count();
            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var result = new PagedResult<Goods>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }
    }
}

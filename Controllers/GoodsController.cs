using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Models.VM;
using efcoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Protocol;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="1,2")]
    public class GoodsController : ControllerBase 
    {
        readonly efContext dbContext;
        FileService fileService;
        private readonly IWebHostEnvironment _env;
        IRepositoryBase<Goods> _repository;
        IMemoryCache memoryCache;
        public GoodsController(efContext _dbContext, FileService _fileService, IWebHostEnvironment env, IRepositoryBase<Goods> repository, IMemoryCache _memoryCache)
        {
            dbContext = _dbContext;
            fileService = _fileService;
            _env = env;
            _repository = repository;
            memoryCache=_memoryCache;
        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]GoodsParameters goodsParameters)
        { 
        var paginatedGoods=   dbContext.goods.OrderBy(on => on.GoodsName)
        .Skip((goodsParameters.PageNumber - 1) * goodsParameters.PageSize)
        .Take(goodsParameters.PageSize)
        .ToList();
            //HttpContext.Response.Cookies.Append("returl", returnUrl, new CookieOptions
            //{
            //    Expires = DateTime.UtcNow.AddMinutes(30)
            //});
        
            return Ok(paginatedGoods);
        }
        [HttpGet("AllGoods")]
        public async Task<IActionResult> AllGoods([FromQuery] GoodsParameters goodsParameters)
        {
            var model = _repository.FindAll().ToList();
        //         .OrderBy(on => on.GoodsName)
        //.Skip((goodsParameters.PageNumber - 1) * goodsParameters.PageSize)
        //.Take(goodsParameters.PageSize)
        //.ToList();
            var plist= PagedList<Goods>.ToPagedList(model,
        goodsParameters.PageNumber,
        goodsParameters.PageSize);
            var metadata = new
            {
                plist.totalCount,
                plist.pageSize,
                plist.currentPage,
                plist.totalPages,
                plist.hasNext,
                plist.hasPrevious
            };
            //Response.Headers.Add("xPagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            if (plist.Any())
            {
                return Ok(plist);
            }
            else
            {
                return Unauthorized();
            }
        }
            // GET: api/<GoodsController>
            //[Authorize(Roles = "1")]
            [AllowAnonymous]
            [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = dbContext.goods.Include(p=>p.Category).ToList();
            foreach (var item in model)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", item.Imgpath);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = GetContentType(item.Imgpath);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                item.ImgSrc = fullBase64;
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
              //  fileModel.Add(new FileModel() { url = fullBase64 });
            }
            return Ok(model);
        }
        [AllowAnonymous]
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Get([FromForm]int id)
        {
            var model = await dbContext.goods.Include(p => p.Category).Where(g => g.GoodsId == id).FirstOrDefaultAsync();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", model.Imgpath);
            // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var base64String = Convert.ToBase64String(fileBytes);
            var mimeType = GetContentType(model.Imgpath);
            var fullBase64 = $"data:{mimeType};base64,{base64String}";
            model.ImgSrc = fullBase64;

            return Ok(model);
        }

        [HttpGet("SearchBy/{name}")]
        public async Task<IActionResult> GetName([FromQuery]string name)
        {

            var model = await dbContext.goods.Include(p => p.Category).Where(p=>p.GoodsName.ToLower().Contains(name.ToLower())).ToListAsync();

            foreach (var item in model)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", item.Imgpath);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = GetContentType(item.Imgpath);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                item.ImgSrc = fullBase64;
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //  fileModel.Add(new FileModel() { url = fullBase64 });
            }

            return Ok(model);
        }
        // GET api/<GoodsController>/5

        [HttpGet("{catName}")]
        public async Task<IActionResult> Get(string catName)
        {
            var model = dbContext.goods.Include(p => p.Category).Where(p => p.Category.CategoryName == catName).ToList();
            //foreach (var item in model) {
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", item.Imgpath);
            //    var fileBytes = System.IO.File.ReadAllBytes(filePath);
            //    return File(fileBytes, "image/jpeg"); // Adjust MIME type as necessary
            //}
            return Ok(model);
        }

        //[HttpGet("GetImage/{Image}")]
        [AllowAnonymous]
        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage()
        {
            //var model = await dbContext.goods.Where(p => p.Imgpath == Image).Select(p => p.Imgpath).FirstOrDefaultAsync();
            var model = dbContext.goods.ToList().Select(p => p.Imgpath).LastOrDefault();
            //var allfileBytes=new List< byte>();
            //foreach (var item in model)
            //{
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", model);
                //var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
            // allfileBytes.AddRange(fileBytes);
           //var memo=new MemoryStream(fileBytes,false);
            
            var contentType = GetContentType(model);
            return File(imageFileStream, contentType);
                   // }
          
               //return File(allfileBytes);
        }
        [AllowAnonymous]
        [HttpGet("GetAllImage")]
        public async Task<IActionResult> GetAllImage()
        {
            var fileModel=new List< FileModel>();

            var model = dbContext.goods.ToList().Select(p => p.Imgpath);
           
            foreach (var fileimage in model)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", fileimage);
               // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = GetContentType(fileimage);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileModel.Add(new FileModel() { url = fullBase64 });
            }
            return Ok(fileModel);
        }

        [HttpGet("GetListImage")]
        public async Task<IActionResult> GetListImage()
        {
            var fileModel = new List<FileModel>();

            var model = dbContext.goods.ToList().Select(p => p.Imgpath);

            foreach (var fileimage in model)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", fileimage);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = GetContentType(fileimage);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileModel.Add(new FileModel() { url = fullBase64 });
            }
            return Ok(fileModel);
        }

        // return File((byte[], "image/jpeg");

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                default: return "application/octet-stream";
            }
        }
        // POST api/<GoodsController>
        [AllowAnonymous]
        [HttpPost]
        //[Consumes("multipart/form-data")]

        public async Task<IActionResult> Post([FromForm][Bind("GoodsName", "CategoryId", "GoodsDetail","GoodImg")]GoodsVm goodsvm)
        {
            var goods = new Goods()
            {
                GoodsName = goodsvm.GoodsName,
                CategoryId = goodsvm.CategoryId,
                GoodsDetail = goodsvm.GoodsDetail,
                GoodImg = goodsvm.GoodImg,
            };                                                                   
            try {

                if (!Directory.Exists("Img"))
                {
                    Directory.CreateDirectory("Img");
                }
                var filename = Guid.NewGuid().ToString().Substring(1,8)+goods.GoodImg.FileName;
                var uploadfolder = Path.Combine(Directory.GetCurrentDirectory(), "Img");
              
                var filepath = Path.Combine(uploadfolder, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    goods.GoodImg.CopyTo(filestream);
                }
                goods.Imgpath = filename;
                await dbContext.AddAsync(goods);
            await dbContext.SaveChangesAsync();
                return Ok("Added Successfully");
            }
             catch (Exception ex)
                {
               // throw new Exception(ex.Message);
                return BadRequest(ex.Message); 
            }      
         }

        // PUT api/<GoodsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GoodsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [AllowAnonymous]
        [HttpGet("MemoryGoods")]
        public async Task<IActionResult> MemoryCache()
        {
            var cacheData = memoryCache.Get<IEnumerable<Goods>>("goods");
            if (cacheData != null)
            {
                return Ok(cacheData);
            }

            //var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var expirationTime = new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5.0),Priority=CacheItemPriority.High,SlidingExpiration=TimeSpan.FromMinutes(20) };
            cacheData = await dbContext.goods.ToListAsync();
            memoryCache.Set("goods", cacheData, expirationTime);
            return Ok(cacheData);
        }
    }
}

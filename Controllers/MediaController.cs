using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Models.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        efContext dbContext;
        public MediaController(efContext _dbContext)
        {
            dbContext = _dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var videoModel = new List<VideoModel>();
            var model = dbContext.Videos.Include(p => p.Category).ToList();
            foreach (var item in model)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Videos", item.VideoUrl);
                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(fileBytes);
                var mimeType = GetContentType(item.VideoUrl);
                var fullBase64 = $"data:{mimeType};base64,{base64String}";

                // var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                videoModel.Add(new VideoModel() { VideoUrl = fullBase64 });
            }
     
            return Ok(videoModel);
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
               // case ".jpg":
                case ".mp4": return "video/mp4";
                //case ".png": return "image/png";
                //case ".gif": return "image/gif";
                default: return "application/octet-stream";
            }
        }
        [HttpGet("GetVideo")]
        public async Task<IActionResult> GetVideo()
        {
            var videoModel = new VideoModel();
            //var model = await dbContext.goods.Where(p => p.Imgpath == Image).Select(p => p.Imgpath).FirstOrDefaultAsync();
            var model = dbContext.Videos.ToList().Select(p => p.VideoUrl).LastOrDefault();
            //var allfileBytes=new List< byte>();
            //foreach (var item in model)
            //{
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Videos", model);
            //var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var base64String = Convert.ToBase64String(fileBytes);
            var mimeType =  GetContentType(model);
            var fullBase64 = $"data:{mimeType};base64,{base64String}";
            videoModel.VideoUrl = fullBase64;
            // allfileBytes.AddRange(fileBytes);
            //var memo=new MemoryStream(fileBytes,false);


            return Ok(videoModel);
            // }

            //return File(allfileBytes);
        }
        [HttpPost]
        //[Consumes("multipart/form-data")]

        public async Task<IActionResult> Post([FromForm][Bind("VideoTitle","CategoryId","VideoDescription", "VideoFile")] Video video)
        {

            try
            {

                if (!Directory.Exists("Videos"))
                {
                    Directory.CreateDirectory("Videos");
                }
                var filename = video.VideoFile.FileName;
                var uploadfolder = Path.Combine(Directory.GetCurrentDirectory(), "Videos");

                var filepath = Path.Combine(uploadfolder, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    video.VideoFile.CopyTo(filestream);
                }
                video.VideoUrl = filename;
                await dbContext.AddAsync(video);
                await dbContext.SaveChangesAsync();
                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

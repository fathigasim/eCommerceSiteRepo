using efcoreApi.Data;
using efcoreApi.Models.VM;
using Microsoft.EntityFrameworkCore;

namespace efcoreApi.Services
{
    public  class FileService
    {
        efContext dbContext;
        public FileService(efContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public List< byte[]> imagesAsync()
        {
            //var model = await dbContext.goods.Where(p => p.Imgpath == Image).Select(p => p.Imgpath).FirstOrDefaultAsync();
            //var model = dbContext.goods.ToList().Select(p => new FileModel()
            //{
            //    url = System.IO.File.ReadAllBytes(p.Imgpath)
            //});
           
     
            return new List<byte[]>();
        }
    }
}

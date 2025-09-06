using System.ComponentModel.DataAnnotations.Schema;

namespace efcoreApi.Models.VM
{
    public class GoodsVm
    {

        public string GoodsName { get; set; }
        public string GoodsDetail { get; set; }
        [NotMapped]
        public IFormFile GoodImg { get; set; }
        
        [NotMapped]
        
        public string? CategoryId { get; set; }
     
    }
}

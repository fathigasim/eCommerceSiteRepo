using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcoreApi.Models
{
    public class Goods
    {
        [Key]
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsDetail { get; set; }
        public decimal Price { get; set; }
   
        [NotMapped]
        public IFormFile GoodImg { get; set; }
        public string Imgpath { get; set; } = "";
        [NotMapped]
        public string ImgSrc  { get; set; }
        public string? CategoryId { get; set; }  
        public virtual Category? Category { get; set; }
    }
}

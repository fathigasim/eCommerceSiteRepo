using System.ComponentModel.DataAnnotations;

namespace efcoreApi.Models
{
    public class BasketItems
    {
        [Key]
        public string BasketitemId { get; set; }
        public string BasketId { get; set; }
        public virtual Basket Basket { get; set; }
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        public int Quantity { get; set; } = 0;

        public string RegisterId { get; set; }

        public virtual Register Register { get; set; }


        public BasketItems()
        {
            BasketitemId = Guid.NewGuid().ToString();
            //Quantity = 0;
        }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcoreApi.Models
{
    public class OrderItems
    {
        [Key]
        public string OrderItemId { get; set; }
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }

   
    
        public int Quantity { get; set; }
        
        public string OrderId { get; set; }
        public virtual Orders Order { get; set; }
        //[NotMapped]
        //public int OrdSeq { get; set; }

        public OrderItems()
        {
            OrderItemId = Guid.NewGuid().ToString();
        }


    }
}

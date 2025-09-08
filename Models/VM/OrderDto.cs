using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcoreApi.Models.VM
{
    public class OrderDto
    {
        public string Id { get; set; }
        public int OrdSeq { get; set; }
        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        //public string Street { get; set; }

        //public string City { get; set; }

        public string OrderStatus { get; set; } = "Pending";
        public DateTime OrderDateTime { get; set; }

        public string RegisterId { get; set; }
       // public virtual Register Register { get; set; }

        [NotMapped]
        //[DisplayName("u Name")]
        [Display(Name = "Username")]
        //[Display(Name= nameof(Order.Username),ResourceType =typeof(Resources.Orders))]
        public string Username { get; set; }

        public List<OrderItemDto> OrderItemsDto { get; set; }

        public OrderDto()
        {
            
            OrderItemsDto = new List<OrderItemDto>();
            OrderDateTime = DateTime.Now;
            //OrderItems = new List<OrderItem>();
        }
    }
}

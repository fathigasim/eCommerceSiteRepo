using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcoreApi.Models
{
    [Table("Orders")]
    public class Orders
    {
        [Key]
        public string Id { get; set; }
        //[Range(1, 10000)]
        public int OrdSeq { get; set; }
        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        //public string Street { get; set; }

        //public string City { get; set; }

        public string OrderStatus { get; set; } = "Pending";
        public DateTime OrderDateTime { get; set; }

        public string RegisterId { get; set; }
        public virtual Register Register { get; set; }

        [NotMapped]
        //[DisplayName("u Name")]
        [Display(Name = "Username")]
        //[Display(Name= nameof(Order.Username),ResourceType =typeof(Resources.Orders))]
        public string Username { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }

        public Orders()
        {
              Id = Guid.NewGuid().ToString();
            OrderItems = new List<OrderItems>();
            OrderDateTime = DateTime.Now;
            //OrderItems = new List<OrderItem>();
        }

    }
}

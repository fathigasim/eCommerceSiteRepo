using System.ComponentModel.DataAnnotations;

namespace efcoreApi.Models
{
    public class Basket
    {
        [Key]
        public string BasketId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual  ICollection<BasketItems> BasketItems { get; set; }

        public Basket()
        {
            BasketId = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            BasketItems = new List<BasketItems>();
        }

    }
}

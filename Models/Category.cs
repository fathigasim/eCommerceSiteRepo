namespace efcoreApi.Models
{
    public class Category
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual  ICollection<Goods> Goods { get; set; }=new List<Goods>();
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
        public Category()
        {
            CategoryId = Guid.NewGuid().ToString();

        }
    }
}

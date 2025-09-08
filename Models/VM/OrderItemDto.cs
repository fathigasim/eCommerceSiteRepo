namespace efcoreApi.Models.VM
{
    public class OrderItemDto
    {
        public int GoodsId { get; set; }
       // public virtual Goods Goods { get; set; }
       public string GoodName { get; set; }


        public int Quantity { get; set; }

        public string OrderId { get; set; }
        // public  Orders Order { get; set; }
    }
}

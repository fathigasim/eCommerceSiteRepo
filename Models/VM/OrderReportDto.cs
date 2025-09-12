namespace efcoreApi.Models.VM
{
    public class OrderReportDto
    {
        public string OrderId { get; set; }
        public string Invoice { get; set; } 
        public int Sequence { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemsReportDto> items { get; set; } = new();
    }
}

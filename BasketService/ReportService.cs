using efcoreApi.Data;
using efcoreApi.Models.VM;
using Microsoft.EntityFrameworkCore;

namespace efcoreApi.BasketService
{
    public class ReportService : IReportService
    {
        efContext dbContext;
        public ReportService(efContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<OrderReportDto>> OrderReport()
        {
            var model = await dbContext.Order.ToListAsync();

            List<OrderReportDto> order = new List<OrderReportDto>();
            foreach (var item in model)
            {
                order.Add(new OrderReportDto
                {
                    OrderId = item.Id,
                    OrderDate= item.OrderDateTime,
                    Invoice = new Random().Next(999999999).ToString(),
                    Sequence = item.OrdSeq,
                    Status = item.OrderStatus,
                    items = item.OrderItems.Select(p =>
                    (new OrderItemsReportDto { Product = p.Goods.GoodsName, Quantity = p.Quantity, Price = p.Goods.Price })).ToList()

                });


                
            }
            return order;
        }

        public async Task<List<OrderReportDto>> OrderByDateReport(DateTime dt)
        {
            var model = await dbContext.Order.Where(p=>p.OrderDateTime.Date==dt.Date).ToListAsync();

            List<OrderReportDto> order = new List<OrderReportDto>();
            foreach (var item in model)
            {
                order.Add(new OrderReportDto
                {
                    OrderId = item.Id,
                    OrderDate = item.OrderDateTime.Date,
                    Invoice = new Random().Next(999999999).ToString(),
                    Sequence = item.OrdSeq,
                    Status = item.OrderStatus,
                    items = item.OrderItems.Select(p =>
                    (new OrderItemsReportDto { Product = p.Goods.GoodsName, Quantity = p.Quantity ,Price=p.Goods.Price})).ToList()

                });



            }
            return order;
        }
    }
}


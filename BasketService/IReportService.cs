using efcoreApi.Models.VM;

namespace efcoreApi.BasketService
{
    public interface IReportService
    {

        Task<List<OrderReportDto>> OrderReport();
        Task<List<OrderReportDto>> OrderByDateReport(DateTime dt);
    }
}

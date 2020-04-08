using System.Threading.Tasks;
using OpayCashier.Models;

namespace OpayCashier
{
    public interface ICashierService
    {
        Task<BaseResponse<OrderData>> Order(OrderRequest request);

        Task<BaseResponse<QueryData>> Query(QueryRequest request);
        Task<BaseResponse<CloseData>> Close(CloseRequest request);
    }
}
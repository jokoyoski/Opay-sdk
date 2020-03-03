using System.Threading.Tasks;
using OpayCashier.Models;

namespace OpayCashier
{
    public interface ICashierService
    {
        Task<BaseResponse<OrderResponse>> Order(OrderRequest request);

        Task<BaseResponse<QueryResponse>> Query(QueryRequest request);
        Task<BaseResponse<CloseResponse>> Close(CloseRequest request);
    }
}
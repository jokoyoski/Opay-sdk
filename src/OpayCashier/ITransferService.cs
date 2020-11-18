using System;
using System.Threading.Tasks;
using Manager;

namespace OpayCashier
{
    public interface ITransferService
    {

        Task<BaseResponse<TransferResponse>> Transfer(TransferRequest request);
    }
}

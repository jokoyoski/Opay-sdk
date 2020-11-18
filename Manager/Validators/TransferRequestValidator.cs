using System;
namespace Manager.Validators
{
    public static  class TransferRequestValidator
    {
        public static  void ValidateRequest(this TransferRequest transferRequest)
        {
            if (transferRequest.Amount <= 0)
            {
                throw new ArgumentNullException(nameof(transferRequest.Amount));
            }

            if (string.IsNullOrWhiteSpace(transferRequest.Country))
            {
                throw new ArgumentNullException(nameof(transferRequest.Country));
            }

            if (string.IsNullOrWhiteSpace(transferRequest.Currency))
            {
                throw new ArgumentNullException(nameof(transferRequest.Currency));
            }

            if (string.IsNullOrWhiteSpace(transferRequest.Reason))
            {
                throw new ArgumentNullException(nameof(transferRequest.Reason));
            }

            if (transferRequest.Reciever == null)
            {
                throw new ArgumentNullException(nameof(transferRequest.Reciever));
            }



        }
    }
}

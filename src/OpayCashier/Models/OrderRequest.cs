using System;
using System.Collections.Generic;

namespace OpayCashier.Models
{
    /// <summary>
    /// The order request
    /// </summary>
    public class OrderRequest : IValidatableRequest
    {
        public List<PaymentMethod> PaymentMethods { get; set; }
        public string Reference { get; set; }
        public string ProductDesc { get; set; }
        public List<PayChannel> PayChannels { get; set; }
        public PayAmount PayAmount { get; set; }
        public string UserMobile { get; set; }
        public string UserRequestIp { get; set; }
        public string CallbackUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string MchShortName { get; set; }
        public string ProductName { get; set; }

        public bool Validate()
        {
            if (PaymentMethods != null && PaymentMethods.Count > 0)
            {
                throw new ArgumentNullException(nameof(PaymentMethods));
            }

            if (PayChannels != null && PayChannels.Count > 0)
            {
                throw new ArgumentNullException(nameof(PayChannels));
            }

            if (string.IsNullOrWhiteSpace(Reference))
            {
                throw new ArgumentNullException(nameof(Reference));
            }

            if (string.IsNullOrWhiteSpace(ProductDesc))
            {
                throw new ArgumentNullException(nameof(ProductDesc));
            }

            if (string.IsNullOrWhiteSpace(UserMobile))
            {
                throw new ArgumentNullException(nameof(UserMobile));
            }

            if (string.IsNullOrWhiteSpace(UserRequestIp))
            {
                throw new ArgumentNullException(nameof(UserRequestIp));
            }

            if (string.IsNullOrWhiteSpace(CallbackUrl))
            {
                throw new ArgumentNullException(nameof(CallbackUrl));
            }

            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                throw new ArgumentNullException(nameof(ReturnUrl));
            }

            if (string.IsNullOrWhiteSpace(ProductName))
            {
                throw new ArgumentNullException(nameof(ProductName));
            }

            if (string.IsNullOrWhiteSpace(MchShortName))
            {
                throw new ArgumentNullException(nameof(MchShortName));
            }

            if (PayAmount == null)
            {
                throw new ArgumentNullException(nameof(PayAmount));
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using Manager;


namespace OpayCashier.Models
{
    /// <summary>
    /// The order request
    /// </summary>
    public class OrderRequest : IValidatableRequest
    {
        private const string DefaultCurrency = "NGN";

        /// <summary>
        /// Order number of merchant (unique order number from merchant platform)
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The short name of a Merchant. It's displayed on the payment confirmation page.
        /// </summary>
        public string MchShortName { get; set; }

        /// <summary>
        /// Product name, utf-8 encoded
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product description, utf-8 encoded
        /// </summary>
        public string ProductDesc { get; set; }

        /// <summary>
        /// User phone number sent by merchant
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// The IP address requested by user, need pass-through by merchant, user Anti-phishing verification.
        /// </summary>
        public string UserRequestIp { get; set; }

        /// <summary>
        /// Amount in kobo
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Currency charge should be performed in. Default is NGN
        /// </summary>
        public string Currency { get; set; }

        public List<PayType> PayTypes { get; set; }
        public List<PayMethod> PayMethods { get; set; }

        /// <summary>
        /// The asynchronous callback address after transaction successful.
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// The address that browser go to after transaction successful.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Transaction would be closed within specific time. Value is in minute.
        /// </summary>
        public string ExpireAt { get; set; }

        public void Validate()
        {
            if (PayMethods == null || PayMethods.Count <= 0)
            {
                throw new ArgumentNullException(nameof(PayMethods));
            }

            if (PayTypes == null || PayTypes.Count <= 0)
            {
                throw new ArgumentNullException(nameof(PayTypes));
            }

            if (string.IsNullOrWhiteSpace(Reference))
            {
                throw new ArgumentNullException(nameof(Reference));
            }

            if (string.IsNullOrWhiteSpace(ProductDesc))
            {
                throw new ArgumentNullException(nameof(ProductDesc));
            }

            if (string.IsNullOrWhiteSpace(UserPhone))
            {
                throw new ArgumentNullException(nameof(UserPhone));
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

            if (string.IsNullOrWhiteSpace(Currency))
            {
                Currency = DefaultCurrency;
            }

            if (string.IsNullOrWhiteSpace(Amount))
            {
                throw new ArgumentNullException(nameof(Amount));
            }

            if (!int.TryParse(Amount, out _))
            {
                throw new ArgumentException(nameof(Amount));
            }

            if (string.IsNullOrWhiteSpace(ExpireAt))
            {
                throw new ArgumentNullException(nameof(ExpireAt));
            }

            if (!int.TryParse(ExpireAt, out _))
            {
                throw new ArgumentException(nameof(ExpireAt));
            }
        }
    }
}
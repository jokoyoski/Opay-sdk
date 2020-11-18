using System;
namespace Manager
{
    public class TransferResponse
    {
        public string Reference { get; set; }

        public string OrderNo { get; set; }

        public string Amount { get; set; }

        public string Fee { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public string FailureReason { get; set; }

        public string Type { get; set; }

        public string PhoneNumber { get; set; }
    }
}

using System;
namespace Manager
{
    public class TransferRequest
    {
        

        public long Amount { get; set; }
        public string Country { get; set; }

        public string Currency { get; set; }

        public string Reason { get; set; }
        public Reciever Reciever { get; set; }
        public string Reference { get; set; }

       

    }

    public class Reciever
    {
        public string MerchantId { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string Type { get; set; }

      
      

    }


   
}


using System;

namespace Manager
{
    public abstract class BaseRequest : IValidatableRequest
    {
        public string OrderNo { get; set; }
        public string Reference { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Reference))
            {
                throw new ArgumentNullException(nameof(Reference));
            }

            if (string.IsNullOrWhiteSpace(OrderNo))
            {
                throw new ArgumentNullException(nameof(OrderNo));
            }
        }
    }
}
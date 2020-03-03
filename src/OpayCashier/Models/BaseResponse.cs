namespace OpayCashier.Models
{
    public class BaseResponse<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
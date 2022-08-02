namespace Cars.COMMON.Responses
{
    public class BaseResponse
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; }
    }
}
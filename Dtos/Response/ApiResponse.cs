namespace NL_THUD.Dtos.Response
{
    public class ApiResponse<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

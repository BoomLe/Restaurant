namespace Restaurant.Web.Models
{
    public class ResponseDto
    {
        public object? Result { set; get; }
        public bool IsSuccess { set; get; } = true;
        public string Messages { set; get; } = "";
    }
}

namespace Restaurant.Service.APIOrder.Models.Dto
{
    public class ResponseDto
    {
        public object? Result { set; get; }
        public bool IsSuccess { set; get; } = true;
        public string Messages { set; get; } = "";
    }
}

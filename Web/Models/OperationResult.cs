namespace Web.Models
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Result { get; set; }
        public OperationResult()
        {
            IsSuccess = false;
            Message = string.Empty;
            Result = null;
        }
    }
}

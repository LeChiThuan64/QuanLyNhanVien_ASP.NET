namespace PhongUserManagement.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Message { get; set; } // Thêm tr??ng Message ?? hi?n th? l?i c? th?
    }

}
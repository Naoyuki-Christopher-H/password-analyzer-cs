namespace password_analyzer_cs.Models
{
    /// <summary>
    /// Error view model for displaying error details
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
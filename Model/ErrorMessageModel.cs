namespace WpfAppСourseWork.Model
{
    public class ErrorMessageModel
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public bool Error { get; set; }

        public ErrorMessageModel(string message, string title = null, bool error = true) {
            Message = message;
            Title = title;
            Error = error;
        }
    }
}

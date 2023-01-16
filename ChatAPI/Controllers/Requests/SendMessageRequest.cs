namespace ChatAPI.Controllers.Requests
{
    public class SendMessageRequest
    {
        public string Text { get; set; }
        public string SenderName { get; set; }
        public DateTime Date { get; set; }
    }
}

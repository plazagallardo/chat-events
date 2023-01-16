namespace ChatAPI.Controllers.Requests
{
    public class SendHighFiveRequest
    {
        public string SenderName { get; set; }
        public string RecipientName { get; set; }
        public DateTime Date { get; set; }
    }
}

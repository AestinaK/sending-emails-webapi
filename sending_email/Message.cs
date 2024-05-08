namespace sending_email;

public class Message
{
    public string EmailToAddress { get; set; }
    public string EmailToUserName { get; set; }
    public string Content { get; set; }
    public string Subject  { get; set; }
    public IFormCollection Attachments { get; set; }
}
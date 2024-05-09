namespace sending_email;

public class Message
{
    public string EmailToAddress { get; set; }
    public string EmailToUserName { get; set; }
    public string Content { get; set; }
    public string Subject  { get; set; }
    public IFormFileCollection? Attachments { get; set; }

    public Message(string emailToAddress,string username, string content, string subject, IFormFileCollection? collection)
    {
        EmailToAddress = emailToAddress;
        EmailToUserName = username;
        Subject = subject;
        Content = content;
        Attachments = collection;
    }
}

public class MessageOnly
{
    public string EmailToAddress { get; set; }
    public string EmailToUserName { get; set; }
    public string Content { get; set; }
    public string Subject  { get; set; }
}
namespace sending_email.Service;

public interface IEmailService
{
    Task SendMessage(MessageOnly message);
    
    //TODO - will do next
    Task SendMailWithFileAsync(Message message);
}
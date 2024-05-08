namespace sending_email.Service;

public interface IEmailService
{
    Task SendMessage(Message message);
    
    //TODO - will do next
    Task SendMailWithFileAsync(Message message);
}
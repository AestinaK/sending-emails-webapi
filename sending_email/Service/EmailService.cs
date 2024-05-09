using MailKit.Net.Smtp;
using MimeKit;

namespace sending_email.Service;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _config;

    public EmailService(EmailConfiguration config)
    {
        _config = config;
    }

    public async Task SendMessage(Message message)
    {
        var email = CreateEmail(message);
        await SendEmail(email);
    }

    //TODO - will do next
    public async Task SendMailWithFileAsync(Message message)
    {
        var email = CreateFileEmail(message);
        //await SendEmail(email);
    }
    
    
    private async Task<MimeMessage> CreateFileEmail(Message message)
    {
        var email = new MimeMessage();
        var mailFrom = new MailboxAddress(_config.From, _config.UserName);
        email.From.Add(mailFrom);
    
        var mailTo = new MailboxAddress(message.EmailToAddress, message.EmailToUserName);
        email.To.Add(mailTo);
        email.Subject = message.Subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return email;
    }
    private MimeMessage CreateEmail(Message message)
    {
        var email = new MimeMessage();
        MailboxAddress mailFrom = new MailboxAddress(_config.UserName, _config.From);
        email.From.Add(mailFrom);

        var mailTo = new MailboxAddress(message.EmailToAddress, message.EmailToUserName);
        email.To.Add(mailTo);
        
        email.Subject = message.Subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return email;
    }

    private async Task SendEmail(MimeMessage message)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                 await client.ConnectAsync(_config.SmtpServer, _config.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_config.UserName, _config.Password);

                await client.SendAsync(message: message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
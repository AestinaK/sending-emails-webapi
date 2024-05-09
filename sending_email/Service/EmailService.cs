using System.Net.Mime;
using MailKit.Net.Smtp;
using MimeKit;
using ContentType = MimeKit.ContentType;

namespace sending_email.Service;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _config;

    public EmailService(EmailConfiguration config)
    {
        _config = config;
    }

    public async Task SendMessage(MessageOnly message)
    {
        var email = CreateEmail(message);
        await SendEmail(email);
    }

    //TODO - will do next
    public async Task SendMailWithFileAsync(Message message)
    {
        var email = await CreateFileEmail(message);
        await SendEmail( email);
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

        var builder = new BodyBuilder { HtmlBody = $"<h3 style='color:red;'>{message.Content}</h3>" };
        if (message.Attachments != null && message.Attachments.Any())
        {
            foreach (var attachment in message.Attachments)
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await attachment.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                builder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }
        email.Body = builder.ToMessageBody();
        return email;
    }

    private MimeMessage CreateEmail(MessageOnly message)
    {
        var email = new MimeMessage();
        MailboxAddress mailFrom = new MailboxAddress(_config.UserName, _config.From);
        email.From.Add(mailFrom);

        var mailTo = new MailboxAddress(message.EmailToUserName, message.EmailToAddress);
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
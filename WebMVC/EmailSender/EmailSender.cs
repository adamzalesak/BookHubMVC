using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;
using DataAccessLayer.Models;
using System.Net;

namespace WebMVC.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailConfig;

        public EmailSender(EmailSettings emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new Message(email, email, subject, htmlMessage);
            SendEmail(message);
            return Task.CompletedTask;
        }

        private void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.Sender));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message.Content;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.MailServer, _emailConfig.MailPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Sender, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

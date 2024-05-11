using MailKit.Net.Smtp;
using MimeKit;

namespace WebMVC.EmailSender
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(string to, string toName, string subject, string content)
        {
            To = new List<MailboxAddress>()
            {
                new MailboxAddress(toName, to)
            };
                        
            Subject = subject;
            Content = content;
        }
    }
}

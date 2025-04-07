using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace MobileShopInMVC.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 465))
            {
                client.Credentials = new NetworkCredential("vijayotaradi118@gmail.com", "cyuu cdsj samu kapb");
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("vijayotaradi118@gmail.com");
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}

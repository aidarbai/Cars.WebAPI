using Cars.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cars.BLL.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
#if DEBUG        
        public async Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage)
        {
            MailMessage mailMessage = new();

            mailMessage.From = new MailAddress("serveremail@ourhosting.com");

            mailMessage.To.Add(new MailAddress(email));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = htmlMessage;

            SmtpClient client = new();

            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            System.IO.Directory.CreateDirectory(@"C:\Test");
            client.PickupDirectoryLocation = @"C:\Test";

            await client.SendMailAsync(mailMessage);
        }

#else

        private readonly ILogger<EmailSender> _logger;
        private readonly string sendGridApiKey;
        private readonly string senderEmail;
        private readonly string senderName;
        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            sendGridApiKey = configuration["SendGrid:ApiKey"];
            senderEmail = configuration["SendGrid:SenderEmail"];
            senderName = configuration["SendGrid:SenderName"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(new SendGridClientOptions { ApiKey = sendGridApiKey, HttpErrorAsException = true });
            var from = new EmailAddress(senderEmail, senderName);
            var to = new EmailAddress(email);
            var plainTextContent = "Message from Cars.Web.API server";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
            Response response = await client.SendEmailAsync(msg);
            var statusCode = response.StatusCode;
            _logger.LogInformation($"SendGrid responded with the code: {statusCode}");
        }

#endif

    }
}
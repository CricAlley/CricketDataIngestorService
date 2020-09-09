using System;
using System.Net;
using System.Net.Mail;

namespace PlayerDataGenerator
{
    public interface IEmailSender
    {
        void Email(string mailText, string subject);
        bool IsMailSettingsConfigured();
    }
    public class EmailSender : IEmailSender
    {
        private readonly GeneralSettings _settings;

        public EmailSender(GeneralSettings settings)
        {
            _settings = settings;
        }

        public void Email(string mailText, string subject)
        {
            try
            {
                using var smtp = new SmtpClient();
                using var message = new MailMessage();
                message.From = new MailAddress(_settings.From);
                message.To.Add(new MailAddress(_settings.RequiredTo));
                if (!string.IsNullOrWhiteSpace(_settings.OptionalTo))
                {
                    message.To.Add(new MailAddress(_settings.OptionalTo));
                }
                message.Subject = subject;
                message.Priority = MailPriority.High;
                message.IsBodyHtml = false; 
                message.Body = mailText;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; 
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_settings.From, _settings.Code);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public bool IsMailSettingsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_settings.RequiredTo)
                && !string.IsNullOrWhiteSpace(_settings.From)
                && !string.IsNullOrWhiteSpace(_settings.Code);
        }
    }
}

using LibrarySystem.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using LibrarySystem.BLL.Setting;

namespace LibrarySystem.BLL.Services.Classes
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailsetting; // to confirm the configruration to =>>>> Strongly typed class 

        public EmailSender(IOptions<EmailSettings> emailsetting)
        {
            _emailsetting = emailsetting.Value;

            if (string.IsNullOrEmpty(_emailsetting.Email))
            {
                throw new Exception("EmailSettings.Email was NOT loaded!");
            }
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587) // we will use the Gmail smtp and port 587 
            {
                EnableSsl = true,
                UseDefaultCredentials = false, // Credantiles => the email and email.app.password => used to send via Gmail 
                Credentials = new NetworkCredential(
                    _emailsetting.Email, // email
                    _emailsetting.Password // email.app.password 
                    )
            };

            var mail = new MailMessage(
                from: _emailsetting.Email,
                                to: email,
                                subject,
                                message


                                );

            mail.IsBodyHtml = true;
            return client.SendMailAsync( mail );

        }

    }

}

using Microsoft.Extensions.Configuration;
using ShiftEase.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShiftEase.Core.Implementation
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public SmtpEmailService(IConfiguration config) => _config = config;

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = int.Parse(_config["Smtp:Port"] ?? "25");
            var user = _config["Smtp:User"];
            var pass = _config["Smtp:Pass"];
            var from = _config["Smtp:From"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = bool.Parse(_config["Smtp:EnableSsl"] ?? "true"),
                Credentials = new NetworkCredential(user, pass)
            };

            var mail = new MailMessage(from, to, subject, htmlBody) { IsBodyHtml = true };
            await client.SendMailAsync(mail);
        }
    }

}
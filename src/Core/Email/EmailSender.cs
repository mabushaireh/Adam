// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description
// ======================================

namespace i2fam.Core.Email
{
    using System.Net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender
    {
        public static SmtpConfig Configuration;


        public static async Task<(bool success, string errorMsg)> SendEmailAsync(IEnumerable<EmailAddress> recipients, string subject, string content, bool isHtml, bool addBccs = false)
        {
            var client = new SendGridClient(Configuration.ApiKey);

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(Configuration.NotificationsEmail, Configuration.NotificationsName));
            msg.AddTos(recipients.ToList());
            if (addBccs)
            {
                var bccList = new List<EmailAddress> {
                    new EmailAddress {
                        Email = Configuration.AdminEmail,
                        Name = Configuration.AdminName
                    }
                };
                msg.AddBccs(bccList);

            }
            msg.SetSubject(subject);
            //TODO: Check if not isHtml send switch to plain text
            msg.AddContent(MimeType.Html, content);

            var response = await client.SendEmailAsync(msg);

            return new ValueTuple<bool, string>(response.StatusCode == HttpStatusCode.Accepted, await response.Body.ReadAsStringAsync());
        }
    }
}

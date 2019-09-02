// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 
//TODO: add description 
// ======================================

namespace i2fam.Core.Email
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class EmailTemplates
    {
        static string accountCreated;
        static string accountActivated;

        static string familyMemberUpdated;


        public static void Initialize()
        {
        }


        public static string GetAccountCreatedEmail(Dictionary<string, string> contentParams, string lang)
        {
            if (accountCreated == null)
                accountCreated = GetTemplate("AccountCreated.template", lang);

            string emailMessage = accountCreated;


            emailMessage = ReplaceParams(contentParams, emailMessage);

            return emailMessage;
        }

        public static string GetAccountActivatedEmail(Dictionary<string, string> contentParams, string lang)
        {
            if (accountActivated == null)
                accountActivated = GetTemplate("AccountActivated.template", lang);

            string emailMessage = accountActivated;


            emailMessage = ReplaceParams(contentParams, emailMessage);

            return emailMessage;
        }

        public static string GetFamilyMemberUpdatedEmail(Dictionary<string, string> contentParams, string lang)
        {
             if (familyMemberUpdated == null)
                familyMemberUpdated = GetTemplate("familyMemberUpdated.template", lang);

            string emailMessage = familyMemberUpdated;


            emailMessage = ReplaceParams(contentParams, emailMessage);

            return emailMessage;
        }

        private static string ReplaceParams(Dictionary<string, string> contentParams, string emailMessage)
        {
            foreach (var param in contentParams)
            {
                emailMessage = emailMessage.Replace(param.Key, param.Value);
            }
            return emailMessage;
        }

        private static string GetTemplate(string templateName, string lang)
        {
            var assembly = typeof(EmailTemplates).GetTypeInfo().Assembly;

            using (
                var textStream =
                    new StreamReader(assembly.GetManifestResourceStream($"i2fam.Core.Email.Templates.{lang}.{templateName}")))
            {
                return textStream.ReadToEnd();
            }
        }


    }
}

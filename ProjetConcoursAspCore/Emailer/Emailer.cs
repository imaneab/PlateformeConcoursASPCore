using System.Net;
using System.Net.Mail;

namespace ProjetConcoursAspCore
{
    public class Emailer
    {
        static string smtpAddress = "smtp.gmail.com";
        static bool enableSSL = true;
        static string password = "zen#@2019"; //Sender Password  
        static string subject = "Registration Password";
       
        
        
        public bool SendEmail(string senderEmail, string receiverEmail, int smtpPort, string registrationPassword)
        {
            try
            {
                string body = "Hey User, Please collect your registation password "+ registrationPassword+" <br> Thanks For Registation!!!!";
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(receiverEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.UseDefaultCredentials = true;
                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
            
        }
    }
}

 
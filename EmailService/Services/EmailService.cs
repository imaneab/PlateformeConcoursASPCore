using EmailService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailService.Services
{
	public interface IEmailService
	{
		void send(string reciver, string subject, string body);
	}
	public class EmailService: IEmailService
	{
		MailMessage message;
		SmtpClient client;
		public EmailService()
		{
			Console.WriteLine(Config.Config.SmtpUsername);
			client = new SmtpClient(Config.Config.SmtpServer)
			{
				Port = Config.Config.SmtpPort,
				UseDefaultCredentials = false,
				EnableSsl = true,
				Credentials = new System.Net.NetworkCredential(Config.Config.SmtpUsername, Config.Config.SmtpPassword),
			};
		}
		public async void send(string reciver,string subject,string body)
		{
			message = new MailMessage();
			message.To.Add(reciver);
			message.Subject = subject;
			message.Body = body;
			message.IsBodyHtml = true;
			message.From = new MailAddress("agurram2013@gmail.com",Config.Config.AppName);
			await client.SendMailAsync(message);
			
		}
	}
}

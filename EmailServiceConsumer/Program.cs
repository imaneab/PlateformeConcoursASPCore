using Confluent.Kafka;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmailServiceConsumer
{
	
	class Program
	{
		const string KAFKA_HOST= "kafka:9092";
		const string KAFKA_TOPIC= "email";
		const string KAFKA_GROUP= "email-asp-group";
		const string EMAIL_SERVICE_ENDPOINT= "http://email_service:3000/";
		public Program()
		{
			start().Wait();
		}
		static void Main(string[] args)
		{
			new Program();
			
		}
		private async Task start()
		{
			Console.WriteLine("Consumer service started ...");
			var conf = new ConsumerConfig
			{
				GroupId = KAFKA_GROUP,
				BootstrapServers = KAFKA_HOST,
				AutoOffsetReset = AutoOffsetReset.Earliest
			};
			using var c = new ConsumerBuilder<Ignore, string>(conf).Build();
			c.Subscribe(KAFKA_TOPIC);
			var cts = new CancellationTokenSource();
			Console.CancelKeyPress += (_, e) => {
				e.Cancel = true;
				cts.Cancel();
			};
			try
			{
				while (true)
				{
					var cr = c.Consume(cts.Token);
					Console.WriteLine($"Consumed message '{cr.Value}' from topic {cr.Topic}, partition {cr.Partition}, offset {cr.Offset}");
					try
					{
						var message = JsonSerializer.Deserialize<MessageToSend>(cr.Value);
						Console.WriteLine("Sending  email to {0} of type {1}", message.UserID, message.Type);
						string url = String.Format("/api/email/signup/{0}",message.UserID);
						if(message.Type.Equals(EmailTypes.ACCEPTED))
						{
							url = String.Format("/api/email/accepted/{0}?json={{filiere:{1}}}",message.UserID,message.Filiere) ;
						}
						else if (message.Type.Equals(EmailTypes.PRESELECTION))
						{
							url = String.Format("/api/email/preselection/{0}?json={{filiere:{1}}}", message.UserID, message.Filiere);
						}
						Console.WriteLine("Sending email");
						var res = await this.SendEmailAsync(url);
						if(res)
						{
							Console.WriteLine("Sending  email to {0} of type {1}, Successeded", message.UserID, message.Type);
						}
						else
						{
							Console.WriteLine("Sending  email to {0} of type {1}, Failed", message.UserID, message.Type);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				c.Close();
			}
		
			return;
		}
		public async System.Threading.Tasks.Task<bool> SendEmailAsync(string url)
		{
			string Baseurl = EMAIL_SERVICE_ENDPOINT;
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);

				client.DefaultRequestHeaders.Clear();
				//Define request data format  
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("token", "vDdzzWaXf8866Gbsdzfz788221Afd");

				//Sending request to find web api REST service resource GetAllEmployees using HttpClient  
				HttpResponseMessage Res = null;
				try
				{
					Res = await client.GetAsync(url);
				}
				catch(Exception ex)
				{
					Console.WriteLine("Exception");
					Console.WriteLine(ex.Message);
				}
				if (Res == null)
					return false; 
				//Checking the response is successful or not which is sent using HttpClient  
				if (Res.IsSuccessStatusCode)
				{
					//Storing the response details recieved from web api   
					var response = Res.Content.ReadAsStringAsync().Result;

					//Deserializing the response recieved from web api and storing into the Employee list  
					if (Res.ToString().Equals("ok")) return true;

				}
				return false;
			}

		}
		public class MessageToSend
		{
			public int UserID { get; set; }
			public string Type { get; set; }
			public string Filiere { get; set; }

			public MessageToSend()
			{
			}

			public MessageToSend(int userID, string type)
			{
				UserID = userID;
				Type = type;
			}

			public MessageToSend(int userID, string type, string filiere) : this(userID, type)
			{
				Filiere = filiere;
			}
		}
		public class EmailTypes
		{
			public static string SIGNUP = "signup";
			public static string ACCEPTED = "accepted";
			public static string PRESELECTION = "preselection";
		}
	}
}

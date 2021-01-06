using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
namespace ProjetConcoursAspCore.Kafka
{
    public class EmailTypes
	{
        public static string SIGNUP = "signup";
        public static string ACCEPTED = "accepted";
        public static string PRESELECTION = "preselection";
    }
    public class MessageToSend
	{
		public int UserID { get; set; }
		public string Type { get; set; }
		public string Filiere { get; set; }

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
    public class ProducerWrapper
    {
        private string _topicName;
        private IProducer<string, string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._config = config;
            this._producer = new ProducerBuilder<string, string>(this._config).Build();
        }
        public async Task writeMessage(int userId,string type,string filiere = null)
        {
            var dr = await this._producer.ProduceAsync(this._topicName, new Message<string, string>()
            {
                Key = Convert.ToString(userId),
                Value = JsonSerializer.Serialize(new MessageToSend(userId,type,filiere))
            });
            ;
            Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            return;
        }
        public async Task writeMessage(MessageToSend message)
		{
            await this.writeMessage(message.UserID, message.Type, message.Filiere);
            return;

        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFI.PrimerParcial.RabbitCommon.Interfaces;

namespace TFI.PrimerParcial.RabbitCommon.Implementations
{
    public class Consumer : IConsumer
    {
        public List<string> ConsumeMessage(string queue, string connString)
        {
            var res = new List<string>();
            var message = string.Empty;
            var cf = CreateConnection(ParseConnectionString(connString));
            var arguments = new Dictionary<string, object>();
            arguments.Add("x-max-priority", 10);
            using (var connection = cf.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue, false, false, false, arguments);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);

                    if (!string.IsNullOrEmpty(message))
                    {
                        Console.WriteLine($"Message Received: {message}");
                        res.Add(message);
                    }
                };

                channel.BasicConsume(queue, autoAck: true, consumer: consumer);
                Console.WriteLine("After finished, press enter");
                Console.ReadLine();
            }

            return res;
        }

        private ConnectionFactory CreateConnection(Dictionary<string, string> parms)
        {
            var cf = new ConnectionFactory();
            cf.VirtualHost = parms.ContainsKey("virtualhost") ? parms["virtualhost"] : "/";
            cf.HostName = parms["host"];
            cf.UserName = parms.ContainsKey("username") ? parms["username"] : cf.UserName;
            cf.Password = parms.ContainsKey("password") ? parms["password"] : cf.Password;
            return cf;
        }

        private Dictionary<string, string> ParseConnectionString(string connString)
        {
            var r = connString.Split(';');
            return r.Select(f => f.Split('=')).ToDictionary(f => f[0].ToLower(), f => f[1]);
        }
    }
}

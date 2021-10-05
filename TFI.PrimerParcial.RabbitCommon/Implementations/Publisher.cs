using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFI.PrimerParcial.RabbitCommon.Interfaces;

namespace TFI.PrimerParcial.RabbitCommon.Implementations
{
    public class Publisher<T> : IPublisher<T> where T : class
    {
        public Task SendToQueue(T obj, string queue, string connString, int? priority = null)
        {
            var cf = CreateConnection(ParseConnectionString(connString));
            var args = new Dictionary<string, object>();
            args.Add("x-max-priority", 10);
            var message = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(message);

            using (var connection = cf.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.Priority = Convert.ToByte(priority);
                
                channel.QueueDeclare(queue, false, false, false, args);
                channel.BasicPublish("", queue, properties, body: body);
            }

            return Task.CompletedTask;
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

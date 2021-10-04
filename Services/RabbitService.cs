using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Contracts;

namespace Services
{
    public class RabbitService<T> : IRabbitService<T> where T : class
    {
        public Task SendToQueue(T data, string queue, int priority)
        {
            var factory = CreateFactory();
            var arguments = new Dictionary<string, object>();
            arguments.Add("x-max-priority", 10);
            var message = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(message);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.Priority = Convert.ToByte(10);

                channel.QueueDeclare(queue, false, false, false, arguments);
                channel.BasicPublish("", queue, properties, body: body);
            }

            return Task.CompletedTask;
        }

        public string ConsumeFromQueue(string queue)
        {
            var factory = CreateFactory();
            var arguments = new Dictionary<string, object>();
            arguments.Add("x-max-priority", 10);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue, false, false, false, arguments);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                };

                channel.BasicConsume(queue, autoAck: true, consumer: consumer);
            }

            return "";
        }


        private ConnectionFactory CreateFactory()
        {
            return new ConnectionFactory() { HostName = "localhost" };
        }
    }
}

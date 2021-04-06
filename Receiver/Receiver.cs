using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using consumidorrabbit;
using RabbitMQ.Client.Events;

namespace Receiver
{
    public class Receiver
    {
        public bool ConsumirReceiver()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Password = "guest";
            factory.Port = 7070;
            factory.UserName = "guest";
            
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "MessageMeuServico", false, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var brokerMessage = Encoding.UTF8.GetString(body.ToArray());
                    var message = JsonSerializer.Deserialize<MessageTransport>(brokerMessage);
                    Console.WriteLine($"{message.guid}");
                    
                }
                catch (Exception ex)
                {
                    var teste = ex.Message;
                }
            };
            channel.BasicConsume("MessageMeuServico", autoAck: false, consumer: consumer);
            Console.Read();
            return true;
        }
    }
}

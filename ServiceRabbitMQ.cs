using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class ServiceRabbitMQ : IHostedService
    {
        private IModel channel = null;
        private IConnection connection = null;

        // Initiate RabbitMQ and start listening to an input queue
		private void Run()
        {
            // ! Fill in your data here !
			var factory = new ConnectionFactory()
            {
                HostName = "hostname",
                // port = 5672, default value
                VirtualHost = "/",
                UserName = "user",
                Password = "password"
            };

            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();

            // ! Declare an exchange, need to be updated !
			this.channel.ExchangeDeclare("exchange", "direct", true, false, null);
			
            // A queue to read messages
			this.channel.QueueDeclare(queue: "queue.in",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            this.channel.QueueBind("queue.in", "exchange", "in");

            // A queue to write messages
			this.channel.QueueDeclare(queue: "queue.out",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            this.channel.QueueBind("queue.out", "exchange", "out");

            this.channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += OnMessageRecieved;
          
            this.channel.BasicConsume(queue: "queue.in",
                                autoAck: false,
                                consumer: consumer);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.Run();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.channel.Dispose();
            this.connection.Dispose();
            return Task.CompletedTask;
        }

        // Publish a received  message with "reply:" prefix
		private void OnMessageRecieved(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            int dots = message.Split('.').Length - 1;

            // Publish a response
            string outMessage = "reply:" + message;
            body = Encoding.UTF8.GetBytes(outMessage);

            this.channel.BasicPublish(exchange: "exchange",
                                 routingKey: "out",
                                 basicProperties: this.channel.CreateBasicProperties(),
                                 body: body);
            Console.WriteLine(" [x] Sent {0}", outMessage);

            Console.WriteLine(" [x] Done");
            this.channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
        }
        
    }
}

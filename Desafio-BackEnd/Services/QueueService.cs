using System.Text.Json;
using RabbitMQ.Client;

namespace Desafio_Backend.Services
{
    public class QueueService<T> : IQueueService<T>
    {
        public ConnectionFactory ConnectionFactory { get; set; }

        public QueueService()
        {
            ConnectionFactory = new ConnectionFactory { HostName = "localhost" };
        }

        public void Publish(T message )
        {
            using var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: typeof(T).Name,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: typeof(T).Name,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
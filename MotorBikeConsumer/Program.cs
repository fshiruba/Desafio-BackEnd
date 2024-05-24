using Desafio_Backend.Data;
using Desafio_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace MotorBikeConsumer
{
    internal class Program
    {

        public static RentalDbContext RentalDbContext { get; set; }
        public static RentalDbContext MockRentalDbContext { get; set; }

        static void Main(string[] args)
        {
            RentalDbContext =  SetupRealDB();

            var dbOptionsMemory = new DbContextOptionsBuilder<RentalDbContext>()
           .UseInMemoryDatabase(databaseName: "MockDB")
           .Options;

            MockRentalDbContext = new RentalDbContext(dbOptionsMemory);


            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "Motorbike",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += HandleMessage;

            channel.BasicConsume(queue: "Motorbike",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static RentalDbContext SetupRealDB()
        {
            // Specify the path to the appsettings.json file in the other project
            var appSettingsPath = @"appsettings.json";

            // Build configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Optional: Set base path if needed
                .AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true);

            var configuration = configBuilder.Build();

            // Access configuration values
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Console.WriteLine($"Connection String: {connectionString}");

            return new RentalDbContext(new DbContextOptionsBuilder<RentalDbContext>().UseNpgsql(connectionString).Options);
        }

        public static void HandleMessage(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var motorBike = JsonSerializer.Deserialize<Motorbike>(body);

            Console.WriteLine($" [x] Received {motorBike!.Model} - {motorBike.Plate} - {motorBike.ProductionYear}");

            if ( motorBike.ProductionYear==2024 )
            {
                //save in DB
                if (Guid.TryParse(motorBike.AdminId, out Guid result))
                {
                    RentalDbContext.MotorbikeLogs.Add(new MotorbikeLog { MotorbikeId = motorBike.Id, TimeStamp = DateTime.Now });
                    RentalDbContext.SaveChanges();
                }
                else
                {
                    MockRentalDbContext.MotorbikeLogs.Add(new MotorbikeLog { MotorbikeId = motorBike.Id, TimeStamp = DateTime.Now });
                    MockRentalDbContext.SaveChanges();

                    foreach ( var item in MockRentalDbContext.MotorbikeLogs)
                    {
                        Console.WriteLine($"{item.MotorbikeId} - {item.TimeStamp}");
                    }
                }
            }

        }

    }
}

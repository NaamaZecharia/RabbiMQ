using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Generic;


public class NewTask
{
    static void Main()
    {
        //Create connection to the server
        var factory = new ConnectionFactory() { HostName = "localhost" };

         using (var connection = factory.CreateConnection())
            {
                //create channel 
                using (var channel = connection.CreateModel())
                {
                    //declare a queue to send to
                    channel.QueueDeclare(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //publish a message to the queue
                    var message = userMessage();
                    

                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties:  properties, body: body);
                    Console.WriteLine(" [x] Sent: {0}", message);
                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

            static string GetMessage(string[] args) //private
            {
                return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
            }


        string userMessage()
        {
            //Ask input from user
            Console.WriteLine("please enter: Name, Date, Age , Profession in this format");
            string s = Console.ReadLine();
            
            string[] message = s.Split(','); //split the input by comma

            //remove age from input
            message[2] = message[3];
            Array.Resize(ref message, 3);
             
            Console.WriteLine(message);
            
            return(string.Join(",", message));
        }
    }
        
}


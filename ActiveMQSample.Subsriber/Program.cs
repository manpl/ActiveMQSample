using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQSample.Subsriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");

            Console.WriteLine("About to connect to " + connecturi);

            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession())
            {
                IDestination destination = session.GetDestination("queue://FOO.BAR");

                connection.Start();
                Console.WriteLine("Connection established");

                using (var consumer = session.CreateConsumer(destination))
                {
                    while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                    {
                        var message = (ITextMessage)consumer.Receive();
                        Console.WriteLine(message.Text);
                        Console.WriteLine("------------------------------------------");
                    }
                }
            }

            Console.WriteLine("Terminating..");
        }
    }
}

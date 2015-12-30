using Apache.NMS;
using System;

namespace ActiveMQSample
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

                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    Console.WriteLine("Press enter to send another message, q to quit");
                    while((Console.ReadLine()??"").ToLower() != "q")
                    {
                        var payload = "Hello there. " + DateTime.Now.ToLongTimeString();
                        var message = session.CreateTextMessage(payload);
                        producer.Send(message);
                        Console.WriteLine("Message sent. " + payload);
                        Console.WriteLine("------------------------------------------");
                    }
                }
            }

            Console.WriteLine("Terminating..");
        }
    }
}

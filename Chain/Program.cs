using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chain
{
    static class Program
    {
        static void Main(string[] args)
        {
            var listenPort = Int32.Parse(args[0]);
            var address = args[1];
            var port = Int32.Parse(args[2]);
            var isInitiator = args.Length == 4 && args[3] == "true";

            Console.WriteLine("Write number to send");
            var x = Convert.ToInt32(Console.ReadLine());

            var listener = InitListener(listenPort);
            var client = InitClient(address, port);

            var worker = FetchWorkingStrategy(isInitiator);
            
            worker.Process(client, listener, x);

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static IWorkingStrategy FetchWorkingStrategy(bool isInitiator)
        {
            if (isInitiator)
            {
                return new InitiatorWorkingStrategy();
            }
            
            return new NormalProcessWorkingStrategy();
        }

        private static Socket InitListener(int listenPort)
        {
            var listenIpAddress = IPAddress.Any;
            var localEp = new IPEndPoint(listenIpAddress, listenPort);
            
            var listener = new Socket(
                listenIpAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            listener.Bind(localEp);
            listener.Listen(10);

            return listener;
        }

        private static Socket InitClient(string address, int port)
        {
            var ipAddress = address == "localhost" ? IPAddress.Loopback : IPAddress.Parse(address);
            var remoteEp = new IPEndPoint(ipAddress, port);
            
            var client = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            
            CircuitBreaker(() =>
            {
                client.Connect(remoteEp);
            }, 5, 2000);

            return client;
        }

        private delegate void Callback();

        private static void CircuitBreaker(Callback callback, int repeats, int waitMilliseconds)
        {
            for (var i = 0; i < repeats; ++i)
            {
                try
                {
                    callback();
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wait for connection");
                    Thread.Sleep(waitMilliseconds);
                }
            }

            throw new Exception("Cannot connect");
        }
    }
}

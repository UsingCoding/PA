using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        private struct Args
        {
            public Args(int port)
            {
                Port = port;
            }
            
            public int Port { get; }
        }

        
        private static void StartListening(Args args)
        {

            // Разрешение сетевых имён
            
            // Привязываем сокет ко всем интерфейсам на текущей машинe
            IPAddress ipAddress = IPAddress.Any; 
            
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, args.Port);

            // CREATE
            Socket listener = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            var messagesHistory = new List<string>();

            try
            {
                // BIND
                listener.Bind(localEndPoint);

                // LISTEN
                listener.Listen(10);

                while (true)
                {
                    // ACCEPT
                    Socket handler = listener.Accept();

                    byte[] buf = new byte[1024];
                    string data = null;
                    while (true)
                    {
                        // RECEIVE
                        int bytesRec = handler.Receive(buf);

                        data += Encoding.UTF8.GetString(buf, 0, bytesRec);
                        if (data.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                        {
                            break;
                        }
                    }

                    data = data.Split("<EOF>")[0];

                    Console.WriteLine("Message received: {0}", data);
                    
                    messagesHistory.Add(data);

                    var response = string.Join(" | ", messagesHistory);

                    // Отправляем текст обратно клиенту
                    byte[] msg = Encoding.UTF8.GetBytes(response);

                    // SEND
                    handler.Send(msg);

                    // RELEASE
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        
        static void Main(string[] cliArgs)
        {
            Args args;
            try
            {
                args = ParseArgs(cliArgs);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error: {0}", e);
                return;
            }
            
            StartListening(args);

            Console.WriteLine("\nНажмите ENTER чтобы выйти...");
            Console.Read();
        }
        
        private static Args ParseArgs(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("invalid count of arguments");
            }
            
            return new Args(int.Parse(args[0]));
        }
    }
}

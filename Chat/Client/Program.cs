using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        private struct Args
        {
            public Args(string host, int port, string message)
            {
                Host = host;
                Port = port;
                Message = message;
            }

            public string Host { get; }
            public int Port { get; }
            public string Message { get; }
        }
        
        private static void StartClient(Args args)
        {
            try
            {
                // Разрешение сетевых имён
                // IPAddress ipAddress = IPAddress.Loopback;
                
                IPHostEntry ipHostInfo = Dns.GetHostEntry(args.Host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                IPEndPoint remoteEp = new IPEndPoint(ipAddress, args.Port);

                // CREATE
                Socket sender = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream, 
                    ProtocolType.Tcp);

                try
                {
                    // CONNECT
                    sender.Connect(remoteEp);

                    // Подготовка данных к отправке
                    var msg = Encoding.UTF8.GetBytes(args.Message + "<EOF>");

                    // SEND
                    int bytesSent = sender.Send(msg);

                    // RECEIVE
                    byte[] buf = new byte[1024];

                    string data = null;
                    var dataPrevLength = 0;
                    while (true)
                    {
                        // RECEIVE
                        int bytesRec = sender.Receive(buf);

                        data += Encoding.UTF8.GetString(buf, 0, bytesRec);
                        if (dataPrevLength == data.Length)
                        {
                            break;
                        }

                        dataPrevLength = data.Length;
                    }

                    Console.WriteLine(data);

                    // RELEASE
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane);
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e);
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
            catch (ArgumentException exception)
            {
                Console.WriteLine("Error: {0}", exception);
                return;
            }
            
            StartClient(args);
        }

        private static Args ParseArgs(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("invalid count of arguments");
            }
            
            return new Args(args[0], int.Parse(args[1]), args[2]);
        }
    }
}

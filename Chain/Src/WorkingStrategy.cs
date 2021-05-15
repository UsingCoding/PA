using System;
using System.Net.Sockets;

namespace Chain
{
    public interface IWorkingStrategy
    {
        public void Process(Socket client, Socket listener, int x);
    }

    public class InitiatorWorkingStrategy : IWorkingStrategy
    {
        public void Process(Socket client, Socket listener, int x)
        {
            var bytes = BitConverter.GetBytes(x);
            client.Send(bytes);

            Socket handler = listener.Accept();
            
            var buf = new byte[4];
            handler.Receive(buf);
            var y = BitConverter.ToInt32(buf);

            x = y;
            Console.WriteLine(x);
            
            bytes = BitConverter.GetBytes(Math.Max(x, y));
            client.Send(bytes);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }

    public class NormalProcessWorkingStrategy : IWorkingStrategy
    {
        public void Process(Socket client, Socket listener, int x)
        {
            var handler = listener.Accept();

            var buf = new byte[4];
            handler.Receive(buf);
            var y = BitConverter.ToInt32(buf);

            var bytes = BitConverter.GetBytes(Math.Max(x, y));
            client.Send(bytes);

            buf = new byte[4];
            handler.Receive(buf);
            var receivedNumber = BitConverter.ToInt32(buf);
            Console.WriteLine(receivedNumber);
            
            client.Send(buf);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}
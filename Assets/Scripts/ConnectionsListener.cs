using System.Net;
using System.Net.Sockets;
using System.Text;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public class ConnectionsListener 
    {
        private const int Port = 8888;
        private static readonly IPAddress IP = IPAddress.Parse("127.0.0.1");
        private static readonly Encoding ConnectionEncoding = Encoding.UTF8;
        
        private readonly TcpListener listener;

        public ConnectionsListener()
        {
            listener = new TcpListener(IP, Port);
        }

        public void StartListening()
        {
        }

        private void ListeningTask()
        {
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var clientObject = new ClientObject(client);
                Debug.LogError("new client connected");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Scenes
{
    public class TestTCPServer : MonoBehaviour
    {
        private const int Port = 8888;
        private static readonly IPAddress IP = IPAddress.Parse("127.0.0.1");
        
        private TcpListener listener;
        private Task connectClientsTask;

        private void Start()
        {
            listener = new TcpListener(IP, Port);
            
            listener.Start();
            connectClientsTask = Task.Run(WaitAndProcessNewClients);
        }

        private void WaitAndProcessNewClients()
        {
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var clientObject = new ClientObject(client);
                Debug.LogError("new client connected");
                Task.Run(clientObject.Process);
            }
        }

        private void OnDestroy()
        {
            connectClientsTask.Dispose();
            listener.Stop();
        }
    }
}
using System;
using System.Net.Sockets;
using System.Text;
using Replication;
using UnityEngine;

namespace Scripts
{
    public class ClientObject
    {
        private readonly TcpClient client;
        
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }
 
        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64];
                while (client.Connected)
                {
                    string message;
                    do
                    {
                        var bytes = stream.Read(data, 0, data.Length);
                        message = Encoding.UTF8.GetString(data, 0, bytes);
                    }
                    while (stream.DataAvailable);

                    Debug.LogError(JsonUtility.FromJson<NetworkMessage>(message));
                }
                Debug.LogError("client disconnected");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }
    }
}
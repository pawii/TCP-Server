using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Replication;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public static class ClientHandler
    {
        public static IObservable<NetworkMessage> StartHandling(
            TcpClient client, 
            Encoding encoding, 
            CancellationToken token)
        {
            return Observable.Create<NetworkMessage>(
                observer =>
                {
                    var stream = client.GetStream();
                    var dataBuffer = new byte[1024];
                    while (!token.IsCancellationRequested && client.Connected)
                    {
                        if (stream.DataAvailable)
                        {
                            var bytesCount = stream.Read(dataBuffer, 0, dataBuffer.Length);
                            var stringMessage = encoding.GetString(dataBuffer, 0, bytesCount);
                            observer.OnNext(JsonUtility.FromJson<NetworkMessage>(stringMessage));
                        }
                    }
                    observer.OnCompleted();
                    return Disposable.Create(
                        () =>
                        {
                            stream.Close();
                            stream.Dispose();
                            client.Close();
                            client.Dispose();
                        });
                })
                .Do(
                    message => { },
                    ex => Debug.LogError(ex.Message),
                    () => Debug.Log("Client disconnected"));
        }
    }
}
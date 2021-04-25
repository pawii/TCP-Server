using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public static class ConnectionsListener 
    {
        public static IObservable<TcpClient> StartListeningAsObservable(
            CancellationToken cancellationToken, 
            IPAddress ip, 
            int port)
        {
            return Observable.Create<TcpClient>(
                observer =>
                {
                    var listener = new TcpListener(ip, port);
                    listener.Start();
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (listener.Pending())
                        {
                            observer.OnNext(listener.AcceptTcpClient());
                        }
                    }
                    observer.OnCompleted();
                    return Disposable.Create(() => listener.Stop()); 
                })
                .Do(
                    client => Debug.LogError("new tcp connection"),
                    () => Debug.LogError("finish listening"));
        }
    }
}

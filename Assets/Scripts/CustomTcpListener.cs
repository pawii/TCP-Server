using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public static class CustomTcpListener 
    {
        public static IObservable<TcpClient> StartListening(
            IPAddress ip, 
            int port,
            CancellationToken cancellationToken)
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
                    client => Debug.Log("new tcp connection"),
                    ex => Debug.LogError(ex.Message),
                    () => Debug.Log("finish listening"));
        }
    }
}

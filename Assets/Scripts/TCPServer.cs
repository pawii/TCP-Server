using System.Threading;
using Replication;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public class TCPServer : MonoBehaviour
    {
        [SerializeField] private ConnectionConfig connectionConfig;

        private CancellationTokenSource cts;
        
        private void Start()
        {
            Debug.Log($"Main Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            cts = new CancellationTokenSource();
            
            CustomTcpListener
                .StartListening(connectionConfig.IP, connectionConfig.Port, cts.Token)
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread()
                .Subscribe(
                    client =>
                    {
                        ClientHandler
                            .StartHandling(client, connectionConfig.Encoding, cts.Token)
                            .SubscribeOn(Scheduler.ThreadPool)
                            .ObserveOnMainThread()
                            .Subscribe(
                                networkMessage =>
                                {
                                    Debug.Log($"Thread: {Thread.CurrentThread.ManagedThreadId}; " +
                                                   $"Message: {networkMessage.ToString()}");
                                });
                    });
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.S))
            {
                cts.Cancel();
            }
        }

        private void OnDestroy()
        {
            cts.Cancel();
        }
    }
}
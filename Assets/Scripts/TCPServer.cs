using System.Net.Sockets;
using System.Threading;
using Replication;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public class TCPServer : MonoBehaviour
    {
        [SerializeField] private ConnectionConfig connectionConfig;
        [SerializeField] private NetworkMessagesHandler networkMessagesHandler;

        private CancellationTokenSource cts;
        
        private void Start()
        {
            Debug.Log($"Main Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            cts = new CancellationTokenSource();
            
            CustomTcpListener
                .StartListening(connectionConfig.IP, connectionConfig.Port, cts.Token)
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread()
                .Subscribe(HandleClient);
        }

        private void HandleClient(TcpClient client)
        {
            var clientScheduler = Scheduler.ThreadPool;
            
            ClientHandler
                .ListenMessages(client, connectionConfig.Encoding, cts.Token)
                .SubscribeOn(clientScheduler)
                .ObserveOnMainThread()
                .Subscribe(networkMessagesHandler.HandleMessage);
            
            ClientHandler
                .ValidatingConnection(client, cts.Token)
                .SubscribeOn(clientScheduler)
                .Subscribe();
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
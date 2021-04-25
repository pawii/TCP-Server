using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Replication;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public class TestTCPServer : MonoBehaviour
    {
        [SerializeField] private ConnectionConfig connectionConfig;

        private CancellationTokenSource cts;
        
        private void Start()
        {
            cts = new CancellationTokenSource();
            
            ConnectionsListener
                .StartListeningAsObservable(cts.Token, connectionConfig.IP, connectionConfig.Port)
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread()
                .Subscribe(
                    client =>
                    {
                        ClientAsObservable
                            .Process(client, connectionConfig.Encoding, cts.Token)
                            .SubscribeOn(Scheduler.ThreadPool)
                            .ObserveOnMainThread()
                            .Subscribe(
                                networkMessage =>
                                {
                                    var tId3 = Thread.CurrentThread.ManagedThreadId;
                                    Debug.LogError(networkMessage.ToString());
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
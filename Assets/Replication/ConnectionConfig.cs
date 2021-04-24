using System.Net;
using System.Text;
using UnityEngine;

namespace Replication
{
    [CreateAssetMenu(fileName = "ConnectionConfig", menuName = "Replication/ConnectionConfig")]
    public class ConnectionConfig : ScriptableObject
    {
        public int Port => 8888;
        public IPAddress IP => IPAddress.Parse("127.0.0.1");
        public Encoding Encoding => Encoding.UTF8;
    }
}

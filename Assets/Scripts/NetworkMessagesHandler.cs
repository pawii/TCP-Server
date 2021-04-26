using Replication;
using UnityEngine;

namespace Scripts
{
    public class NetworkMessagesHandler : MonoBehaviour
    {
        [SerializeField] private Light light;
        [SerializeField] private ParticleSystem explosionParticle;

        public void HandleMessage(NetworkMessage message)
        {
            switch (message)
            {
                case NetworkMessage.MakeExplosion:
                    MakeExplosion();
                    break;
                case NetworkMessage.TurnLightOn:
                    TurnLightOn();
                    break;
                case NetworkMessage.TurnLightOff:
                    TurnLightOff();
                    break;
                default:
                    Debug.LogError($"Unknown network message: {message}");
                    break;
            }
        }

        private void MakeExplosion()
        {
            explosionParticle.time = 0f;
            explosionParticle.Play(true);
        }
        
        private void TurnLightOn() => light.enabled = true;

        private void TurnLightOff() => light.enabled = false;
    }
} 

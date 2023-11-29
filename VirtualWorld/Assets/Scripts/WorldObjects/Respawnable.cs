using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldObjects
{
    public class Respawnable : NetworkBehaviour
    {
        [SerializeField] float minRespawnTime;
        [SerializeField] float maxRespawnTime;

        public void Despawn()
        {
            DespawnServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        void DespawnServerRpc()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            DespawnObserversRpc();
            StartCoroutine(RespawnTimer());
        }

        [ObserversRpc(BufferLast = true)]
        void DespawnObserversRpc()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        IEnumerator RespawnTimer()
        {
            
            float respawnTime = Random.Range(minRespawnTime, maxRespawnTime);
            yield return new WaitForSeconds(respawnTime);

            transform.GetChild(0).gameObject.SetActive(true);
            RespawnObserversRpc();
        }

        [ObserversRpc(BufferLast = true)]
        void RespawnObserversRpc()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }


    }
}


using Characters;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldObjects
{
    public class Respawnable : NetworkBehaviour
    {
        [SerializeField] float minRespawnTime;
        [SerializeField] float maxRespawnTime;

        [SyncVar] bool isEnabled;

        I_Interactable interactableChild;
        GameObject interactableGO;

        // On server start the object is always enabled
        public override void OnStartServer()
        {
            base.OnStartServer();
            isEnabled = true;
        }

        // On client start, check the SyncVar and disable/enable the gameobject accordingly
        public override void OnStartClient()
        {
            base.OnStartClient();

            transform.GetChild(0).gameObject.SetActive(isEnabled);
        }

        public void Despawn(I_Interactable interactable, GameObject interactableGO)
        {
            interactableChild = interactable;
            this.interactableGO = interactableGO;
            DespawnServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        void DespawnServerRpc()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            DespawnObserversRpc();
            StartCoroutine(RespawnTimer());
        }

        [ObserversRpc]
        void DespawnObserversRpc()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            PlayerEvents.Instance.CallEventInteractionEnded(interactableChild, interactableGO);
        }

        IEnumerator RespawnTimer()
        {
            isEnabled = false;
            float respawnTime = Random.Range(minRespawnTime, maxRespawnTime);
            yield return new WaitForSeconds(respawnTime);

            transform.GetChild(0).gameObject.SetActive(true);
            isEnabled = true;
            RespawnObserversRpc();
        }

        [ObserversRpc]
        void RespawnObserversRpc()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }


    }
}


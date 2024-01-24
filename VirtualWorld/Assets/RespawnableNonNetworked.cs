using Characters;
using System.Collections;
using UnityEngine;

namespace WorldObjects
{
    public class RespawnableNonNetworked : MonoBehaviour
    {
        [SerializeField] float minRespawnTime;
        [SerializeField] float maxRespawnTime;

        [SerializeField] private bool isEnabled;

        I_Interactable interactableChild;
        GameObject interactableGO;

        public void Start()
        {
            isEnabled = true;
            transform.GetChild(0).gameObject.SetActive(isEnabled);
        }


        //// On client start, check the SyncVar and disable/enable the gameobject accordingly
        //public override void OnStartClient()
        //{
        //    base.OnStartClient();

        //    transform.GetChild(0).gameObject.SetActive(isEnabled);
        //}

        public void Despawn(I_Interactable interactable, GameObject interactableGO)
        {
            interactableChild = interactable;
            this.interactableGO = interactableGO;
            Despawn();
        }


        void Despawn()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            DespawnAndCallEvent();
            StartCoroutine(RespawnTimer());
        }

        void DespawnAndCallEvent()
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
            Respawn();
        }

 
        void Respawn()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }


    }
}


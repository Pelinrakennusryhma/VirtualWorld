
using UnityEngine;
using UnityEngine.Events;
using WorldObjects;

namespace Characters
{
    public class DialogInteraction : MonoBehaviour, I_Interactable
    {
        [field: SerializeReference]
        public string DetectionMessage { get; set; }
        public Vector3 DetectionMessageOffSet { 
            get => offSet; 
            private set => offSet = value; 
        }
        private Vector3 offSet;

        NPC npc;

        void Start()
        {
            DetectionMessageOffSet = new Vector3(0, 1.5f, 0);
            npc = GetComponent<NPC>();
        }

        public void Interact(string playerId, UnityAction callback)
        {
            PlayerEvents.Instance.CallEventDialogOpened(npc);
        }
    }
}


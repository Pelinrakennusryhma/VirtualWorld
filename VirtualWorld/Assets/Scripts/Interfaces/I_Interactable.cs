using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace WorldObjects
{
    public interface I_Interactable
    {
        public string DetectionMessage { get; }
        public Vector3 DetectionMessageOffSet { get; }
        public void Interact(string playerId, UnityAction callback);
    }
}

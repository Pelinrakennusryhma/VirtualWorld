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
        // used for toggling the interaction prompt on and off
        public bool IsActive { get; }
        public void Interact(UnityAction onCompletionCallback);
    }
}

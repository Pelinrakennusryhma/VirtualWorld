using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface I_Interactable
{
    public string DetectionMessage { get; set; }
    public void Interact(string playerId);
}

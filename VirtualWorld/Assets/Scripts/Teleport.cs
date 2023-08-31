using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public string SceneName;

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.parent != null)
        {
            FirstPersonPlayerController fpsCtrl = other.transform.parent.GetComponent<FirstPersonPlayerController>();
            if (fpsCtrl != null)
            {
                fpsCtrl.OnTeleport(SceneName);
            }
        }
    }
}

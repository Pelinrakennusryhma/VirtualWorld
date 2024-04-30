using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Characters;

public class NetworkedFPSControllerInitializer : MonoBehaviour
{

    public ThirdPersonController Third;
    public void Initialize(GameObject parent,
                           ThirdPersonController third)
    {

        //Debug.LogError("INITIALIZING FPS CONTROLLER");
        Third = third;
        
       // Debug.LogError("Initting fps controller");

        //ThirdPersonController third = parent.GetComponent<ThirdPersonController>();

        third._mainCamera.SetActive(false);

        //third = CharacterManager.Instance.OwnedCharacter.GetComponentInChildren<ThirdPersonController>(true);

       // Debug.LogError("CAlling initialize");

        if (CharacterManager.Instance.OwnedCharacter != null) {
            //Debug.LogError("Caracter manager instance gameobject is " + CharacterManager.Instance.OwnedCharacter.gameObject.name);
        }

        else
        {
            //Debug.LogError("Null cahracter manager owned character");
        }

        FirstPersonPlayerControllerShooting fps = GetComponent<FirstPersonPlayerControllerShooting>();
        fps.ChangeColliderSize(third._controller);
        
        DisableThingsOnTrhid();
    }

    public void DisableThingsOnTrhid()
    {
        if (Third != null) 
        {
            Third.DisableThings();
        }
    }

    public void SyncFirstPersonPosToThirdPersonController(Vector3 pos,
                                                          Quaternion rot,
                                                          Vector3 prevPos,
                                                          bool jumped,
                                                          bool crouched)
    {
        if (Third != null) 
        {
            Third.MoveToPos(pos, 
                            rot, 
                            prevPos, 
                            jumped, 
                            crouched);
        }

        else
        {
            //Debug.LogError("Null third");
        }
    }
}

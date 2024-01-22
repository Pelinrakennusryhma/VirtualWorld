using Characters;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public class NetworkSceneInit : NetworkBehaviour
    {
        [field: SerializeField] public Transform PlayerCharacterSpawnSpot { get; private set; }

        //public override void OnStartClient()
        //{
        //    base.OnStartClient();

        //    //Debug.Log("moving character");

        //    //ThirdPersonController tpc = CharacterManager.Instance.OwnedCharacter.GetComponent<ThirdPersonController>();
        //    //StartCoroutine(Delay(tpc));

        //}

        //IEnumerator Delay(ThirdPersonController tpc)
        //{
        //    Debug.Log("CharacterManager.Instance.OwnedCharacter.transform.position" + CharacterManager.Instance.OwnedCharacter.transform.position);
        //    yield return new WaitForSeconds(0.5f);
        //    tpc.SetPosition(playerCharacterSpawnSpot.position);
        //    Debug.Log("CharacterManager.Instance.OwnedCharacter.transform.position" + CharacterManager.Instance.OwnedCharacter.transform.position);
        //}
    }
}


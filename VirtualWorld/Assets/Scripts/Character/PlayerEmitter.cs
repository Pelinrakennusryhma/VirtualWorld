using StarterAssets;
using UI;
using Mirror;
using UnityEngine;

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        //StarterAssetsInputs inputs;
        //bool controlsDisabled = false;
        //void Start()
        //{
        //    if (isLocalPlayer)
        //    {
        //        Character.Instance.SetPlayerGameObject(gameObject);
        //        UIManager.Instance.SetPlayerCharacter(gameObject);

        //        inputs = GetComponentInChildren<StarterAssetsInputs>();

        //        UIManager.Instance.EventMenuToggled.AddListener(TogglePlayerInputs);
        //    }
        //}

        //private void Update()
        //{
        //    if (controlsDisabled)
        //    {
        //        inputs.ZeroInputs();
        //    }
        //}

        //void TogglePlayerInputs(bool menuEnabled)
        //{
        //    controlsDisabled = menuEnabled;
        //    Debug.Log("Inputs enabled: " + !menuEnabled);
        //}

    }
}

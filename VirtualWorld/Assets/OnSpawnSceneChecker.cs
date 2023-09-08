using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class OnSpawnSceneChecker : NetworkBehaviour
{
    void Start()
    {
        if (!IsOwner)
        {


            // NOTE: if a client is playing a minigame and has active scene set to other than
            // the main scene, the spawn will happen to wrong scene and that is not too cool

            Scene mainScene = SceneManager.GetSceneByName(Scenes.SceneLoader.Instance.MainSceneName);

            if (SceneManager.GetActiveScene() != mainScene)
            {
                Scenes.SceneLoader.Instance.OnEnterNewPlayerWhileMainSceneIsInactive(gameObject);
                Debug.Log("On network spawn called. Character might be in the wrong scene, if the owner is playing a minigame. Migrating character to main scene");
            }
        }

        else
        {
            // Causes the game to freeze

            //Scene mainScene = SceneManager.GetSceneByName(Scenes.SceneLoader.Instance.MainSceneName);

            //if (SceneManager.GetActiveScene() != mainScene)
            //{
            //    SceneManager.MoveGameObjectToScene(gameObject, mainScene);
            //    Debug.Log("On network spawn called. Character might be in the wrong scene, if the owner is playing a minigame. Migrating character to main scene");
            //}
        }
    }
}

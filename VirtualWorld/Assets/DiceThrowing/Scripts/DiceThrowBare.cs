using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    public class DiceThrowBare : MonoBehaviour
    {
        [SerializeField] float forwardOffset = 4f;
        [SerializeField] CamMover camMover;
        [SerializeField] GameObject environments;
        void Start()
        {
            MoveObjects();
        }

        void MoveObjects()
        {
            if(SceneLoader.Instance == null)
            {
                return;
            }

            object sceneData = SceneLoader.Instance.sceneLoadParams.sceneData;

            // whether we're playing dicethrow on arcade or out in the world determines if the surroundings are shown
            if ((string)sceneData == "ShowWorlds")
            {
                environments.SetActive(true);

            } else
            {
                Vector3 offset = SceneLoader.Instance.sceneLoadParams.origo;
                Quaternion rotation = SceneLoader.Instance.sceneLoadParams.rotation;

                transform.position = transform.position + offset;
                transform.rotation = rotation;

                transform.position = transform.position + (transform.forward * forwardOffset);

                camMover.Init();

                environments.SetActive(false);
            }
        }
    }
}


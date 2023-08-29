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

            // if params are passed we are throwing dice out in the world, get vector data and disable the dice surroundings
            if (!SceneLoader.Instance.sceneLoadParams.nulled)
            {
                Vector3 offset = SceneLoader.Instance.sceneLoadParams.origo;
                Quaternion rotation = SceneLoader.Instance.sceneLoadParams.rotation;

                transform.position = transform.position + offset;
                transform.rotation = rotation;

                transform.position = transform.position + (transform.forward * forwardOffset);

                camMover.Init();

                environments.SetActive(false);
            } else
            {
                environments.SetActive(true);
            }
        }
    }
}


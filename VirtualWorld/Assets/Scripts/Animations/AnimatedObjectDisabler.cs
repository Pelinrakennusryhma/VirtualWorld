using FishNet.Component.Animating;
using FishNet.Component.Transforming;
using FishNet.Object;
using Scenes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using Characters;

namespace Animations
{
    // Because network animations break when the game object is disabled and enabled
    // this script is required on animated network objects.
    // Disable and Enable methods are called instead of toggling the gameobject off and on.
    public class AnimatedObjectDisabler : MonoBehaviour
    {
        List<CachedMonoBehaviour> monoBehaviours = new List<CachedMonoBehaviour>();
        List<CachedGameObject> childGameObjects = new List<CachedGameObject>();
        List<CachedCollider> colliders = new List<CachedCollider>();

        // Scripts that shouldn't get disabled to avoid anything network related breaking,
        // like networked animations
        List<MonoBehaviour> ignoredMonobehaviours = new List<MonoBehaviour>();

        void Start()
        {
            ignoredMonobehaviours.Add(GetComponent(typeof(Animator)) as MonoBehaviour);
            ignoredMonobehaviours.Add(GetComponent(typeof(NetworkAnimator)) as MonoBehaviour);
            ignoredMonobehaviours.Add(GetComponent(typeof(NetworkObject)) as MonoBehaviour);
            ignoredMonobehaviours.Add(GetComponent(typeof(NetworkTransform)) as MonoBehaviour);
            ignoredMonobehaviours.Add(this);
        }


        public void Enable()
        {
            foreach (CachedMonoBehaviour cachedMono in monoBehaviours)
            {
                cachedMono.mb.enabled = cachedMono.isEnabled;
            }
            monoBehaviours.Clear();

            foreach (CachedCollider cachedCollider in colliders)
            {
                cachedCollider.col.enabled = cachedCollider.isEnabled;
            }
            colliders.Clear();

            foreach (CachedGameObject cachedGO in childGameObjects)
            {
                cachedGO.gameObject.SetActive(cachedGO.isEnabled);
            }
            childGameObjects.Clear();


            //foreach (Collider collider in GetComponents<Collider>())
            //{
            //    if (collider is CharacterController)
            //    {
            //        collider.enabled = true;
            //        continue;
            //    }

            //    //colliders.Add(new CachedCollider(collider, collider.enabled));
            //    //collider.enabled = false;
            //}

            //CharacterController controller = GetComponentInChildren<CharacterController>(true);
            //controller.enabled = true;

        }

        public void Disable()
        {
            // Monobehaviours
            foreach (MonoBehaviour monoBehaviour in GetComponents<MonoBehaviour>())
            {
                if (!ignoredMonobehaviours.Contains(monoBehaviour))
                {
                    monoBehaviours.Add(new CachedMonoBehaviour(monoBehaviour, monoBehaviour.isActiveAndEnabled));
                    monoBehaviour.enabled = false;
                }
            }

            // Colliders
            foreach (Collider collider in GetComponents<Collider>())
            {
                if(collider is CharacterController)
                {
                    continue;
                }

                colliders.Add(new CachedCollider(collider, collider.enabled));
                collider.enabled = false;
            }

            // Child gameObjects
            foreach (Transform child in transform)
            {
                childGameObjects.Add(new CachedGameObject(child.gameObject, child.gameObject.activeSelf));
                child.gameObject.SetActive(false);
            }

            // In case of owned player character, stop animations from playing so we don't hear footsteps in minigame

            if (CharacterManager.Instance != null) 
            {
                if (gameObject == CharacterManager.Instance.OwnedCharacter)
                {
                    ThirdPersonController tpc = GetComponent<ThirdPersonController>();
                    if (tpc != null)
                    {
                        tpc.StopAnimations();
                    }
                }
            }

            else
            {
                if (gameObject == CharacterManagerNonNetworked.Instance.OwnedCharacter)
                {
                    ThirdPersonController tpc = GetComponent<ThirdPersonController>();
                    if (tpc != null)
                    {
                        tpc.StopAnimations();
                    }
                }
            }

            //CharacterController controller = GetComponentInChildren<CharacterController>(true);
            //controller.enabled = false;
        }
    }
}

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
        public CharacterController CharacterController;

        List<CachedMonoBehaviour> monoBehaviours = new List<CachedMonoBehaviour>();
        List<CachedGameObject> childGameObjects = new List<CachedGameObject>();
        List<CachedCollider> colliders = new List<CachedCollider>();

        // Scripts that shouldn't get disabled to avoid anything network related breaking,
        // like networked animations
        List<MonoBehaviour> ignoredMonobehaviours = new List<MonoBehaviour>();

        public GameObject BlockerCollider;

        public CapsuleCollider CapsuleCollider;

        void Start()
        {
            ignoredMonobehaviours.Add(GetComponent(typeof(Animator)) as MonoBehaviour);
            ignoredMonobehaviours.Add(GetComponent(typeof(NetworkAnimator)) as MonoBehaviour);
            ignoredMonobehaviours.Add(GetComponent(typeof(NetworkObject)) as MonoBehaviour);
            ignoredMonobehaviours.Add(GetComponent(typeof(NetworkTransform)) as MonoBehaviour);
            ignoredMonobehaviours.Add(this);

            //Debug.LogError("Animated object disabler gameobject name is " + gameObject.name);

            //if (CharacterController != null) 
            //{
            //    BlockerCollider = new GameObject();
            //    BlockerCollider.gameObject.name = "BlockerCollider";
            //    BlockerCollider.transform.parent = transform;
            //    CapsuleCollider capsule = BlockerCollider.AddComponent<CapsuleCollider>();
            //    capsule.radius = CharacterController.radius;
            //    capsule.height = CharacterController.height;
            //    capsule.center = CharacterController.center;

            //    BlockerCollider.layer = 8;
            //}

        }


        public void Enable()
        {
            Debug.LogError("Enabling. We are probably unpacking");

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

            //if (CharacterController != null)
            //{
            //    CharacterController.enabled = true;
            //}


           // Debug.LogError("About to enable colliders");

            // Colliders
            foreach (Collider collider in GetComponents<Collider>())
            {
                //Debug.LogError("We have a collider at gameobject" + gameObject.name);

                if (collider is CharacterController)
                {
                    //collider.enabled = true;
                    if (gameObject.GetComponentInChildren<PlayerEmitter>(true)) 
                    {
                        //Debug.Log("Character controller collider is enabled " + collider.enabled + " game object name is " + gameObject.name + " client id is " + GetComponentInChildren<PlayerEmitter>(true).GetClientID());
                    }
                    else
                    {
                        //Debug.LogError("Null player emitter");
                    }//continue;
                }
            }

            if (CapsuleCollider != null)
            {
                CapsuleCollider.enabled = true;
            }
        }

        public void Disable()
        {

            Debug.LogError("Disabling");
            monoBehaviours.Clear();
            colliders.Clear();
            childGameObjects.Clear();

            // Monobehaviours
            foreach (MonoBehaviour monoBehaviour in GetComponents<MonoBehaviour>())
            {
                if (!ignoredMonobehaviours.Contains(monoBehaviour))
                {
                    monoBehaviours.Add(new CachedMonoBehaviour(monoBehaviour, monoBehaviour.isActiveAndEnabled));
                    monoBehaviour.enabled = false;

                    Debug.LogError("disabled " + monoBehaviour.name);
                }
            }

            if (gameObject.GetComponentInChildren<PlayerEmitter>(true))
            {
                //Debug.LogError("About to disable colliders of client id " + GetComponentInChildren<PlayerEmitter>(true).GetClientID());
            }

            else
            {
                //Debug.LogError("Null player emitter");
            }

            // Colliders
            foreach (Collider collider in GetComponents<Collider>())
            {
                //Debug.LogError("We have a collider at gameobject" + gameObject.name);

                if (collider is CharacterController)
                {

                    //Debug.Log("Character controller collider is enabled " + collider.enabled + " gameobject name is " + gameObject.name);
                    continue;
                }

                colliders.Add(new CachedCollider(collider, collider.enabled));
                collider.enabled = false;
                Debug.LogError("disabled " + collider.name);
            }

            // Child gameObjects
            foreach (Transform child in transform)
            {
                childGameObjects.Add(new CachedGameObject(child.gameObject, child.gameObject.activeSelf));
                child.gameObject.SetActive(false);
                Debug.LogError("disabled " + child.name);
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

            //CharacterController controller = GetComponentInChildren<CharacterController>(true);
            //controller.enabled = false;

            //if (CharacterController != null)
            //{
            //    CharacterController.enabled = false;
            //}

            if (CapsuleCollider != null) 
            {
                CapsuleCollider.enabled = false;
            }
        }
    }
}

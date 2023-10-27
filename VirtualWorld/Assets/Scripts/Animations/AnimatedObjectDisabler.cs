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
    public class AnimatedObjectDisabler : MonoBehaviour
    {
        List<CachedMonoBehaviour> monoBehaviours = new List<CachedMonoBehaviour>();
        List<CachedGameObject> childGameObjects = new List<CachedGameObject>();
        List<CachedCollider> colliders = new List<CachedCollider>();

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

            foreach (CachedCollider cachedCollider in colliders)
            {
                cachedCollider.col.enabled = cachedCollider.isEnabled;
            }

            foreach (CachedGameObject cachedGO in childGameObjects)
            {
                cachedGO.gameObject.SetActive(cachedGO.isEnabled);
            }

        }

        public void Disable()
        {
            foreach (MonoBehaviour monoBehaviour in GetComponents<MonoBehaviour>())
            {
                if (!ignoredMonobehaviours.Contains(monoBehaviour))
                {
                    monoBehaviours.Add(new CachedMonoBehaviour(monoBehaviour, monoBehaviour.isActiveAndEnabled));
                    monoBehaviour.enabled = false;
                }
            }

            foreach (Collider collider in GetComponents<Collider>())
            {
                colliders.Add(new CachedCollider(collider, collider.enabled));
                collider.enabled = false;
            }

            foreach (Transform child in transform)
            {
                childGameObjects.Add(new CachedGameObject(child.gameObject, child.gameObject.activeSelf));
                child.gameObject.SetActive(false);
            }

            // In case of owned player character, stop animations from playing so we don't hear footsteps in minigame
            if(gameObject == CharacterManager.Instance.OwnedCharacter)
            {
                ThirdPersonController tpc = GetComponent<ThirdPersonController>();
                if (tpc != null)
                {
                    tpc.StopAnimations();
                }
            }
        }
    }
}

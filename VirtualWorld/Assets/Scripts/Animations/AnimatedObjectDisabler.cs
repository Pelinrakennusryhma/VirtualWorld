using FishNet.Component.Animating;
using Scenes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimatedObjectDisabler : MonoBehaviour
    {
        Animator _animator;
        NetworkAnimator _networkAnimator;
        List<CachedMonoBehaviour> monoBehaviours = new List<CachedMonoBehaviour>();
        List<CachedGameObject> childGameObjects = new List<CachedGameObject>();
        List<CachedCollider> colliders = new List<CachedCollider>();

        void Start()
        {
            _animator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();
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
                if (monoBehaviour != _animator && monoBehaviour != this && monoBehaviour != _networkAnimator)
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

            // In case of player character, stop animations from playing so we don't hear footsteps in minigame
            ThirdPersonController tpc = GetComponent<ThirdPersonController>();
            if(tpc != null)
            {
                tpc.StopAnimations();
            }
        }
    }
}

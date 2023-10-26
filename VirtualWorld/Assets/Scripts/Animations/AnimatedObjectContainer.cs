using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimatedObjectContainer : MonoBehaviour
    {
        public void EnableChildren()
        {
            foreach (Transform child in transform)
            {
                AnimatedObjectDisabler disabler = child.gameObject.GetComponent<AnimatedObjectDisabler>();
                if(disabler != null)
                {
                    disabler.Enable();
                }
            }
        }

        public void DisableChildren()
        {
            foreach (Transform child in transform)
            {
                AnimatedObjectDisabler disabler = child.gameObject.GetComponent<AnimatedObjectDisabler>();
                if (disabler != null)
                {
                    disabler.Disable();
                }
            }
        }
    }
}

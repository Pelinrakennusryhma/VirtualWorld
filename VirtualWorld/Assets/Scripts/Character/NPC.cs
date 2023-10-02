using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Characters
{
    public class NPC : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
        [SerializeField] TMP_Text nameplate;
        Animator _animator;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        void Start()
        {
            nameplate.text = Name;
            _animator = GetComponent<Animator>();
            AssignAnimationIDs();
            SetIdle();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void SetIdle()
        {
            _animator.SetBool(_animIDGrounded, true);
            _animator.SetFloat(_animIDSpeed, 0);
            _animator.SetFloat(_animIDMotionSpeed, 1);
        }
    }
}


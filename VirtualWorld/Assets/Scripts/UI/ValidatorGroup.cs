using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Authentication
{
    public class ValidatorGroup : MonoBehaviour
    {
        [SerializeField] TextFieldValidator[] validators;
        Button button;

        private void Start()
        {
            if(button == null)
            {
                button = GetComponent<Button>();
            }

            if(button == null)
            {
                Destroy(this);
            }
        }

        void Update()
        {
            if(validators.Length > 0)
            {
                bool valid = true;
                foreach (TextFieldValidator validator in validators)
                {
                    if (!validator.isValid)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    EnableButton();
                } else
                {
                    DisableButton();
                }

            } else
            {
                EnableButton();
            }
        }

        private void EnableButton()
        {
            button.interactable = true;
        }

        private void DisableButton()
        {
            button.interactable = false;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Authentication
{
    public class TextFieldValidator : MonoBehaviour, IDeselectHandler
    {
        [SerializeField] TMP_InputField field;
        [SerializeField] int minLength = 3;
        [SerializeField] string errorMessage;
        [SerializeField] GameObject errorObject;
        TMP_Text errorText;
        bool touched = false;
        [field: SerializeField] public bool isValid { get; private set; }

        void Start()
        {
            // Destroy script if no validation is asked
            if (minLength <= 0)
            {
                Destroy(this);
            }

            if(field == null)
            {
                field = GetComponentInChildren<TMP_InputField>();
            }

            if(errorMessage.Length > 0)
            {
                errorMessage = errorMessage.Replace("%%MIN_CHARS%%", minLength.ToString());
                errorText = errorObject.GetComponent<TMP_Text>();
                errorText.text = errorMessage;
            }

            field.onValueChanged.AddListener(Validate);

        }

        void OnEnable()
        {
            errorObject.SetActive(false);
            touched = false;
        }

        void Update()
        {
            if (!touched)
            {
                return;
            }

            if (isValid)
            {
                errorObject.SetActive(false);
            } else
            {
                errorObject.SetActive(true);
            }
        }

        public void Validate(string value)
        {
            touched = true;
            if(value.Length >= minLength)
            {
                isValid = true;
            } else
            {
                isValid = false;
            }

        }

        public void OnDeselect(BaseEventData eventData)
        {
            Validate(field.text);
        }
    }
}


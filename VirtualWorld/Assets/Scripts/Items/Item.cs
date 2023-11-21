using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName="Items/Item", order=0)]
    public class Item : ScriptableObject
    {
        [SerializeProperty("DisplayName")] // pass the name of the property as a parameter
        public string _displayName;

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                CreateId();
            }
        }

        [ReadOnly] public string Id;

        [field:SerializeField]
        [field:TextArea(3, 6)]
        public string Description { get; private set; }

        void CreateId()
        {
            if(_displayName == "")
            {
                Id = "";
                // Remove from db here
            } else
            {
                string _id = GetType().Name;
                _id += "-";
                _id += _displayName.Replace(" ", "");
                _id += "-";
                _id += Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                Id = _id;

                // Add to db here
            }


        }
    }
}


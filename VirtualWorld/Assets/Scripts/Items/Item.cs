using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        string oldId;

        [field:SerializeField]
        [field:TextArea(3, 6)]
        public string Description { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Double Value { get; private set; }

        ItemDatabase ItemDatabase
        {
#if UNITY_EDITOR
            get
            {
                return (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/Data/Items/ItemDatabase.asset", typeof(ItemDatabase));
            }
#else
            get
            {
                return null;
            }
#endif
        }

        private void OnDestroy()
        {
            ItemDatabase.RemoveItem(this);
        }

        void CreateId()
        {
            oldId = Id;
            if (_displayName == "")
            {
                Id = "";
                // Remove from db here
            } else
            {
                string _id = GetType().Name;
                _id += "-";                             
                // Type-

                _id += _displayName.Replace(" ", "");
                _id += "-";                             
                // Type-Name-

                _id += Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                Id = _id;                               
                // Type-Name-yhQWqwd89qw8HDqwh21

                // Add to db here
            }

            Save();

        }

        void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();

            ItemDatabase itemDatabase = (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/Data/Items/ItemDatabase.asset", typeof(ItemDatabase));

            itemDatabase.RemoveItem(this);

            if(_displayName != "")
            {
                itemDatabase.AddItem(this);
            }

#endif
        }

#region Reliable In-Editor OnDestroy
#if UNITY_EDITOR
        // Sadly OnDestroy is not being called reliably by the editor. So we need this.
        // Thanks to: https://forum.unity.com/threads/ondestroy-and-ondisable-are-not-called-when-deleting-a-scriptableobject-file.1129220/#post-7259671
        class OnDestroyProcessor : UnityEditor.AssetModificationProcessor
        {
            // Cache the type for reuse.
            static System.Type _type = typeof(Item);

            // Limit to certain file endings only.
            static string _fileEnding = ".asset";

            public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions _)
            {
                if (!path.EndsWith(_fileEnding))
                    return AssetDeleteResult.DidNotDelete;

                var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
                if (assetType != null && (assetType == _type || assetType.IsSubclassOf(_type)))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<Item>(path);
                    asset.OnDestroy();
                }

                return AssetDeleteResult.DidNotDelete;
            }
        }
#endif
#endregion
    }
}


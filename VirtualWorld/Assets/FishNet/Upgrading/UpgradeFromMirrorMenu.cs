#if UNITY_EDITOR
using FishNet.Documenting;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FishNet.Upgrading.FishNet.Editing
{

    /* IMPORTANT IMPORTANT IMPORTANT IMPORTANT 
    * If you receive errors about missing FishNet components,
    * such as NetworkIdentity, then remove FishNet and any other
    * FishNet defines.
    * Project Settings -> Player -> Other -> Scripting Define Symbols.
    * 
    * If you are also using my assets add FGG_ASSETS to the defines, and
    * then remove it after running this script. */
    [APIExclude]
    public class UpgradeFromFishNetMenu : MonoBehaviour
    {

        /// <summary>
        /// Replaces all components.
        /// </summary>
        [MenuItem("Fish-Networking/Upgrading/From FishNet/Replace Components", false, 2)]
        private static void ReplaceComponents()
        {
#if FishNet
            FishNetUpgrade result = GameObject.FindObjectOfType<FishNetUpgrade>();
            if (result != null)
            {
                Debug.LogError("FishNetUpgrade already exist in the scene. This suggests an operation is currently running.");
                return;
            }

            GameObject iteratorGo = new GameObject();
            iteratorGo.AddComponent<FishNetUpgrade>();
#else
            Debug.LogError("FishNet must be imported to perform this function.");
#endif
        }

        [MenuItem("Fish-Networking/Upgrading/From FishNet/Remove Defines", false, 2)]
        private static void RemoveDefines()
        {
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            /* Convert current defines into a hashset. This is so we can
             * determine if any of our defines were added. Only save playersettings
             * when a define is added. */
            HashSet<string> definesHs = new HashSet<string>();
            string[] currentArr = currentDefines.Split(';');

            bool removed = false;
            //Add any define which doesn't contain FishNet.
            foreach (string item in currentArr)
            {
                string itemLower = item.ToLower();
                if (itemLower != "FishNet" && !itemLower.StartsWith("FishNet_"))
                    definesHs.Add(item);
                else
                    removed = true;
            }

            if (removed)
            {
                Debug.Log("Removed FishNet defines to player settings.");
                string changedDefines = string.Join(";", definesHs);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, changedDefines);
            }
        }


    }
}
#endif

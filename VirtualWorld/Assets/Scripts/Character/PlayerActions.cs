using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using StarterAssets;

public class PlayerActions : MonoBehaviour
{
    StarterAssetsInputs inputs;
    List<IPlayerAction> actions = new List<IPlayerAction>();

    //string actionStringFormat = "action";
    //Dictionary<string, IPlayerAction> actionKeybinds = new Dictionary<string, IPlayerAction>();

    void Start()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        FindAllActions();
    }

    void Update()
    {
        if (inputs.action1)
        {
            actions[0].Execute();
        }
    }


    // ---------- On hold until solid input system ---------

    void FindAllActions()
    {
        var actionScripts = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerAction>();
        foreach (IPlayerAction actionScript in actionScripts)
        {
            actions.Add(actionScript);
        }
    }

    //void MapActionsToKeys()
    //{
    //    for (int i = 0; i < actions.Count; i++)
    //    {
    //        actionKeybinds.Add($"{actionStringFormat}{i + 1}", actions[i]);
    //    }
    //}

    //void ListenToInput()
    //{
    //    foreach (KeyValuePair<string, IPlayerAction> action in actionKeybinds)
    //    {

    //    }
    //}
}

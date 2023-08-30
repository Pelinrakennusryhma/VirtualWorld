using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using StarterAssets;

public class PlayerActions : MonoBehaviour
{
    [Tooltip("Delay before queued action is used, to prevent awkward behaviour after character lands.")]
    [SerializeField] float queuedActionExecuteDelay = 0.5f;
    StarterAssetsInputs inputs;
    ThirdPersonController thirdPersonController;
    List<IPlayerAction> actions = new List<IPlayerAction>();

    IPlayerAction queuedAction;
    IPlayerAction queuedDelayedAction;

    //string actionStringFormat = "action";
    //Dictionary<string, IPlayerAction> actionKeybinds = new Dictionary<string, IPlayerAction>();

    void Start()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        thirdPersonController = GetComponentInParent<ThirdPersonController>();
        FindAllActions();
    }

    private void OnEnable()
    {
        queuedAction = null;
        queuedDelayedAction = null;
    }

    void Update()
    {
        if (inputs.action1)
        {
            queuedAction = actions[0];
        }

        if (queuedAction != null)
        {
            if (CanExecute(queuedAction))
            {
                queuedAction.Execute();
            }
            else
            {
                queuedDelayedAction = queuedAction;
                queuedAction = null;
            }
        } else if (queuedDelayedAction != null)
        {
            if (CanExecute(queuedDelayedAction))
            {
                StartCoroutine(DelayExecute(queuedDelayedAction, queuedActionExecuteDelay));
                queuedDelayedAction = null;
            }

        }
    }

    bool CanExecute(IPlayerAction action)
    {
        if (!action.RequireGrounded)
        {
            return true;
        }
        else if (thirdPersonController.Grounded || !action.RequireGrounded)
        {
            return true;
        } else
        {
            return false;
        }
        //else
        //{
        //    if (queuedCoroutine != null)
        //    {
        //        StopCoroutine(queuedCoroutine);
        //    }

        //    queuedAction = null;
        //    queuedCoroutine = StartCoroutine(DelayExecute(action, queuedActionExecuteDelay));
        //}



        //if (!action.RequireGrounded || thirdPersonController.Grounded)
        //{
        //    if(queuedCoroutine != null)
        //    {
        //        StopCoroutine(queuedCoroutine);
        //    }

        //    queuedCoroutine = StartCoroutine(DelayExecute(action, queuedActionExecuteDelay));
        //}
    }

    IEnumerator DelayExecute(IPlayerAction action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Execute();
    }

    // ---------- Action Key mapping On hold until solid input system ---------

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

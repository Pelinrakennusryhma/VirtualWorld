using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverableGameModeButton : MonoBehaviour
{
    public GameModeInfo GameModeInfoElement;
    public GameFlowManager.GameMode GameMode;

    public void OnMouseEnter()
    {
        GameModeInfoElement.SetClarifier(GameMode);
        //Debug.Log("Mouse enter " + gameObject.name.ToString());
    }

}

using BackendConnection;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDice : MonoBehaviour, IPlayerAction
{
    public string ActionName { get => "Throw dice.";}

    public void Execute()
    {
        Debug.Log("executing throw dice");
        SceneLoader.Instance.LoadSceneByName("DiceThrowingBare", new SceneLoadParams(transform.position, transform.rotation, ScenePackMode.PLAYER_ONLY));
    }

}

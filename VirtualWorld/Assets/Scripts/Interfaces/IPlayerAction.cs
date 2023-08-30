using BackendConnection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    [Tooltip("Does the action require character to be on the ground(not jumping in air)?")]
    public bool RequireGrounded { get; }
    public string ActionName { get; }
    void Execute();
}

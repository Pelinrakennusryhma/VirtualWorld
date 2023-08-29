using BackendConnection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    public string ActionName { get; }
    void Execute();
}

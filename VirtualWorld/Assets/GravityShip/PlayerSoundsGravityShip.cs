using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsGravityShip : MonoBehaviour
{
    public void OnBoost()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayOnBoost();
    }

    public void OnDeath()
    {
        GameManagerGravityShip.Instance.SoundManager.PlayDeathSound();
    }
}

using Audio;
using TableTopInvaders;
using UnityEngine;

public class BallAudio : MonoBehaviour
{
    public void HitWall(float velocity)
    {
        // Velocity should be a variable in FMOD and it should be adjusted from this code instead of doing it here

        if (velocity > 12)
        {
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.HitWall1, Vector3.zero);
        }

        else if (velocity > 9)
        {
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.HitWall2, Vector3.zero);
        }

        else if (velocity > 6.0f)
        {
            AudioManager.Instance.PlayOneShot(FMODEventsTTI.Instance.HitWall3, Vector3.zero);
        }

        // Pitch is randomized in FMOD
    }
}

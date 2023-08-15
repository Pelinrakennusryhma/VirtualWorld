using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLights : MonoBehaviour
{
    public Light Light1;
    public Light Light2;

    public AnimationCurve LightCurve1;
    public AnimationCurve LightCurve2;

    public float MaxIntensity1;
    public float MaxIntensity2;

    public float MaxRange1;
    public float MaxRange2;

    public bool ComingIn;
    public bool ComingOut;

    public float ComeInTime;
    public float ComeOutTime;

    private float FadeInLength = 0.05f;
    private float FadeOutLength = 0.08f;

    public void Awake()
    {
        MaxIntensity1 = Light1.intensity;
        MaxIntensity2 = Light2.intensity;
        MaxRange1 = Light1.range;
        MaxRange2 = Light2.range;
        Light1.intensity = 0;
        Light1.range = 0;
        Light2.intensity = 0;
        Light2.range = 0;

    }
    // Start is called before the first frame update

    public void FireCannon()
    {
        ComingIn = true;
        ComeInTime = Time.time;
        ComingOut = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (ComingIn)
        {
            float ratio = (Time.time - ComeInTime) / FadeInLength;

            if (ratio > 1.0f)
            {
                ratio = 1.0f;

                ComingIn = false;
                ComingOut = true;
                ComeOutTime = Time.time;
            }

            Light1.intensity = LightCurve1.Evaluate(ratio) * MaxIntensity1;
            Light1.range = LightCurve1.Evaluate(ratio) * MaxRange1;
            Light2.intensity = LightCurve1.Evaluate(ratio) * MaxIntensity2;
            Light2.range = LightCurve1.Evaluate(ratio) * MaxRange2;
        }

        else if (ComingOut)
        {
            float ratio = (Time.time - ComeOutTime) / FadeOutLength;

            if (ratio > 1.0f)
            {
                ratio = 1.0f;

                ComingIn = false;
                ComingOut = false;
            }

            Light1.intensity = LightCurve2.Evaluate(ratio) * MaxIntensity1;
            Light1.range = LightCurve2.Evaluate(ratio) * MaxRange1;
            Light2.intensity = LightCurve2.Evaluate(ratio) * MaxIntensity2;
            Light2.range = LightCurve2.Evaluate(ratio) * MaxRange2;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickUp : PickUp
{    
    public MeshRenderer Cube001;
    public MeshRenderer Cylinder;    
    public MeshRenderer Sphere;
    public MeshRenderer Sphere001;
    public MeshRenderer Torus;


    public Color FadeStartColorCube001;    
    public Color FadeStartColorCylinder;
    public Color FadeStartColorSphere;
    public Color FadeStartColorSphere001;
    public Color FadeStartColorTorus;

    public override void Spawn()
    {
        base.Spawn();
        FadeStartColorCube001 = Cube001.material.color;
        FadeStartColorCylinder = Cylinder.material.color;
        FadeStartColorSphere = Sphere.material.color;
        FadeStartColorSphere001 = Sphere001.material.color;
        FadeStartColorTorus = Torus.material.color;
        FadingIn = true;
        GameFlowManager.Instance.SoundManager.PlaySound("Spawn time pickup ");
    }

    public override void Update()
    {
        base.Update();

        if (FadingIn && !hasDied)
        {
            FadeIn();
        }
    }

    public override void FadeIn()
    {
        base.FadeIn();

        float ratio = (Time.time - FadeInStartTime) / FadeInLenght;

        if (ratio >= 1.0f)
        {
            ratio = 1.0f;
            FadingIn = false;
        }

        Color cube001 = new Color(FadeStartColorCube001.r,
                                  FadeStartColorCube001.g,
                                  FadeStartColorCube001.b,
                                  ratio);

        Color cylinder = new Color(FadeStartColorCylinder.r,
                                   FadeStartColorCylinder.g,
                                   FadeStartColorCylinder.b,
                                   ratio);

        Color sphere = new Color(FadeStartColorSphere.r,
                                 FadeStartColorSphere.g,
                                 FadeStartColorSphere.b,
                                 ratio);

        Color sphere001 = new Color(FadeStartColorSphere001.r,
                                    FadeStartColorSphere001.g,
                                    FadeStartColorSphere001.b,
                                    ratio);

        Color torus = new Color(FadeStartColorTorus.r,
                                FadeStartColorTorus.g,
                                FadeStartColorTorus.b,
                                ratio);

        Cube001.material.color = cube001;
        Cylinder.material.color = cylinder;
        Sphere.material.color = sphere;
        Sphere001.material.color = sphere001;
        Torus.material.color = torus;
    }

    public override void ReactToBallHit()
    {
        base.ReactToBallHit();
        // Add time
        TimerCountdown.AddTime(10.0f);

        FadeStartColorCube001 = Cube001.material.color;
        FadeStartColorCylinder = Cylinder.material.color;
        FadeStartColorSphere = Sphere.material.color;
        FadeStartColorSphere001 = Sphere.material.color;
        FadeStartColorTorus = Torus.material.color;

        ToppleOverText.Spawn();
    }
}

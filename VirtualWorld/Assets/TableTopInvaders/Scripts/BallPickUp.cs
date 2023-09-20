using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPickUp : PickUp
{
    public MeshRenderer Torus;
    public MeshRenderer Cylinder;

    public Color FadeStartColorTorus1;
    public Color FadeStartColorTorus2;
    public Color FadeStartColorCylinder;

    public override void ReactToBallHit()
    {
        base.ReactToBallHit();
        // Add balls

        VasenTykki.kuuliaVasemmassaTykissä += 5;
        OikeaTykki.kuuliaOikeassaTykissä += 5;

        FadeStartColorTorus1 = Torus.materials[0].color;
        FadeStartColorTorus2 = Torus.materials[1].color;
        FadeStartColorCylinder = Cylinder.material.color;

        ToppleOverText.Spawn();

        //Debug.Log("Add balls");
    }

    public override void Spawn()
    {
        base.Spawn();
        FadeStartColorTorus1 = Torus.materials[0].color;
        FadeStartColorTorus2 = Torus.materials[1].color;
        FadeStartColorCylinder = Cylinder.material.color;
        FadingIn = true;

        GameFlowManager.Instance.SoundManager.PlaySound("Spawn ammo pickup ");
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

        Color torus1 = new Color(FadeStartColorTorus1.r,
                                 FadeStartColorTorus1.g,
                                 FadeStartColorTorus1.b,
                                 ratio);

        Color torus2 = new Color(FadeStartColorTorus2.r,
                                 FadeStartColorTorus2.g,
                                 FadeStartColorTorus2.b,
                                 ratio);

        Color cylinder = new Color(FadeStartColorCylinder.r,
                                   FadeStartColorCylinder.g,
                                   FadeStartColorCylinder.b,
                                   ratio);

        Torus.materials[0].color = torus1;
        Torus.materials[1].color = torus2;
        Cylinder.material.color = cylinder;
    }
}

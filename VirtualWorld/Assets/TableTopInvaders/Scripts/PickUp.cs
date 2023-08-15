using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool SpawnOnEasy;
    public bool SpawnOnMedium;
    public bool SpawnOnHard;


    public float SpawnTimeFromPhaseStart;
    public bool HasBeenSpawned;

    public GameObject GraphicsObject;

    public ParticleSystem Particles;
    public MeshRenderer MeshRenderer;
    public Collider MainCollider;

    protected bool hasDied;

    protected float fadeLength;
    protected float deathTime;
    protected Color fadeStartColor;
    protected bool HasDisabledMeshRenderer;

    protected bool FadingIn;
    protected float FadeInLenght;
    protected float FadeInStartTime;

    public ToppleOverText ToppleOverText;

    public PickupAudio PickupAudio;

    public void Awake()
    {
        ToppleOverText = GetComponentInChildren<ToppleOverText>();
        ToppleOverText.gameObject.SetActive(false);
        Particles.gameObject.SetActive(false);
        Particles.Stop(true);
        hasDied = false;
        fadeLength = 0.5f;
        HasDisabledMeshRenderer = false;
        //MeshRenderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent - 1; // Draw below the top glass
    }

    public virtual void Spawn()
    {
        HasBeenSpawned = true;
        FadingIn = true;
        FadeInStartTime = Time.time;
        FadeInLenght = 0.5f;
        gameObject.SetActive(true);
        PickupAudio.Spawn();
        //SetMaterialsToTransparent();
        //fadeStartColor = MeshRenderer.material.color;
    }

    public void OnCollisionEnter(Collision collision)
    {            
        if (collision.rigidbody == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Kuula")
            && collision.rigidbody.gameObject.GetComponent<Ball>().LastKnownVeloMagBeforeHit >= 4.0f)
        {
            ReactToBallHit();
        }
    }

    public void SpawnParticles()
    {
        Particles.gameObject.SetActive(true);
        Particles.Play(true);
    }

    public virtual void Update()
    {
        //if (FadingIn
        //    && !hasDied)
        //{
            //    if (FadeInLenght <= 0)
            //    {
            //        FadeInLenght = 0.000001f;
            //    }

            //    float ratio = (Time.time - FadeInStartTime) / FadeInLenght;

            //    Color currentColor = new Color(fadeStartColor.r,
            //                                   fadeStartColor.g,
            //                                   fadeStartColor.b,
            //                                   Mathf.Lerp(0, fadeStartColor.a, ratio));

            //    MeshRenderer.material.color = currentColor;
            //}

            //if (hasDied)
            //{
            //    // Just in case check, if someone messes up with the value
            //    if (fadeLength <= 0)
            //    {
            //        fadeLength = 0.0001f;
            //    }

            //    float ratio = (Time.time - deathTime) / fadeLength;



            //    Color currentColor = new Color(fadeStartColor.r, 
            //                                   fadeStartColor.g, 
            //                                   fadeStartColor.b,
            //                                   Mathf.Lerp(fadeStartColor.a, 0, ratio));

            //    MeshRenderer.material.color = currentColor;

            //    if (ratio >= 1.0f
            //        && !HasDisabledMeshRenderer)
            //    {
            //        MeshRenderer.enabled = false;
            //        HasDisabledMeshRenderer = true;
            //    }

            if (hasDied && Time.time >= deathTime + 3.6f)
            {
                Destroy(gameObject);
            }
        //}
    }

    public virtual void SetMaterialsToTransparent()
    {

    }

    public virtual void SetMaterialsToOpaque()
    {

    }

    public virtual void FadeIn()
    {

    }

    public virtual void FadeOut()
    {

    }

    public virtual void ReactToBallHit()
    {
        SpawnParticles();
        MainCollider.enabled = false;
        hasDied = true;
        deathTime = Time.time;
        GraphicsObject.SetActive(false);
        PickupAudio.Explode();
        GameFlowManager.Instance.SoundManager.PlaySound("pick up exploded");
        //fadeStartColor = MeshRenderer.material.color;
    }
}

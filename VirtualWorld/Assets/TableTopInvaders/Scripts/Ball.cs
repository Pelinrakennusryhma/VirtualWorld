using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody Rigidbody;

    public Material TransparentMaterial;
    public MeshRenderer Renderer;

    public float DeathTimer;
    private bool WaitingForRemoval;
    public float DeathStartTime;
    public float DeathLenght;

    private float soundHitTimer;

    public BallAudio BallAudio;

    public float LastKnownVeloMagBeforeHit;

    private void Awake()
    {
        if (PinPhaser.Instance != null) 
        {
            PinPhaser.Instance.AddBall(this);
        }

        else if (EndlessPinPhaser.Instance != null)
        {
            EndlessPinPhaser.Instance.AddBall(this);
        }

        transform.parent = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        LastKnownVeloMagBeforeHit = Rigidbody.velocity.magnitude;
    }

    private void SetMaterialToTransparent()
    {
        Renderer.material = TransparentMaterial;
    }

    private void Update()
    {
        soundHitTimer -= Time.deltaTime;

        if (WaitingForRemoval)
        {
            FadeOut();

            DeathTimer -= Time.deltaTime;

            if (DeathTimer <= 0)
            {
                if (PinPhaser.Instance != null)
                {
                    PinPhaser.Instance.RemoveBall(this);
                }

                else if (EndlessPinPhaser.Instance != null)
                {
                    EndlessPinPhaser.Instance.RemoveBall(this);
                }

                gameObject.SetActive(false);
            }
        }
    }

    public void StartRemoval(float time)
    {
        SetMaterialToTransparent();
        WaitingForRemoval = true;
        DeathTimer = time;
        DeathStartTime = Time.time;
        DeathLenght = time;
        //Debug.Log("Ball removed " + Time.time);
    }

    public void FadeOut()
    {
        float ratio = (Time.time - DeathStartTime) / DeathLenght;
        float alpha = 1.0f - ratio;

        Color newColor = new Color(Renderer.material.color.r,
                                   Renderer.material.color.g,
                                   Renderer.material.color.b,
                                   alpha);

        Renderer.material.color = newColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Wall"))
        {
            soundHitTimer = 0.3f;
            BallAudio.HitWall(LastKnownVeloMagBeforeHit);
            GameFlowManager.Instance.SoundManager.PlaySound("Ball hit wall at speed " + LastKnownVeloMagBeforeHit + " ");
        }
    }
}

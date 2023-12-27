using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunProjectile : MonoBehaviour
{
    public LaserGun OwnerLaserGun;
    public Rigidbody Rigidbody;

    public bool IsActiveInTheScene;

    public float LifeTime;

    public Vector3 freezePoint;

    SphereCollider sphereCollider;

    //public Collider[] collidersThatAreHitDuringInitialLaunch;

    public int FramesInCollision;

    public bool HasStopped;
    public TrailRenderer TrailRenderer;
    public float TrailRendererStartWidth;
    public float TrailRendererEndWidth;

    public AnimationCurve ShrinkCurve;

    public float StopStartTime;
    public float ShrinkLength;

    public float TrailLifeTime;

    public bool HasDisabledTrail;

    public ParticleSystem HitParticles;
    public GameObject Graphics;
    public Vector3 OriginalGraphicsObjectScale;

    private bool HasDealtDamageOnThisLaunch;
    public void Init(LaserGun owner)
    {
        ShrinkLength = 0.72f;
        LifeTime = 3.0f;
        OwnerLaserGun = owner;
        Rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        TrailRenderer = GetComponentInChildren<TrailRenderer>(true);
        TrailRendererStartWidth = TrailRenderer.startWidth;
        TrailRendererEndWidth = TrailRenderer.endWidth;
        TrailLifeTime = TrailRenderer.time;
        gameObject.SetActive(false);
        transform.position = owner.transform.position;
        HitParticles.Stop(true);
        OriginalGraphicsObjectScale = Graphics.transform.localScale;
    }

    public void OnLaunch(Vector3 originPos,
                         Vector3 launchDir)
    {
        //RaycastHit hitInfo;
        //bool hit = Physics.Raycast(originPos, launchDir.normalized * 30.0f, out hitInfo);

        //if (hit)
        //{
        //    Debug.Log("Hitting " + hitInfo.collider.gameObject.name + " at time " + Time.time);
        //}

        //TrailRenderer.startWidth = TrailRendererStartWidth;
        //TrailRenderer.endWidth = TrailRendererEndWidth;

        //Debug.Log("Trailrenderer start width is " + TrailRenderer.startWidth) ;

        //Debug.Log("On launch projectile. LaunchDir is " + launchDir);

        HasDealtDamageOnThisLaunch = false;
        HitParticles.Stop(true);
        HasDisabledTrail = false;
        TrailRenderer.gameObject.SetActive(true);
        TrailRenderer.time = TrailLifeTime;
        HasStopped = false;
        LifeTime = 3.0f;
        gameObject.transform.position = originPos;
        //gameObject.transform.position = originPos + launchDir.normalized * 0.7f;
        //Debug.Log("Origin pos is " + originPos);
        gameObject.SetActive(true);
        IsActiveInTheScene = true;
        Rigidbody.velocity = launchDir.normalized * 110.0f;
        Graphics.gameObject.SetActive(true);
        Graphics.gameObject.transform.localScale = OriginalGraphicsObjectScale;

        sphereCollider.enabled = true;

        //Debug.Log("Graphics object local scale is " + Graphics.gameObject.transform.localScale);

        //Physics.OverlapSphereNonAlloc(transform.position, sphereCollider.radius, collidersThatAreHitDuringInitialLaunch);

        //for (int i = 0; i < collidersThatAreHitDuringInitialLaunch.Length; i++)
        //{
        //    Debug.Log("Collider that was hit during initial launch is " + collidersThatAreHitDuringInitialLaunch[i].gameObject.name);
        //}

        //Rigidbody.AddForce(launchDir.normalized * 30.0f, ForceMode.Impulse);



        //Debug.Log("Launch projectile " + Time.time + " rigidbody velocity is " + Rigidbody.velocity);
    }

    public void ReturnToPool()
    {
        IsActiveInTheScene = false;
        gameObject.SetActive(false);
        transform.position = OwnerLaserGun.transform.position;
        OwnerLaserGun.ReturnObjectToPool(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.gameObject.CompareTag("Player"))
        {
            TrailRenderer.time = 0.1f;
            StopStartTime = Time.time;
            HasStopped = true;
            HasDisabledTrail = false;
            //ReturnToPool();
            //Debug.Break();

            Vector3 rigidbodyVelocityOnHit = Rigidbody.velocity;

            sphereCollider.enabled = false;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            HitParticles.Play(true);
            //Graphics.gameObject.SetActive(false);
            //Debug.Log("Laser gun projectile entered collision " + Time.time + " other is " + collision.collider.gameObject.name);

            HitDetector hitDetector = collision.collider.gameObject.GetComponent<HitDetector>();

            if (hitDetector != null
                && !HasDealtDamageOnThisLaunch)
            {
                hitDetector.OnHit(Health.DamageType.LaserGun);

                if (collision.rigidbody != null)
                {
                    // https://docs.unity3d.com/ScriptReference/Collision-contacts.html
                    // We can always be sure that contacts has at least one element, says the docs
                    // so we can just access the first one safely
                    hitDetector.AddForceToPos(collision.GetContact(0).point,
                                              collision.rigidbody.velocity,
                                              ForceMode.Impulse);
                }


                HasDealtDamageOnThisLaunch = true;
            }

            //Debug.Log("On collision enter called");
         }

    }



 



    public void Update()
    {
        if (IsActiveInTheScene)
        {
            LifeTime -= Time.deltaTime;

            if (LifeTime <= 0)
            {
                ReturnToPool();
            }
        }

        if (HasStopped)
        {
            if (!HasDisabledTrail 
                && (Time.time - StopStartTime) >= 0.1f)
            {
                TrailRenderer.gameObject.SetActive(false);
                HasDisabledTrail = true;
                //Debug.LogWarning("Disabled trail");
            }


            float ratio = (Time.time - StopStartTime) / ShrinkLength;

            if (ratio >= 1.0f)
            {
                Graphics.transform.localScale = Vector3.zero;
            }

            else
            {
                float valueAtCurve = ShrinkCurve.Evaluate(ratio);
                Vector3 newScale = new Vector3(OriginalGraphicsObjectScale.x * valueAtCurve,
                                               OriginalGraphicsObjectScale.y * valueAtCurve,
                                               OriginalGraphicsObjectScale.z * valueAtCurve);

                Graphics.transform.localScale = newScale;
            }

            //TrailRenderer.startWidth = 0;
                
            //TrailRenderer.endWidth = 0;


            //return;
            //if (ratio >= 1.0f) 
            //{

            //}

            //else
            //{
            //    TrailRenderer.startWidth = TrailRendererStartWidth * ShrinkCurve.Evaluate(ratio);
            //    TrailRenderer.endWidth = TrailRendererEndWidth * ShrinkCurve.Evaluate(ratio);            
            //    Debug.Log("Doing the stopped things " + Time.time);
            //}


        }
    }
}

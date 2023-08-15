using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{    
    public float SpawnTimeFromPhaseStart;
    public bool HasBeenSpawned;

    public RespawnToPreviousPositions.PinType PinType;
    public bool HasToppledOver;

    public Rigidbody Rigidbody;
    public MeshCollider MeshCollider;
    public CapsuleCollider CapsuleCollider;

    private float disappearanceTimer;
    private bool waitingToDisappear = false;

    private MeshRenderer[] AllMeshRenderers;

    public MeshRenderer BottomPlateRenderer;
    public MeshCollider BottomPlateCollider;

    private float fadeOutStartTime;
    private float fadeOutLength = 0.5f;
    private bool fadingOut;

    private bool fadingIn;
    private float fadeInLength = 0.5f;
    private float fadeInStartTime;

    private Vector3 stopPos;
    private Quaternion stopRot;

    public MeshRenderer PinBase;
    public MeshRenderer[] Numbers;

    public Material OpaqueBaseMaterial;
    public Material OpaqueNumbersMaterial;

    public Material TransparentBaseMaterial;
    public Material TransparentNumbersMaterial;

    public ToppleOverText ToppleOverText;

    private float ballHitSoundTimer;
    private PinSounds pinSounds;


    private Vector3 pinObjectOriginalPosition;
    private Quaternion pinObjectOriginalRotation;

    public PinToppleOver PinToppleOver;


    public void Awake()
    {
        pinObjectOriginalPosition = PinToppleOver.transform.position;
        pinObjectOriginalRotation = PinToppleOver.transform.rotation;
        //Debug.Log("Awake called " + Time.time);

        HasToppledOver = false;
        fadingOut = false;
        ToppleOverText = GetComponentInChildren<ToppleOverText>();
        ToppleOverText.gameObject.SetActive(false);

        // We just set toppleovertext to inactive, so we don't find it's meshrenderer while fetching children. This is wanted.
        AllMeshRenderers = Rigidbody.GetComponentsInChildren<MeshRenderer>();
        pinSounds = GetComponentInChildren<PinSounds>(true);
        //for (int i = 0; i < AllMeshRenderers.Length; i++)
        //{
        //    SetMaterialToTransParent(AllMeshRenderers[i].material);
        //}
    }

    public void Spawn()
    {
         PinToppleOver.transform.position = pinObjectOriginalPosition;
         PinToppleOver.transform.rotation = pinObjectOriginalRotation;

        //Debug.Log("Spawn called " + Time.time);
        HasToppledOver = false;
        HasBeenSpawned = true;
        fadeInStartTime = Time.time;
        fadingIn = true;
        SetMaterialsToTransparent();
        Rigidbody.gameObject.SetActive(true);
        BottomPlateCollider.gameObject.SetActive(true);
        MeshCollider.enabled = true;
        //CapsuleCollider.enabled = true;
        Rigidbody.constraints = RigidbodyConstraints.None;
        Rigidbody.useGravity = true;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;


        pinSounds.Spawn();
        GameFlowManager.Instance.SoundManager.PlaySound("Spawn pin " + PinType.ToString());
    }

    public void SetMaterialsToTransparent()
    {
        // We swap materials, because the below solution doesn't work in build for some reason.

        BottomPlateRenderer.material = TransparentNumbersMaterial;
        PinBase.material = TransparentBaseMaterial;

        for (int i = 0; i < Numbers.Length; i++)
        {
            Numbers[i].material = TransparentNumbersMaterial;
        }
   }

    public void SetMaterialsToOpaque()
    {
        BottomPlateRenderer.material = OpaqueNumbersMaterial;
        PinBase.material = OpaqueBaseMaterial;

        for (int i = 0; i < Numbers.Length; i++)
        {
            Numbers[i].material = OpaqueNumbersMaterial;
        }
    }

    public void SetMaterialToTransparent(Material material)
    {
        // https://forum.unity.com/threads/change-rendering-mode-via-script.476437/

        // This solution doesn't work in build for some reason, if a opaque or even transparent material is modified like this 

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent - 1;   // Cahnge this to transparent minus one to avoid putting objects randomly on top of transparent top glass.     
    }

    public void ToppleOver()
    {
        if (!HasToppledOver
            && TimerCountdown.peliVoiAlkaa)
        {
            switch (PinType)
            {
                case RespawnToPreviousPositions.PinType.None:
                    break;
                case RespawnToPreviousPositions.PinType.Minus2:
                    PisteLaskuri.pisteet += -2;
                    break;
                case RespawnToPreviousPositions.PinType.Minus5:
                    PisteLaskuri.pisteet += -5;
                    break;
                case RespawnToPreviousPositions.PinType.Plus1:
                    PisteLaskuri.pisteet += 1;
                    break;
                case RespawnToPreviousPositions.PinType.Plus2:
                    PisteLaskuri.pisteet += 2;
                    break;
                case RespawnToPreviousPositions.PinType.Plus5:
                    PisteLaskuri.pisteet += 5;
                    break;
                case RespawnToPreviousPositions.PinType.Plus10:
                    PisteLaskuri.pisteet += 10;
                    break;
                default:
                    break;
            }
        }

        HasToppledOver = true;
        ToppleOverText.Spawn();
    }

    public void OnCollision(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Lattia"))
        {
            ToppleOver();
            //Debug.Log("Pin " + PinType.ToString() + " has toppled over " + Time.time);
            waitingToDisappear = true;
            disappearanceTimer = 1.5f;
        }

        else if (collision.collider.gameObject.CompareTag("Kuula")
                 && ballHitSoundTimer <= 0)
        {
            ballHitSoundTimer = 0.3f;
            float speed = collision.collider.GetComponent<Ball>().LastKnownVeloMagBeforeHit;
            pinSounds.BallHitPin(speed);
            GameFlowManager.Instance.SoundManager.PlaySound("Ball hit pin " + PinType.ToString() + " at speed " + speed + " ");
        }
    }

    public void OnHitTheGround(Collider other)
    {

    }

    private void Update()
    {
        ballHitSoundTimer -= Time.deltaTime;

        if (waitingToDisappear)
        {
            disappearanceTimer -= Time.deltaTime;

            if(disappearanceTimer <= 0)
            {
                //Rigidbody.gameObject.SetActive(false);
                stopPos = transform.position;
                stopRot = transform.rotation;
                fadingOut = true;
                fadeOutStartTime = Time.time;
                MeshCollider.enabled = false;
                CapsuleCollider.enabled = false;
                Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                Rigidbody.useGravity = false;

                waitingToDisappear = false;

                //for (int i = 0; i < AllMeshRenderers.Length; i++)
                //{
                //    SetMaterialToTransparent(AllMeshRenderers[i].material);
                //}

                //// Set transparency to bottom plate too.
                //SetMaterialToTransparent(BottomPlateRenderer.material);

                SetMaterialsToTransparent();
                pinSounds.Die();
                GameFlowManager.Instance.SoundManager.PlaySound("Pin fading out");
            }
        }

        if (fadingOut)
        {
            float ratio = (Time.time - fadeOutStartTime) / fadeOutLength;
            float alpha = 1.0f - ratio;

            for (int i = 0; i < AllMeshRenderers.Length; i++)
            {
                Color newColor = new Color(AllMeshRenderers[i].material.color.r,
                                           AllMeshRenderers[i].material.color.g,
                                           AllMeshRenderers[i].material.color.b,
                                           alpha);

                AllMeshRenderers[i].material.color = newColor;
            }

            Color bottomPlateColor = new Color(BottomPlateRenderer.material.color.r,
                                               BottomPlateRenderer.material.color.g,
                                               BottomPlateRenderer.material.color.b,
                                               alpha);

            BottomPlateRenderer.material.color = bottomPlateColor;

            if (ratio >= 1.0f)
            {
                Rigidbody.gameObject.SetActive(false);
                BottomPlateCollider.gameObject.SetActive(false);
                fadingOut = false;
            }

        }

        if (fadingIn)
        {

            float ratio = (Time.time - fadeInStartTime) / fadeInLength;
            float alpha = ratio;

            for (int i = 0; i < AllMeshRenderers.Length; i++)
            {
                Color newColor = new Color(AllMeshRenderers[i].material.color.r,
                                           AllMeshRenderers[i].material.color.g,
                                           AllMeshRenderers[i].material.color.b,
                                           alpha);

                AllMeshRenderers[i].material.color = newColor;
            }

            Color bottomPlateColor = new Color(BottomPlateRenderer.material.color.r,
                                               BottomPlateRenderer.material.color.g,
                                               BottomPlateRenderer.material.color.b,
                                               alpha);

            BottomPlateRenderer.material.color = bottomPlateColor;

            if (ratio >= 1.0f)
            {
                Rigidbody.gameObject.SetActive(true);
                BottomPlateCollider.gameObject.SetActive(true);
                fadingIn = false;
                SetMaterialsToOpaque();
            }
        }
    }
}

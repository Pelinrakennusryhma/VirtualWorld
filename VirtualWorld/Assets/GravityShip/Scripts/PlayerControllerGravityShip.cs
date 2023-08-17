using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGravityShip : MonoBehaviour
{
    public enum ShipType
    {
        None = 0,
        ShipOne = 1,
        ShipTwo = 2
    }

    public static ShipType TypeOfShip;

    public PlayerSoundsGravityShip PlayerSounds;

    public GameObject ShipOneModel;
    public GameObject ShipTwoModel;

    public static PlayerControllerGravityShip Instance;

    public Rigidbody Rigidbody;

    public GravityAreaTrigger[] GravityAreas;

    private float FreeFromGravityTime;
    private Vector3 VeloWithoutGravity;    
    private bool SpaceWasPressedDuringLastUpdate;
    private Vector2 movement;

    [SerializeField] private BoostParticles BoostParticles;

    private float BoostCooldown;

    public ParticleSystem DeathParticles;

    public bool IsDead = false;

    public NewMiniGameInputs Inputs;

    public bool SpaceReleased;

    public static void SetShipType(ShipType type)
    {
        TypeOfShip = type;
        //Debug.Log("Ship is " + TypeOfShip.ToString());
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this; 
        GravityAreas = FindObjectsOfType<GravityAreaTrigger>();




    }

    private void Start()
    {
        //Debug.Log("Should activate the correct ship type here!");
        Rigidbody.AddForce(Rigidbody.transform.forward * 5, ForceMode.Impulse);
        IsDead = false;
        DeathParticles.Stop(true);

        ShipOneModel.SetActive(false);
        ShipTwoModel.SetActive(false);

        if (TypeOfShip == ShipType.ShipOne)
        {
            ShipOneModel.SetActive(true);
        }

        else if (TypeOfShip == ShipType.ShipTwo)
        {
            ShipTwoModel.SetActive(true);
        }

        else
        {
            ShipOneModel.SetActive(true);
            //Debug.LogError("No ship type selected");
        }
    }

    public void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        BoostCooldown -= Time.deltaTime;
        FreeFromGravityTime -= Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0, 
                                               Rigidbody.transform.eulerAngles.y + movement.x * Time.deltaTime * 60.0f, 
                                               0);
        Rigidbody.MoveRotation(rotation);

        if (SpaceWasPressedDuringLastUpdate)
        {
            Rigidbody.AddForce(Rigidbody.transform.forward * (Rigidbody.velocity.magnitude + 5.0f), ForceMode.Impulse);
            //Debug.Log("Adding force");
            SpaceWasPressedDuringLastUpdate = false;
            FreeFromGravityTime = 0.4f;
            BoostParticles.Boost();
            BoostCooldown = 0.5f;
            PlayerSounds.OnBoost();
        }


        if (FreeFromGravityTime <= 0)
        {
            for (int i = 0; i < GravityAreas.Length; i++)
            {
                Vector3 toGravityArea = GravityAreas[i].gameObject.transform.position - transform.position;
                float distance = toGravityArea.magnitude;

                if (distance <= GravityAreas[i].Gravity)
                {

                    float ratio = (distance / GravityAreas[i].Gravity);
                    Mathf.Clamp(ratio, 0, 1.0f);
                    Vector3 towardsPlanet = (GravityAreas[i].gameObject.transform.position - transform.position).normalized;
                    //Vector3 gravityRot = towardsPlanet + Vector3.right * towardsPlanet.magnitude;

                    VeloWithoutGravity = Rigidbody.velocity - (towardsPlanet * 300.0f * ratio);

                    VeloWithoutGravity = Vector3.Lerp(VeloWithoutGravity, 
                                                      VeloWithoutGravity.normalized 
                                                      * towardsPlanet.magnitude * ratio, 
                                                      3000.99f * Time.deltaTime);
                    //Rigidbody.velocity = VeloWithoutGravity;
                    Rigidbody.AddForce(towardsPlanet * 300.0f * ratio);

                    
                    //float ratio = (distance / GravityAreas[i].Gravity);
                    //Mathf.Clamp(ratio, 0, 1.0f);
                    //Vector3 towardsPlanet = (GravityAreas[i].gameObject.transform.position - transform.position);

                    ////Vector3 gravityRot = towardsPlanet + Vector3.right * towardsPlanet.magnitude;
                    //Rigidbody.AddForce(towardsPlanet.normalized * Mathf.Pow(distance, 2) * 0.5f *  GravityAreas[i].Gravity);
                }

                else
                {
                    VeloWithoutGravity = Rigidbody.velocity;
                }
            }

            float veloMagClamped = Mathf.Clamp(Rigidbody.velocity.magnitude, 0.0f, 80.0f);


            Rigidbody.velocity = Rigidbody.velocity.normalized * veloMagClamped;

            //Debug.DrawRay(Rigidbody.transform.position, VeloWithoutGravity * 10.0f);
        }

        if (SpaceWasPressedDuringLastUpdate)
        {
            Rigidbody.AddForce(Rigidbody.transform.forward * (Rigidbody.velocity.magnitude + 5.0f), ForceMode.Impulse);
            //Debug.Log("Adding force");
            SpaceWasPressedDuringLastUpdate = false;
            FreeFromGravityTime = 0.4f;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Inputs.escOptions)
        {
            GameManagerGravityShip.Instance.GoToTitleScreen();
            Inputs.ClearEscOptions();
        }

        if (IsDead)
        {
            return;
        }

        movement = new Vector2(Inputs.move.x, Inputs.move.y);

        if (!Inputs.jump)
        {
            SpaceReleased = true;
            //Debug.Log("Released space");
        }

        if (Inputs.jump && SpaceReleased)
        {
            SpaceWasPressedDuringLastUpdate = true;
            SpaceReleased = false;
            //Debug.Log("Pressed space");
        }



        //movement = new Vector2(Input.GetAxisRaw("Horizontal"),
        //                       Input.GetAxisRaw("Vertical"));

        //if (Input.GetButtonDown("Jump"))
        //{
        //    SpaceWasPressedDuringLastUpdate = true;
        //}

    }

    public void EnteredGravityTriggerArea(GravityAreaTrigger area)
    {

        Debug.Log("Entered gravity trigger area " + Time.time);
    }

    public void OnPlayerDeath()
    {
        IsDead = true;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;

        DeathParticles.transform.parent = null;
        DeathParticles.Play(true);        
        gameObject.SetActive(false);
        PlayerSounds.OnDeath();
    }

    public void WarpWithWormHole(Vector3 targetPos)
    {
        transform.position = targetPos;
    }
}

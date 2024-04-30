using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using FishNet.Object;
using StarterAssets;

public class FirstPersonSpawner : NetworkBehaviour
{
    public GameObject FPSPrefab;
    public GameObject SpawnedObject;

    public ThirdPersonController third;

    public bool FirstSpawn;

    public override void OnStartClient()
    {
        if (IsOwner)
        {
            base.OnStartClient();
            CharacterManager.Instance.SetFirstPersonSpawner(this);
        }

    }

    public void OnSpawnFirsPersonController()
    {
        if (SpawnedObject == null) 
        {
            //Debug.LogError("SPAWN FPS CONTROLLEr");


            //Debug.LogError("Spawning first person controller");

            SpawnedObject = Instantiate(FPSPrefab);
            //SpawnedObject.transform.position = transform.position;
            SpawnedObject.transform.localPosition = Vector3.zero;

            //Debug.LogError("Parent transform position is " + transform.position);

            //Debug.LogError("Spawned object transfrom position is " + SpawnedObject.transform.position);

            //Debug.LogError("Spawned object transfrom local position is " + SpawnedObject.transform.localPosition);

            NetworkedFPSControllerInitializer initializer = SpawnedObject.GetComponent<NetworkedFPSControllerInitializer>();
            initializer.Initialize(gameObject, third);

            FirstSpawn = true;
        }
        
    }

    public void SetControllerSpawnPositionAndRotation(Vector3 pos,
                                                      Quaternion rot,
                                                      UnityEngine.SceneManagement.Scene sceneToMoveTo)
    {
        if (SpawnedObject != null) 
        {
            SpawnedObject.transform.position = pos;
            SpawnedObject.transform.rotation = rot;

            //Debug.LogError("Setting controller spawn pos and rot. Pos is " + SpawnedObject.transform.position + " Rot is " + SpawnedObject.transform.rotation);

            //SpawnedObject.GetComponent<FirstPersonPlayerControllerShooting>().DisablePhysicsTest();

            //Debug.LogError("Should move the fps controller to scene " + sceneToMoveTo.name);

            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, sceneToMoveTo);

            SpawnedObject.gameObject.layer = 8;
            SpawnedObject.GetComponentInChildren<LaserGun>(true).MoveProjectilesToScene(sceneToMoveTo);

            ThirdPersonController third = gameObject.GetComponent<ThirdPersonController>();
            third.DisablePhysicsAndAll();

            CapsuleCollider standingCapsule = SpawnedObject.GetComponent<FirstPersonPlayerControllerShooting>().StandingCapsuleCollider;

            Collider[] overlaps = Physics.OverlapCapsule((SpawnedObject.transform.position + Vector3.up * standingCapsule.height / 2),
                                                         (SpawnedObject.transform.position + Vector3.down * standingCapsule.height / 2),
                                                         standingCapsule.radius);

            for (int i = 0; i < overlaps.Length; i++)
            {
                //Debug.LogError("Overlapping on spawn with " + overlaps[i].gameObject.name);
            }


            //Debug.LogError("aBOUT TO CHECK OVERLAPS WITH CONTROLLERS. POS IS " + SpawnedObject.transform.position);

            CharacterController[] controllers = FindObjectsOfType<CharacterController>(true);

            for (int i = 0; i < controllers.Length; i++)
            {
                if ((SpawnedObject.transform.position - controllers[i].transform.position).magnitude <= 2.5f)
                {
                    SpawnedObject.transform.position = controllers[i].transform.position + Vector3.up * 3.0f;
                    //Debug.LogError("Trying to avoid overlapping on spawn with " + controllers[i].gameObject.name);
                }
            }

            //Debug.LogError("DONE CHECKING OVERLAPS WITH CONTROLLLERS. POS IS " + SpawnedObject.transform.position);
        }
    }

    public void Start()
    {
        if (SpawnedObject != null) 
        {
            //Debug.LogError("ON START POS AND ROT. Pos is " + SpawnedObject.transform.position + " Rot is " + SpawnedObject.transform.rotation);
        }
    }

    public void LateUpdate()
    {
        FirstSpawn = false;
    }

    public void OnShouldDestroy()
    {
        Destroy(SpawnedObject.gameObject);
        
        //Debug.LogError("Destroying spawned object ");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (FirstSpawn)
        {
            //transform.position += collision.collider.gameObject.transform.position + Vector3.up * 5.0f;
            //FirstSpawn = false;

            //Debug.LogError("Moving on spawn");
        }
    }
}

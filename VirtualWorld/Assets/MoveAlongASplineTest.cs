using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class MoveAlongASplineTest : MonoBehaviour
{
    public SplineContainer[] Splines;

    public Rigidbody Rigidbody;

    public GameObject NearestPointVisualizer;

    public Camera BoatCamera;

    private Vector3 waterCurrentVelocity;
    private Vector3 raftFreeVelocity;

    public Vector2 Inputs;

    public WaterInfo[] WaterInfos;

    private RaycastHit[] hits = new RaycastHit[32];

    private bool hasReachedStopArea;

    public GameObject RaftGraphics;

    private float previousGoodYHit;

    private Vector3 lastRaftRigidbodyPosition;
    private Vector3 predictedRaftRigidbodyPosition;
    private float timeOfLastFixedUpdate;
    private float graphicsYOffset;
    private Vector3 lastRaftGraphicsPosition;

    [SerializeField] private RaftControls RaftControls;

    Vector3 lastPos;
    float totalDistanceTravelled;

    void Start()
    {  
        Splines = FindObjectsOfType<SplineContainer>();
        WaterInfos = FindObjectsOfType<WaterInfo>();
        hasReachedStopArea = false;
        graphicsYOffset = RaftGraphics.transform.position.y - Rigidbody.transform.position.y;
        lastPos = transform.position;
    }


    void FixedUpdate()
    {


        //Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"),
        //                               Input.GetAxisRaw("Vertical"));

        Vector2 movement = new Vector2(RaftControls.Horizontal,
                                       RaftControls.Vertical);

        Inputs = movement;

        // Stolen from ShowNearestPoint.cs that is an example script
        var nearest = new float4(0, 0, 0, float.PositiveInfinity);
        float interp = 0;
        Spline currentSpline = null;

        float waterCurrentStrength = 1.0f;
        //foreach (var container in Splines)
        //{
        //    using var native = new NativeSpline(container.Spline, container.transform.localToWorldMatrix);
        //    float d = SplineUtility.GetNearestPoint(native, transform.position, out float3 p, out float t);
        //    if (d < nearest.w)
        //        nearest = new float4(p, d);
        //    currentSpline = container.Spline;
        //    interp = t;
        //}

        foreach (var waterInfo in WaterInfos)
        {
            using var native = new NativeSpline(waterInfo.SplineContainer.Spline, waterInfo.SplineContainer.transform.localToWorldMatrix);
            float d = SplineUtility.GetNearestPoint(native, transform.position, out float3 p, out float t);
            
            if (d < nearest.w) 
            {
                nearest = new float4(p, d);
                currentSpline = waterInfo.SplineContainer.Spline;
                interp = t;
                waterCurrentStrength = waterInfo.WaterCurrentStrength;
            } 
        }

        float3 direction = SplineUtility.EvaluateTangent(currentSpline, interp);

        Vector3 dir = direction.xyz;

        Debug.DrawRay(nearest.xyz, dir);

        Vector3 xzVelo = new Vector3(Rigidbody.velocity.x,
                                     0,
                                     Rigidbody.velocity.z);

        float angle = Vector3.Angle(xzVelo.normalized, dir.normalized);
        float ratio = angle / 180.0f;

        float angleMultiplier = Mathf.Lerp(2, 360, ratio);


        //Rigidbody.velocity += dir.normalized * 0.4f * angleMultiplier * angleMultiplier * Time.deltaTime;

        //Rigidbody.velocity = Rigidbody.velocity.normalized * Mathf.Clamp(Rigidbody.velocity.magnitude, 0, 3);

        //Rigidbody.velocity += new Vector3(movement.x, 0, movement.y) * 460.5f * Time.deltaTime;


        waterCurrentVelocity += waterCurrentStrength * dir.normalized * 0.4f * angleMultiplier * angleMultiplier * Time.deltaTime;

        //waterCurrentVelocity= waterCurrentVelocity.normalized * Mathf.Clamp(waterCurrentVelocity.magnitude, 0, 3);
        waterCurrentVelocity = waterCurrentVelocity.normalized * Mathf.Clamp(waterCurrentVelocity.magnitude, 0, waterCurrentStrength * 3.0f);

        //raftFreeVelocity += new Vector3(movement.x, 0, movement.y) * 2.4f * Time.deltaTime;

        if (movement.y < -0.1f)
        {
            movement = new Vector2(movement.x, -0.25f);
        }

        Vector3 forwardMovement = new Vector3(BoatCamera.transform.forward.x,
                                              0,
                                              BoatCamera.transform.forward.z).normalized
                                              * movement.y;

        Vector3 sidewaysMovement = new Vector3(BoatCamera.transform.right.x,
                                               0,
                                               BoatCamera.transform.right.z).normalized
                                               * movement.x;

        // "Tank(ish)" controls
        //Vector3 forwardMovement = new Vector3(Rigidbody.transform.forward.x,
        //                                      0,
        //                                      Rigidbody.transform.forward.z).normalized
        //                                      * movement.y;

        //Vector3 sidewaysMovement = new Vector3(Rigidbody.transform.right.x,
        //                                       0,
        //                                       Rigidbody.transform.right.z).normalized
        //                                       * movement.x;

        //Vector3 totalMovementRelativeToCamera = (forwardMovement + sidewaysMovement);

        //raftFreeVelocity += totalMovementRelativeToCamera * 8.4f * Time.deltaTime;

        //raftFreeVelocity = raftFreeVelocity.normalized * Mathf.Clamp(raftFreeVelocity.magnitude, 0, 2.06f);

        //Debug.Log("Forward movement is " + forwardMovement);

        Vector3 totalMovementRelativeToCamera = (forwardMovement * 4.2f+ sidewaysMovement * 8.4f);

        raftFreeVelocity += totalMovementRelativeToCamera * Time.deltaTime;

        raftFreeVelocity = raftFreeVelocity.normalized * Mathf.Clamp(raftFreeVelocity.magnitude, 0, 3.12f);





        float formerYVelo = Rigidbody.velocity.y;

        if (hasReachedStopArea)
        {
            waterCurrentVelocity = Vector3.zero;
            raftFreeVelocity = Vector3.zero;
        }

        //Rigidbody.velocity = waterCurrentVelocity
        //                     + raftFreeVelocity
        //                     + new Vector3(0, formerYVelo, 0);

        ////Rigidbody.transform.forward = Vector3.RotateTowards(Rigidbody.transform.forward.normalized,
        ////                                                    Rigidbody.velocity.normalized, 8.0f * Time.deltaTime,
        ////                                                    1.0f);
        //Vector3 newXZVelo = new Vector3(Rigidbody.velocity.x,
        //                                0,
        //                                Rigidbody.velocity.z);

        //Rigidbody.transform.forward = Vector3.Lerp(Rigidbody.transform.forward.normalized,
        //                                           newXZVelo.normalized,
        //                                           1.9f * Time.deltaTime);

        ////Rigidbody.transform.forward = Vector3.RotateTowards(Rigidbody.transform.forward.normalized,
        ////                                                    raftFreeVelocity, 0.5f * Time.deltaTime,
        ////                                                    1.0f);

        //Rigidbody.angularVelocity = Vector3.zero;

        ////Rigidbody.transform.forward = dir.normalized;
        ////Rigidbody.transform.up = Vector3.up;

        //NearestPointVisualizer.transform.position = nearest.xyz;

        //Debug.Log("Movement " + movement + " nearest is " + nearest.xyz + " water current velocity mag is " + waterCurrentVelocity.magnitude);

        bool foundGround = false;
        float yHeightGround = -1000;

        Physics.RaycastNonAlloc(transform.position + Vector3.up * 10.0f,
                                Vector3.down * 1000.0f,
                                hits);

        for (int i = 0; i< hits.Length; i++)
        {
            if (hits[i].collider != null
                && hits[i].collider.CompareTag("Ground")) 
            {
                foundGround = true;
                
                if (hits[i].point.y >= yHeightGround) 
                {
                    yHeightGround = hits[i].point.y;
                }

                //Debug.Log("Hit is " + hits[i].collider.gameObject.name + " point y is " + hits[i].point.y);
            }
        }

        if (!foundGround)
        {
            yHeightGround = 0;
        }

        bool isGrounded = false;

        if (transform.position.y - yHeightGround
            <= 0.1f) 
        {

            Rigidbody.transform.position = new Vector3(Rigidbody.transform.position.x,
                                                       yHeightGround - 0.5f,
                                                       Rigidbody.transform.position.z);
            Rigidbody.useGravity = false;
            Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            isGrounded = true;
        }

        else
        {

            //Rigidbody.velocity = new Vector3(Rigidbody.velocity.x,
            //                                 yVelocity,
            //                                 Rigidbody.velocity.z);

            if (Rigidbody.transform.position.y <= yHeightGround - 0.5f)
            {
                Rigidbody.transform.position = new Vector3(Rigidbody.transform.position.x,
                                                           yHeightGround - 0.5f,
                                                           Rigidbody.transform.position.z);
                Rigidbody.useGravity = false;
                Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                isGrounded = true;
                //Debug.Log("Snapping to ground " + Time.time);
            }

            else
            {
                isGrounded = false;
                Rigidbody.useGravity = true;
                Rigidbody.constraints = RigidbodyConstraints.None;
                Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                //Debug.Log("We should be letting gravity handle things now " + Time.time);
            }

            //Debug.Log("We should be in free fall. " + Time.time + " velo mag is " + Rigidbody.velocity.magnitude);
        }

        if (!isGrounded)
        {
            raftFreeVelocity = Vector3.zero;
        }

        else
        {
            formerYVelo = 0;
        }

        Rigidbody.velocity = waterCurrentVelocity
                     + raftFreeVelocity
                     + new Vector3(0, formerYVelo, 0);

        //Rigidbody.transform.forward = Vector3.RotateTowards(Rigidbody.transform.forward.normalized,
        //                                                    Rigidbody.velocity.normalized, 8.0f * Time.deltaTime,
        //                                                    1.0f);
        Vector3 newXZVelo = new Vector3(Rigidbody.velocity.x,
                                        0,
                                        Rigidbody.velocity.z);

        Rigidbody.transform.forward = Vector3.Lerp(Rigidbody.transform.forward.normalized,
                                                   newXZVelo.normalized,
                                                   1.9f * Time.deltaTime);

        //Rigidbody.transform.forward = Vector3.RotateTowards(Rigidbody.transform.forward.normalized,
        //                                                    raftFreeVelocity, 0.5f * Time.deltaTime,
        //                                                    1.0f);

        Rigidbody.angularVelocity = Vector3.zero;

        //Rigidbody.transform.forward = dir.normalized;
        //Rigidbody.transform.up = Vector3.up;

        NearestPointVisualizer.transform.position = nearest.xyz;

        if (foundGround) 
        {
            previousGoodYHit = yHeightGround - 0.5f;
        }

        timeOfLastFixedUpdate = Time.time;
        lastRaftRigidbodyPosition = Rigidbody.transform.position;
        predictedRaftRigidbodyPosition = Rigidbody.transform.position + Rigidbody.velocity * Time.fixedDeltaTime;

        float magFromLastPos = (transform.position - lastPos).magnitude;

        totalDistanceTravelled += magFromLastPos;

        //Debug.Log("Total distance travelled " + (int) totalDistanceTravelled);

        lastPos = transform.position;
    }

    public void Update()
    {
        //return;

        //float ratio = (Time.time - timeOfLastFixedUpdate) / Time.fixedDeltaTime;
        //RaftGraphics.transform.position = Vector3.Lerp(lastRaftRigidbodyPosition,
        //                                               predictedRaftRigidbodyPosition,
        //                                               ratio);

        //RaftGraphics.transform.position = new Vector3(RaftGraphics.transform.position.x,
        //                                              RaftGraphics.transform.position.y + graphicsYOffset,
        //                                              RaftGraphics.transform.position.z);

        //Vector3 rigidPos = Rigidbody.transform.position;
        //rigidPos = new Vector3(rigidPos.x, rigidPos.y + graphicsYOffset, rigidPos.z); 

        //RaftGraphics.transform.position = Vector3.Lerp(lastRaftGraphicsPosition,
        //                                               rigidPos,
        //                                               0.8f);

        //Debug.Log("Ratio is " + ratio + " at time " + Time.time);
    }

    public void OnStopAreaReached()
    {
        hasReachedStopArea = true;
    }
}

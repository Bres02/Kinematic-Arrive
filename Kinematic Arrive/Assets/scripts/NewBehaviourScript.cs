using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform formationLeader;

    [SerializeField] private float formationDistanceOffset;
    [SerializeField] private float formationRoatationOffset;

    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    private float moveSpeed;
    private float turnSpeed;
    private float radiusOfSatisfaction;

    private Ray[] rays;
    private bool[] avoid;
    private bool isLeft = false;
    private Quaternion stopRoation;
    enum turning { front, left, right, stop }
    turning myDir = turning.front;
    [SerializeField] private float angle;
    [SerializeField] private int numberOfRays;
    [SerializeField] private float lineLength;
    [SerializeField] private GameObject wall;
    public LayerMask isWallLayer;

    private void Start()
    {
        moveSpeed = 5f;
        turnSpeed = 5f;
        radiusOfSatisfaction = 1.5f;
        rays = new Ray[numberOfRays];
        avoid = new bool[numberOfRays];
    }

    private void Update()
    {

        // Project point forward from leader's forward facing vector
        Vector3 projectedPoint = formationLeader.forward * formationDistanceOffset;

        // Rotate that point to find this character's spot in the formation
        Vector3 positionInFormation = Quaternion.Euler(0f, formationRoatationOffset, 0f) * projectedPoint;

        positionInFormation += formationLeader.position;
        RaycastHit[] hitInfos = new RaycastHit[numberOfRays];

        var rotation = this.transform.rotation;


        if (Vector3.Distance(trans.position, positionInFormation) > radiusOfSatisfaction)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle - (angle / 2), this.transform.up);
                var direction = rotation * rotationMod * Vector3.forward;
                rays[i] = new Ray(this.transform.position, direction);
                avoid[i] = Physics.Raycast(rays[i], out hitInfos[i], lineLength);
                Debug.DrawRay(transform.position, rays[i].direction * lineLength, Color.red);
            }

            // Calculate vector to the position in the formation
            Vector3 towards = positionInFormation - trans.position;

            // Normalize vector to standardize movement speed
            
            Quaternion targetRotation;

            Debug.DrawLine(trans.position, positionInFormation, Color.red);

            if (myDir == turning.front)
            {
                if (avoid[(avoid.Length - 1) / 2] && hitInfos[(avoid.Length - 1) / 2].transform.tag == "wall" || avoid[((avoid.Length - 1) / 2) - 1] && hitInfos[((avoid.Length - 1) / 2) - 1].transform.tag == "wall" || avoid[((avoid.Length - 1) / 2) + 1] && hitInfos[((avoid.Length - 1) / 2) + 1].transform.tag == "wall")
                {
                    if (avoid[1] && hitInfos[1].transform.tag == "wall" || avoid[2] && hitInfos[2].transform.tag == "wall")
                    {
                        myDir = turning.stop;
                        stopRoation = Quaternion.Lerp(trans.rotation, Quaternion.LookRotation(rays[numberOfRays-1].direction), turnSpeed * Time.deltaTime);//
                        isLeft = false;
                    }
                    else if (avoid[avoid.Length - 2] && hitInfos[avoid.Length - 2].transform.tag == "wall" || avoid[avoid.Length - 3] && hitInfos[avoid.Length - 3].transform.tag == "wall")
                    {
                        myDir = turning.stop;
                        stopRoation = Quaternion.Lerp(trans.rotation, Quaternion.LookRotation(rays[0].direction), turnSpeed * Time.deltaTime);//add code
                        isLeft = true;
                    }
                }
            }
            if (myDir == turning.stop)
            {
                if (isLeft)
                {
                    trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.LookRotation(rays[0].direction), turnSpeed * Time.deltaTime);//add code
                    if (Quaternion.Angle(trans.rotation, stopRoation) < 0.02f)
                    {
                        myDir = turning.left;
                    }
                }
                else
                {
                    trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.LookRotation(rays[numberOfRays - 1].direction), turnSpeed * Time.deltaTime);//add code
                    if (Quaternion.Angle(trans.rotation, stopRoation) < 0.02f)
                    {
                        myDir = turning.right;
                    }
                }
            }
            if (myDir == turning.front)
            {
                // Face player along movement vector
                targetRotation = Quaternion.LookRotation(towards);
                trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.velocity = transform.forward.normalized * moveSpeed;

            }
            else if (myDir == turning.left)
            {
                //add code
                targetRotation = Quaternion.LookRotation(rays[0].direction);
                trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.velocity = transform.forward.normalized * moveSpeed;
                if (!avoid[avoid.Length - 1] && !avoid[avoid.Length - 2])
                {
                    myDir = turning.front;
                }
            }
            else if (myDir == turning.right)
            {
                //add code
                targetRotation = Quaternion.LookRotation(rays[numberOfRays -1].direction);
                trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.velocity = transform.forward.normalized * moveSpeed;
                if (!avoid[0] && !avoid[1])
                {
                    myDir = turning.front;
                }
            }

        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    private Ray[] rays;
    private bool[] avoid;
    private bool isLeft = false;
    private Quaternion stopRoation;
    enum turning { front, left, right, stop  }
    turning myDir = turning.front;

    [SerializeField] private float angle;
    [SerializeField] private int numberOfRays;
    [SerializeField] private float lineLength;

    [SerializeField] private GameObject wall;
    public LayerMask isWallLayer;

    [SerializeField] private Transform mover;
    [SerializeField] private Rigidbody rigidBodMover;

    private Vector3 targetLocation;
    
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance;

    // Start is called before the first frame update
    void Start()
    {
        
        
        speed = 5.0f;
        stopDistance = 3.0f;
        targetLocation = mover.position;
        rays = new Ray[numberOfRays];
        avoid = new bool[numberOfRays];
        
    }

    //allows for user input since fixed update is janky
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetLocation = clickocation();
        }
        if(Input.GetMouseButtonDown(1)) 
        {
            Instantiate(wall, new Vector3(clickocation().x, 6.25f, clickocation().z), Quaternion.identity);
        }
    }
    //makes physics run smoother
    void FixedUpdate()
    {
        Vector3 towards = targetLocation - mover.position;

        RaycastHit[] hitInfos = new RaycastHit[numberOfRays];

        var rotation = this.transform.rotation;

        for (int i = 0; i < numberOfRays; i++)
        {
            var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle - (angle / 2), this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            rays[i] = new Ray(this.transform.position, direction);
            avoid[i] = Physics.Raycast(rays[i], out hitInfos[i], lineLength);
            Debug.DrawRay(transform.position, rays[i].direction * lineLength, Color.red);
        }
        if (towards.magnitude <= stopDistance) { return; }
        if (myDir == turning.front)
        {
            if (avoid[(avoid.Length - 1) / 2] && hitInfos[(avoid.Length - 1)/2].transform.tag == "wall" || avoid[((avoid.Length - 1) / 2)-1] && hitInfos[((avoid.Length - 1) / 2) - 1].transform.tag == "wall" || avoid[((avoid.Length - 1) / 2)+1] && hitInfos[((avoid.Length - 1) / 2)+1].transform.tag == "wall")
            {
                if (avoid[1] && hitInfos[1].transform.tag == "wall" || avoid[2] && hitInfos[2].transform.tag == "wall")
                {

                    myDir = turning.stop;
                    stopRoation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(rays[numberOfRays - 1].direction), .02f);
                    isLeft = false;
                }
                else if (avoid[avoid.Length-2] && hitInfos[avoid.Length-2].transform.tag == "wall" || avoid[avoid.Length - 3] && hitInfos[avoid.Length - 3].transform.tag == "wall")  
                {
                    myDir = turning.stop;
                    stopRoation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(rays[0].direction), .02f);
                    isLeft = true;
                }
            }
        }
        
        if (myDir == turning.stop)
        {
            if (isLeft)
            {
                mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(rays[0].direction), .02f);
                if (Quaternion.Angle(mover.rotation, stopRoation) < 0.02f)
                {
                    myDir = turning.left;
                }
            }
            else 
            {
                mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(rays[numberOfRays - 1].direction), .02f);
                if (Quaternion.Angle(mover.rotation, stopRoation) < 0.02f)
                {
                    myDir = turning.right;
                }
            }
        }
        if (myDir == turning.front)
        {
            mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(towards), .02f);
            rigidBodMover.velocity = transform.forward.normalized * speed;

        }
        else if (myDir == turning.left)
        {
            mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(rays[0].direction), .02f);
            rigidBodMover.velocity = transform.forward.normalized * speed;
            if (!avoid[avoid.Length-1] && !avoid[avoid.Length - 2])
            {
                myDir = turning.front;
            }
        }
        else if (myDir == turning.right)
        {
            mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(rays[numberOfRays-1].direction), .02f);
            rigidBodMover.velocity = transform.forward.normalized * speed;
            if (!avoid[0] && !avoid[1] )
            {
                myDir = turning.front;
            }
        }
        

    }

    //the Kinematic part of the assinment
    Vector3 clickocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
             return hit.point;
        }
        return targetLocation;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    private Ray front, left, right;
    [SerializeField] private float lineLength;

    [SerializeField] private GameObject wall;

    [SerializeField] private Transform mover;
    [SerializeField] private Rigidbody rigidBodMover;

    private Vector3 targetLocation;
    
    [SerializeField] private float speed;
    [SerializeField] private float stop;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
        stop = 3.0f;
        targetLocation = mover.position;
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
        
        if (towards.magnitude <= stop) { return; }


        var rotation = this.transform.rotation;
        var rotationModleft = Quaternion.AngleAxis((0 / ((float)2)) * 45 - (45 / 2), this.transform.up);
        var rotationModfront = Quaternion.AngleAxis((1 / ((float)2)) * 45 - (45 / 2), this.transform.up);
        var rotationModright = Quaternion.AngleAxis((2 / ((float)2)) * 45 - (45 / 2), this.transform.up);

        var directionleft = rotation * rotationModleft * Vector3.forward;
        var directionfront = rotation * rotationModfront * Vector3.forward;
        var directionright = rotation * rotationModright * Vector3.forward;

        right = new Ray(this.transform.position, directionright);
        front = new Ray(this.transform.position, directionfront);
        left = new Ray(this.transform.position, directionleft);


        RaycastHit hitInfoLeft;
        RaycastHit hitInfoFront;
        RaycastHit hitInfoRight;

        if (Physics.Raycast(left, out hitInfoLeft, lineLength))
        {
            Debug.DrawRay(transform.position, left.direction * lineLength, Color.red);

        }
        else if (Physics.Raycast(right, out hitInfoRight, lineLength))
        {
            Debug.DrawRay(transform.position, right.direction * lineLength, Color.red);

        }
        else if (Physics.Raycast(front, out hitInfoFront, lineLength))
        {
            
                Debug.DrawRay(transform.position, front.direction * lineLength, Color.red);

        }
        else
        {
            Debug.DrawRay(transform.position, right.direction * lineLength, Color.green);
            Debug.DrawRay(transform.position, left.direction * lineLength, Color.green); 
            Debug.DrawRay(transform.position, front.direction * lineLength, Color.green);

        }
        if( !Physics.Raycast(front, out hitInfoFront,lineLength))
        {
            mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(towards), .1f);
            rigidBodMover.velocity = transform.forward.normalized * speed;
        }
        else 
        {
            if (!Physics.Raycast(left, out hitInfoLeft, lineLength))
            {
                mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(-right.direction), .1f);
            }
            rigidBodMover.velocity = transform.forward.normalized * speed;

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

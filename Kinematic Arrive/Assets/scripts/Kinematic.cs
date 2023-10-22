using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{

    [SerializeField] private Transform mover;
    [SerializeField] private Rigidbody rigidBodMover;

    private Vector3 targetLocation;
    
    [SerializeField] private float speed;
    [SerializeField] private float stop;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
        stop = 1.0f;
        targetLocation = mover.position;
    }

    //allows for user input since fixed update is janky
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetLocation = clickocation();
        }
    }
    
    //makes physics run smoother
    void FixedUpdate()
    {
        Vector3 towards = targetLocation - mover.position;
        
        if (towards.magnitude <= stop) { return; }
        mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(towards), .1f);
        rigidBodMover.velocity = transform.forward.normalized * speed;

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

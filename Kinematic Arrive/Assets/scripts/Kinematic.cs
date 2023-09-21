using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{

    [SerializeField] private Transform mover;
    [SerializeField] private Rigidbody rigidBodMover;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float stop;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
        stop = 3.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 towards = target.position - mover.position;
        
        if (towards.magnitude <= stop) { return; }
        mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(towards), .1f);
        rigidBodMover.velocity = transform.forward.normalized * speed;

    }
}

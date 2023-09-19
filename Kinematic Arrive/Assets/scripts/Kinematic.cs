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
    void Update()
    {
        Vector3 towards = target.position - mover.position;
        mover.rotation = Quaternion.LookRotation(towards);

        if (towards.magnitude > stop)
        {

            towards.Normalize();
            towards *= speed;

            rigidBodMover.velocity = towards;
        }
    }
}

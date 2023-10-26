using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class follower : MonoBehaviour
{
    [SerializeField] private GameObject kinematicTracker;
    private Vector3 targetLocation;
    [SerializeField] private Transform mover;
    [SerializeField] private Rigidbody rigidBodMover;
    private float speed;
    private float stopDistance;
    [SerializeField] private GameObject wall;


    // Start is called before the first frame update
    void Start()
    {
        stopDistance = 1.0f;
        speed = 2.0f;
        targetLocation = new Vector3(kinematicTracker.transform.position.x,kinematicTracker.transform.position.y,kinematicTracker.transform.position.z+10.0f);

        //Instantiate(wall, new Vector3(kinematicTracker.transform.position.x, kinematicTracker.transform.position.y, kinematicTracker.transform.position.z+2.0f), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        targetLocation = new Vector3(kinematicTracker.transform.position.x, this.transform.position.y, kinematicTracker.transform.forward.z+2.0f);
    
    }
    private void FixedUpdate()
    {
        
        Vector3 towards = targetLocation - mover.position;
        if (towards.magnitude <= stopDistance) { return; }
        mover.rotation = Quaternion.Lerp(mover.rotation, Quaternion.LookRotation(towards), .02f);
        rigidBodMover.velocity = transform.forward.normalized * speed;
    }
}

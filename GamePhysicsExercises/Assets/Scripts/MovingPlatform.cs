using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3[] waypoints;
    private int waypointIndex;
    public float moveSpeed;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        rb = this.GetComponent<Rigidbody>();
        moveSpeed *= .1f;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 waypoint = Vector3.MoveTowards(transform.position, waypoints[waypointIndex], moveSpeed);
        rb.MovePosition(waypoint);
        if (this.transform.position == waypoints[waypointIndex])
        {
            if (waypointIndex == waypoints.Length-1) waypointIndex = 0;
            else waypointIndex++;
        }
    }
}

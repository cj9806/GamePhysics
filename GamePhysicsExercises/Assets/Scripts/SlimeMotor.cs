using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*need to hop towards particular location
* controll the jump power vericly
* control jump power horizontily
* jump interval
* should be implemented using Rigidbody.AddForce
*/
public class SlimeMotor : MonoBehaviour
{
    public float verticleJumpPower;
    public float horizontalJumpPower;
    public float jumpInterval;
    public Vector3 travelPoint;
    private Rigidbody rb;
    private System.Random random;
    private bool closeToGoal;
    float elapsed;
    RaycastHit ray;
    RaycastHit targetRay;
    public bool attracted;
    [SerializeField] LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        closeToGoal = false;
        elapsed = 0f;
        random = new System.Random();
        attracted = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //basic timer
        elapsed += Time.deltaTime;
        if (elapsed >= jumpInterval)
        {
            //reset timer
            elapsed = 0f;
            //if not close to goal
            if (!attracted)
            {
                //jump
                Jump();

                //see if close to goal
                if (targetRay.distance <= 2)
                {
                    travelPoint = createNewWaypoint();
                }
            }
            else 
            {
                Jump();
                if (targetRay.distance <= 2)
                {
                    horizontalJumpPower = 0;
                }
            }
        }
        //raycast to look at goal
        Vector3 goal = (travelPoint - transform.position).normalized;
        Physics.Raycast(transform.position, goal, out targetRay, mask);
        Debug.Log(targetRay.collider.gameObject.name);

    }
    void Jump()
    {
        Physics.Raycast(transform.position, Vector3.down, out ray);
        //jump up
        rb.AddForce(0, verticleJumpPower, 0, ForceMode.Impulse);
        //jump Forward
        rb.AddForce((travelPoint - transform.position).normalized * horizontalJumpPower, ForceMode.Impulse);
    }
    Vector3 createNewWaypoint()
    {
        int randX = random.Next(-23, 23);
        int randZ = random.Next(-23, 23);
        return new Vector3(randX, 0, randZ);
    }
}

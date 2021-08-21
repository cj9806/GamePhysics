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
    
    RaycastHit ray;
    RaycastHit targetRay;
    public bool attracted;
    [SerializeField] LayerMask mask;
    //timer varibles
    float elapsed;
    private float growTimer;

    private float jumpReset;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        closeToGoal = false;
        elapsed = 0f;
        growTimer = 0f;
        random = new System.Random();
        attracted = false;
        jumpReset = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        //basic timer
        elapsed += Time.deltaTime;
        if (attracted)
        {
            //wait for jump interval
            if (elapsed >= jumpInterval)
            {
                //jump
                Jump();
                //reset timer
                elapsed = 0;
            }

            //if close to goal
            if (targetRay.distance <= 2)
            {
                growTimer += Time.deltaTime;
                horizontalJumpPower = 0;
            }
        }
        //not attracted
        else
        {
            //wait for jump interval
            if (elapsed >= jumpInterval)
            {
                //jump
                Jump();
                //reset timer
                elapsed = 0;
            }
            //see if close to goal
            if (targetRay.distance <= 2)
            {
                travelPoint = CreateNewWaypoint();
            }
            if (growTimer >= 0) growTimer -= Time.deltaTime;
        }
        //if grow timer has reached 10
        if (growTimer >= 10)
        {
            Grow();
            attracted = false;
            horizontalJumpPower = jumpReset;
        }
        if (growTimer <= 0)
        {
            Shrink();
        }
        //raycast to look at goal
        Vector3 goal = (travelPoint - transform.position).normalized;
        Physics.Raycast(transform.position, goal, out targetRay, mask);
        

    }
    void Jump()
    {
        Physics.Raycast(transform.position, Vector3.down, out ray);
        //jump up
        rb.AddForce(0, verticleJumpPower, 0, ForceMode.Impulse);
        //jump Forward
        rb.AddForce((travelPoint - transform.position).normalized * horizontalJumpPower, ForceMode.Impulse);
    }
    Vector3 CreateNewWaypoint()
    {
        int randX = random.Next(-23, 23);
        int randZ = random.Next(-23, 23);
        return new Vector3(randX, 0, randZ);
    }
    void Grow()
    {
        transform.localScale = new Vector3(2, 2, 2);
    }
    void Shrink()
    {
        transform.localScale = Vector3.one;
    }
}

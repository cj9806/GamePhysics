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
    //goal is 4,.75,0
    // Start is called before the first frame update
    private Rigidbody rb;
    private System.Random random;
    private bool closeToGoal;
    float elapsed;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        closeToGoal = false;
        elapsed = 0f;
        verticleJumpPower *= 100;
        //horizontalJumpPower *= 100;
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        //basic timer
        elapsed += Time.deltaTime;
        //after given time
        if (elapsed >= jumpInterval)
        {
            //reset timer
            elapsed = 0f;
            //if not close to goal
            if (!closeToGoal)
            {
                //jump up
                Jump();
                //move forward

                //see if close to goal
                if (travelPoint.x - transform.position.x <= 1 && travelPoint.z - transform.position.z <= 1)
                {
                    closeToGoal = true;
                }
            }
            else
            {
                travelPoint = createNewWaypoint();
                closeToGoal = false;
            }
        }

    }
    void Jump()
    {
        if (this.transform.position.y == .75)
        {
            rb.AddForce(0, verticleJumpPower, 0);
            rb.AddForce((travelPoint - transform.position).normalized * horizontalJumpPower * Time.deltaTime);
            //jump forward
        }
    }
    Vector3 createNewWaypoint()
    {
        int randX = random.Next(-23, 23);
        int randZ = random.Next(-23, 23);
        return new Vector3(randX, .75f, randZ);
    }
}

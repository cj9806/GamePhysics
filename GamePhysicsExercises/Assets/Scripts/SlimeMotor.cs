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
    private bool closeToGoal;
    float elapsed;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        closeToGoal = false;
        elapsed = 0f;
        verticleJumpPower *= 100;
        horizontalJumpPower *= 100;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= jumpInterval)
        {
            elapsed = 0f;
            if (!closeToGoal)
            {
                Jump();
                if (this.transform.position.x >= 3)
                    closeToGoal = true;
            }
        }

    }
    void Jump()
    {
        if (this.transform.position.y == .75)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(horizontalJumpPower, verticleJumpPower, 0);
        }
    }
}

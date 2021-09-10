using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMotor : MonoBehaviour
{
    public float verticleJumpPower;
    public float horizontalJumpPower;
    public float jumpInterval;
    public Vector3 travelPoint;
    private Rigidbody rb;
    private System.Random random;
    
    public bool attracted;
    public float attractionRange;
    [SerializeField] LayerMask mask;
    //timer varibles
    float elapsed;
    public float growTimer;

    Vector3 goal;
    private float distance;

    private float jumpReset;
    private Vector3 baseScale;

    [HideInInspector]
    public KinematicPlayerController player;

    public Material[] materialsList;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        elapsed = 0f;
        growTimer = 0f;
        random = new System.Random();
        attracted = false;
        jumpReset = horizontalJumpPower;
        baseScale = GetComponentInParent<Transform>().localScale;
        ColorMeSlimey();
    }

    // Update is called once per frame
    void Update()
    {
        //basic timer
        elapsed += Time.deltaTime;
        if (elapsed >= jumpInterval)
        {
            //jump
            Jump();
            //reset timer
            elapsed = 0;
        }
        distance = Vector3.Distance(transform.position, travelPoint);

        if (attracted)
        {
            
            
            //if close to goal
            if (distance <= attractionRange)
            {
                growTimer += Time.deltaTime;
                horizontalJumpPower = 0;
            }
        }
        //not attracted
        else
        { 
            //see if close to goal
            if (distance <= attractionRange)
            {
                travelPoint = CreateNewWaypoint();
            }
            if (growTimer >= 0) growTimer -= Time.deltaTime;
        }
        //if grow timer has reached 10
        if (growTimer >= 10)
        {
            //Grow();
            attracted = false;
            horizontalJumpPower = jumpReset;
        }
        if (growTimer <= 0)
        {
            //Shrink();
        }
        //delete slime if it gets trown off the side
        if (transform.parent.transform.position.y <= -10f)
        {
            Destroy(transform.parent.gameObject);
            player.spawnedSlimes--;
        }
    }
    void Jump()
    {
        //jump up
        rb.AddForce(0, verticleJumpPower, 0, ForceMode.Impulse);
        //jump Forward
        rb.AddForce((travelPoint - transform.position).normalized * horizontalJumpPower, ForceMode.Impulse);
    }
    Vector3 CreateNewWaypoint()
    {
        int randX = random.Next(-74, 74);
        int randZ = random.Next(-74, 74);
        return new Vector3(randX, 0, randZ);
    }
    void Grow()
    {
        transform.parent.transform.localScale = baseScale * 2;
    }
    void Shrink()
    {
        transform.parent.transform.localScale = baseScale;
    }
    void ColorMeSlimey()
    {
        MeshRenderer[] mr = transform.parent.transform.GetComponentsInChildren<MeshRenderer>();
        Material choice = materialsList[random.Next(0, materialsList.Length)];
        foreach(MeshRenderer mesh in mr)
        {
            mesh.material = choice;
        }
    }
}

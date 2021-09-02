using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KinematicPlayerController : MonoBehaviour
{
    [SerializeField] Camera camera;
    public float moveSpeed;
    float tempX;
    float tempZ;
    Vector2 tempVectTwo;

    public bool useGravity;
    public float gravityStrength;

    public float rotationSpeed;
    float lookX;
    float lookY;
    //[SerializeField] 

    public float skinWidth;
    Collider col;
    
    Vector3 expp;

    public PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponentInChildren<Collider>();
        col.transform.localScale = transform.localScale + (Vector3.one * skinWidth);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(lookX + " " + lookY);
        expp = transform.position;
        //movement handling events
        {
            Vector2 LookInput = input.currentActionMap["Look"].ReadValue<Vector2>();
            lookX = LookInput.x;
            lookY = LookInput.y;

            Vector2 moveInput = input.currentActionMap["Move"].ReadValue<Vector2>();
            tempX = moveInput.x;
            tempZ = moveInput.y;
            if (tempX != 0)
            {
                expp += transform.right * tempX * moveSpeed;
            }
            if (tempZ != 0)
            {
                expp += transform.forward * tempZ * moveSpeed;
            }
            
            
            //jump handling
            if (useGravity)
            {
                expp += new Vector3(0,gravityStrength,0);
            }
           
            if(lookX != 0 || lookY != 0)
            {
                transform.Rotate(new Vector3(0,lookX,0) * rotationSpeed);
                camera.transform.Rotate(new Vector3(-lookY, 0, 0) * rotationSpeed);
            }
            //end movement handling
        }
        //colision handling
        
        Collider[] overlaps = Physics.OverlapBox(transform.position,transform.localScale/2);
        foreach (var overlap in overlaps)
        {
            if (overlap == col) continue;
            Physics.ComputePenetration(
                //first collider info
                col,
                expp,
                col.transform.rotation,
                //second collider info
                overlap,
                overlap.transform.position,
                overlap.transform.rotation,
                //outputs
                out Vector3 cldrDir,
                out float cldrDist
                );
            //draw a ray for testing purposes
            Debug.DrawRay(this.transform.position, cldrDir * cldrDist, Color.red);
            if (overlap.name == "Floor")
                expp += cldrDir * -gravityStrength;
            else expp += cldrDir * moveSpeed;

        }
        transform.position = expp;
    }
}

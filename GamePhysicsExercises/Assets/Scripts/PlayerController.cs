using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    float tempX;
    float tempZ;
    Vector2 tempVectTwo;

    float tempY;
    bool jumping;
    public bool useGravity;
    public float gravityStrength;

    public float rotationSpeed;
    float tempRot;
    [SerializeField] Camera mainCamera;
    //[SerializeField] 

    public float skinWidth;
    Collider col;

    Vector3 expp;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponentInChildren<Collider>();
        col.transform.localScale = transform.localScale + (Vector3.one * skinWidth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        expp = transform.position;
        //movement handling events
        {

            tempVectTwo.Normalize();

            Debug.Log(tempVectTwo);
            expp += new Vector3(tempVectTwo.x, 0, tempVectTwo.y)* moveSpeed;
            
            //jump handling
            if (useGravity)
            {
                expp += new Vector3(0,gravityStrength,0);
            }
            if (tempRot != 0)
            {
                transform.Rotate(new Vector3(0, tempRot, 0) * rotationSpeed);
            }
            //end movement handling
        }
        //colision handling
        Vector3 pointZero = expp - new Vector3(0, -.5f, 0);
        Vector3 pointOne = expp - new Vector3(0, .5f, 0);

        Collider[] overlaps = Physics.OverlapCapsule(pointZero, pointOne, .5f);
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
            else expp = cldrDir * moveSpeed;

        }
        transform.position = expp;
    }

    void OnMove(InputValue value)
    {
        tempVectTwo = value.Get<Vector2>();
        //tempX = .x;
        //tempZ = value.y;
        //not sure if this line is necessary
        //transform.position += new Vector3(tempX, 0, tempZ) * moveSpeed; 
    }
    void OnJump(InputValue value)
    {
        jumping = value.isPressed;
    }
    void OnLook(InputValue value)
    {
        tempRot = value.Get<Vector2>().x;
    }
}

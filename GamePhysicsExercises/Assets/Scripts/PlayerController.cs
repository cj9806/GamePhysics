using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    float tempX;
    float tempZ;

    float tempY;
    bool jumping;

    float tempRot;
    [SerializeField] Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //movement handling events
        {
            if (tempX != 0)
            {
                transform.position += transform.right * tempX * moveSpeed;
            }
            if (tempZ != 0)
            {
                transform.position += transform.forward * tempZ * moveSpeed;
            }
            //jump handling
            if (jumping)
            {
                Debug.Log("Jumping");
            }
            if (tempRot != 0)
            {
                transform.Rotate(new Vector3(0, tempRot, 0) * rotationSpeed);
            }
            //end movement handling
        }
        //colision handling
        Collider[] overlapColliders = Physics.OverlapBox(transform.position, transform.localScale/2, Quaternion.identity);
        if (overlapColliders.Length > 1)
        {
            Collider cldrOne = overlapColliders[0];
            Collider cldrtwo = overlapColliders[1];
            Physics.ComputePenetration(
                //first collider info
                cldrOne,
                cldrOne.transform.position,
                cldrOne.transform.rotation,
                //second collider info
                cldrtwo,
                cldrtwo.transform.position,
                cldrtwo.transform.rotation,
                //outputs
                out Vector3 cldrDir,
                out float cldrDist
                );
            //draw a ray for testing purposes
            Debug.DrawRay(this.transform.position, cldrDir, Color.red);
        }
        
    }

    void OnMove(InputValue value)
    {
        tempX = value.Get<Vector2>().x;
        tempZ = value.Get<Vector2>().y;
        Debug.Log(value);


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

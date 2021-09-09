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
    private Vector3 skin;
    BoxCollider col;
    
    Vector3 expp;

    public PlayerInput input;
    bool pickingUp;

    public float playerReach;
    FixedJoint joint;
    [SerializeField] LayerMask grabableLayers;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponentInChildren<BoxCollider>();
        skin = col.size + (Vector3.one * skinWidth);
        //col
        Cursor.lockState = CursorLockMode.Locked;
        joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        expp = transform.position;
        //movement handling events
        {
            //looking
            Vector2 LookInput = input.currentActionMap["Look"].ReadValue<Vector2>();
            lookX = LookInput.x;
            lookY = LookInput.y;
            //moving
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
            //pick up animations
            //if right click and there is no held object pick up
            //elseif right click and there is a held object do nothing
            //elseif no right click
            if (pickingUp && this.gameObject.GetComponent <FixedJoint>()== null) 
            {
                PickUp();
            }
            else if(!pickingUp && this.gameObject.GetComponent<FixedJoint>() != null)
            {
                Drop();
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
        
        Collider[] overlaps = Physics.OverlapBox(expp, skin, col.transform.rotation);
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
                expp += cldrDir * cldrDist;
            else expp += cldrDir * moveSpeed;

        }
        transform.position = expp;
    }
    void OnPickUp(InputValue value)
    {
        pickingUp = value.isPressed;
    }

    void PickUp()
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit hit, playerReach, grabableLayers);
        GameObject slimeColider = hit.collider.gameObject;
        joint.connectedBody = slimeColider.GetComponent<Rigidbody>();
    }
    void Drop()
    {
        Destroy(this.GetComponent<FixedJoint>());
    }
}

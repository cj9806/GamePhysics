using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KinematicPlayerController : MonoBehaviour
{
    public PlayerInput input;
    Vector3 expp;
    bool pickingUp;
    bool firing;
    FixedJoint joint;

    [Header("Movement Controlls")]
    [SerializeField] GameObject camera;
    public float moveSpeed;
    float tempX;
    float tempZ;
    public float rotationSpeed;
    public float lookMax;
    private float pitchControl = 0f;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityStrength;

    [Header("Collison Detection")]
    public float skinWidth;
    private Vector3 skin;
    BoxCollider col;
    
    [Header("Player Input")]
    public float playerReach;
    [SerializeField] LayerMask grabableLayers;
    public float explosionForce;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponentInChildren<BoxCollider>();
        skin = col.size + (Vector3.one * skinWidth);
        //col
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    void Update()
    {
        //looking
        Vector2 lookInput = input.currentActionMap["Look"].ReadValue<Vector2>();
        transform.Rotate(Vector3.up, lookInput.x*rotationSpeed);
        pitchControl = Mathf.Clamp(pitchControl - lookInput.y * rotationSpeed, -lookMax, lookMax);
        camera.transform.localRotation = Quaternion.Euler(pitchControl, 0, 0);
        


        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        expp = transform.position;
        //movement handling events
        {
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
            //jump handling
            if (useGravity)
            {
                expp += new Vector3(0, gravityStrength, 0);
            }
        }
        //colision handling
        
        Collider[] overlaps = Physics.OverlapBox(expp, skin, col.transform.rotation);
        foreach (var overlap in overlaps)
        {
            if (overlap == col) continue;
            if (overlap.isTrigger) continue;
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
            else expp += cldrDir * cldrDist;

        }
        transform.position = expp;
    }
    void OnPickUp(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("pickup");
            PickUp();
        }
        else if(!value.isPressed && joint != null)
            Drop();
    }
    void OnFire(InputValue value)
    {
        if (value.isPressed && joint != null)
        {
            Rigidbody ammo = joint.connectedBody;
            Drop();
            
            ammo.AddForce(camera.transform.forward*explosionForce);
        }
    }
    void PickUp()
    {
        var ray = camera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit hit, playerReach, grabableLayers);
        Debug.DrawRay(camera.transform.position, ray.direction, Color.red);
        if (hit.collider != null)
        {

            joint = camera.AddComponent<FixedJoint>();
            GameObject slimeColider = hit.collider.gameObject;
            joint.connectedBody = slimeColider.GetComponent<Rigidbody>();
        }
    }
    void Drop()
    {
        joint.connectedBody.WakeUp();
        Destroy(joint);
    }
}

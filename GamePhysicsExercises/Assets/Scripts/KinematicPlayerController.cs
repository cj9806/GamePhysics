using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KinematicPlayerController : MonoBehaviour
{
    public PlayerInput input;
    Vector3 expp;
    FixedJoint joint;
    [HideInInspector]
    public int spawnedSlimes = 0;

    [Header("Movement Controlls")]
    [SerializeField] GameObject camera;
    public float moveSpeed;
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
    [SerializeField] GameObject spawnableSlime;
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
        //moving
        Vector2 moveInput = input.currentActionMap["Move"].ReadValue<Vector2>();
        expp += transform.right * moveInput.x * moveSpeed;
        expp += transform.forward * moveInput.y * moveSpeed;

        //Verticle movement
        if (useGravity)
        {
            expp += new Vector3(0, gravityStrength, 0);
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
            PickUp();
        }
        else if(!value.isPressed && joint != null)
            Drop();
    }
    void OnFire(InputValue value)
    {
        if (value.isPressed && joint != null)
        {
            Rigidbody ammo = null;
            if (joint.connectedBody.gameObject.layer == 8)
            {
                ammo = joint.connectedBody.transform.parent.Find("slime brain").GetComponent<Rigidbody>();
            }
            else ammo = joint.connectedBody;
            Drop();
            
            ammo.AddForce(camera.transform.forward*explosionForce);
        }
    }
    void OnSpawn(InputValue value)
    {
        if (spawnedSlimes < 100)
        {
            Vector3 pos = transform.position + (transform.forward * 5);
            pos.y += 5;
            GameObject newSlime = GameObject.Instantiate(spawnableSlime,pos,Quaternion.identity);
            newSlime = newSlime.transform.GetChild(0).gameObject;
            newSlime.GetComponent<SlimeMotor>().player = this;
            spawnedSlimes++;
        }
    }
    void OnQuit(InputValue value)
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
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

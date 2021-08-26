using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    float tempX;
    float tempY;
    bool lmd;
    bool rmd;

    Camera cam;
    Vector2 mosPos;
    float mosx;
    float mosy;
    float mosz;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (tempX != 0 || tempY != 0)
        {
            transform.position += new Vector3(tempX, 0, tempY) * moveSpeed;
        }
        if (lmd)
        {
            mosx = Mouse.current.position.x.ReadValue();
            mosy = Mouse.current.position.y.ReadValue();

            RaycastHit hit;
            mosPos = new Vector2(mosx, mosy);
            Ray screenRay = cam.ScreenPointToRay(mosPos);
            if(Physics.Raycast(screenRay, out hit))
            {
                if (hit.rigidbody != null && hit.rigidbody.name == "Slime")
                {
                    Debug.Log(hit.rigidbody.name);
                }
            }
        }
        if (rmd)
        {
            Debug.Log("rmd");
        }
    }
    void OnMove(InputValue value)
    {
        tempX = value.Get<Vector2>().x;
        tempY = value.Get<Vector2>().y;
        
        transform.position += new Vector3(tempX, 0, tempY) * moveSpeed;
    }
    void OnFire(InputValue value)
    {
        lmd = value.isPressed;
    }
    void OnAltFire(InputValue value)
    {
        rmd = value.isPressed;
    }
}

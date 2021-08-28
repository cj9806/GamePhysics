using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpringVisualize : MonoBehaviour
{
    public SpringJoint[] springs; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(SpringJoint spring in springs)
        {
            Gizmos.DrawLine(transform.position, spring.connectedBody.position);
        }
        
    }
}

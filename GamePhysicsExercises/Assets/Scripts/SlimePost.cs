using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePost : MonoBehaviour
{
    public float attractionRadius;
    SphereCollider radiusCollider;
    // Start is called before the first frame update
    void Start()
    {
        radiusCollider = this.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        radiusCollider.radius = attractionRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        SlimeMotor sm = other.GetComponentInParent<SlimeMotor>();
        if (sm != null)
        {
            if(sm.growTimer <= 0)
            sm.travelPoint = transform.position;
            sm.attracted = true;
        }
        
    }
}

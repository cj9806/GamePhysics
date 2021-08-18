using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameratThrottler : MonoBehaviour
{
    public int targetFramerate = 200;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFramerate;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = targetFramerate;
    }
}

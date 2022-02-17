using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bob : MonoBehaviour
{
    Vector3 startPos;
    public float amplitude = 10f;
    public float period = 1f;
    private Transform body;

    protected void Start()
    {
        body = GameObject.FindWithTag("MainCamera").transform;
        startPos = body.localPosition;
        
    }

    public void update_bob()
    {
        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        body.localPosition = startPos + Vector3.up * distance;
    }
}

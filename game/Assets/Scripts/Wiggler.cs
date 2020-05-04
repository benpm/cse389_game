using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggler : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        Vector3 scale = transform.localScale;
        scale.x = 1.0f + Mathf.Sin(t + transform.position.x) * 0.1f;
        scale.y = 1.0f + Mathf.Sin(t *0.9f + 10.0f + transform.position.x) * 0.15f;
        transform.localScale = scale;
    }
}

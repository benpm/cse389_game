using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveScale : MonoBehaviour
{
    // Returns the amount by which to scale in y direction
    public static float yscaleAmnt()
    {
        return 1.0f / Mathf.Cos(Mathf.Deg2Rad * (360.0f - Camera.main.transform.rotation.eulerAngles.x));
    }

    // Adjust the yscale of this object so it looks correct with a rotated camera
    void adjust()
    {
        float yscale = yscaleAmnt();
        Vector3 scale = transform.localScale;
        scale.y = yscale * (scale.x + scale.z) * 0.5f;
        transform.localScale = scale;
    }

    // Start is called before the first frame update
    void Awake()
    {
        adjust();
    }

    // Update is called once per frame
    void Update()
    {
        adjust();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TrainCar : MonoBehaviour
{
    private PathCreator path;

    public float speed = 1.0f;
    public float position = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        path = FindObjectOfType<PathCreator>();
        Debug.Assert(path, "Train Path object not found");
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        Vector3 point = path.path.GetPointAtDistance(t * speed - position);
        transform.position = point;
        Vector3 rot = path.path.GetRotationAtDistance(t * speed - position).eulerAngles;
        rot.z = -rot.x;
        rot.x = rot.y = 0;
        transform.rotation = Quaternion.Euler(rot);
    }
}

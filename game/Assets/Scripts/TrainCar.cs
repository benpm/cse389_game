using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCar : MonoBehaviour
{
    private PathCreation.PathCreator path;

    public float speed = 1.0f;
    public float position = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        path = FindObjectOfType<PathCreation.PathCreator>();
        Debug.Assert(path, "Train Path object not found");
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        Vector3 point = path.path.GetPointAtDistance(t * speed - position);
        transform.position = point;
        Vector3 rotation = path.path.GetDirectionAtDistance(t * speed - position);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
}

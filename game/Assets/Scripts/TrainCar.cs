using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCar : MonoBehaviour
{
    private Path path;

    public float speed = 1.0f;
    public float position = 0.0f;
    public int hp = 1000;
    public bool abandoned = false;

    // Start is called before the first frame update
    void Start()
    {
        path = GameObject.Find("TrainPath").GetComponent<Path>();
        Debug.Assert(path, "Train Path object not found");
    }

    // Update is called once per frame
    void Update()
    {
        if (!abandoned)
        {
            float t = Time.timeSinceLevelLoad;

            Vector3 point = path.distanceToPosition(t * speed - position);
            point.z = transform.position.z;
            transform.position = point;

            float rot = path.distanceToAngle(t * speed - position);

            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        hp -= 100;
        if (hp <= 0)
        {
            Destroy(gameObject);
            SendMessageUpwards("trainCarDestroyed", this);
        }
    }
}

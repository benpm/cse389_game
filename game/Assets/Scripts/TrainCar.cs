using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainCar : MonoBehaviour
{
    private Path path;

    public float speed = 1.0f;
    public float position = 0.0f;
    public bool abandoned = false;
    public float distAlongTrack { get; private set; }

    // Add delegates for scene loading
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Initialize for start and scene loaded
    private void Init()
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
            distAlongTrack = t * speed - position;
            Vector3 point = path.distanceToPosition(distAlongTrack);
            point.z = transform.position.z;
            transform.position = point;

            float rot = path.distanceToAngle(t * speed - position);

            transform.rotation = Quaternion.Euler(0, 0, rot);

            if (distAlongTrack > path.length)
            {
                GameController.self.EndLevel();
            }
        }
    }

    void hit()
    {
        Debug.LogFormat("{0} hit!", name);
    }
}

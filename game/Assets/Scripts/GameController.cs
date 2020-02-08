using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController self;

    private Camera cam;

    //Enforce singleton behavior
    void Awake()
    {
        if (!self)
        {
            self = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameController not destroyed");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("GameController destroyed");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        Debug.Assert(cam, "Cannot find Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        //Follow the train

    }

    //Make camera follow
    public void CameraFollow(Vector3 pos)
    {
        cam.transform.position = new Vector3(
            pos.x,
            pos.y + cam.transform.position.z / 2,
            cam.transform.position.z
            );
    }
}

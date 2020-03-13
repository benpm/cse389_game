using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController self;

    private Camera cam;
    public BulletSystem bulletSystem;

    //Enforce singleton behavior
    void Awake()
    {
        if (!self)
        {
            self = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletSystem = transform.Find("BulletSystem").GetComponent<BulletSystem>();
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        Debug.Assert(cam, "Cannot find Main Camera");
        Debug.Assert(bulletSystem, "Cannot find BulletSystem");
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
            pos.y + (cam.transform.position.z * 0.5f),
            cam.transform.position.z
            );
    }
}

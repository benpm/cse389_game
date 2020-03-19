using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController self;

    private Camera cam;
    public BulletSystem playerBulletSystem;
    public BulletSystem enemyBulletSystem;
    public Train train;

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
        playerBulletSystem = transform.Find("PlayerBulletSystem").GetComponent<BulletSystem>();
        enemyBulletSystem = transform.Find("EnemyBulletSystem").GetComponent<BulletSystem>();
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        train = FindObjectOfType<Train>();
        Debug.Assert(cam, "Cannot find Main Camera");
        Debug.Assert(playerBulletSystem, "Cannot find PlayerBulletSystem");
        Debug.Assert(enemyBulletSystem, "Cannot find EnemyBulletSystem");
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Make camera follow
    public void CameraFollow(Vector3 pos)
    {
        cam.transform.position = new Vector3(
            pos.x,
            // Adjust y position to account for depth
            pos.y + (cam.transform.position.z * ((cam.transform.rotation.eulerAngles.x - 360.0f) / -45.0f)),
            cam.transform.position.z
            );
    }
}

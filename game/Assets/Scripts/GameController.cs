using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController self;

    private Camera cam;
    private AudioSource audioSource;
    private int _money = 0;
    private Dictionary<string, AudioClip> audioClips;
    public BulletSystem playerBulletSystem;
    public BulletSystem enemyBulletSystem;
    public Train train;
    public LevelProperties levelProperties;
    public UI_Controller ui;
    public int money {
        get { return _money; }
        set
        {
            _money = value;
            ui.setMoney(_money);
        }
    }

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
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
        playerBulletSystem = transform.Find("PlayerBulletSystem").GetComponent<BulletSystem>();
        enemyBulletSystem = transform.Find("EnemyBulletSystem").GetComponent<BulletSystem>();
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        train = FindObjectOfType<Train>();
        levelProperties = FindObjectOfType<LevelProperties>();
        ui = GetComponentInChildren<UI_Controller>();
        // Load audio clip resources
        audioClips = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sound");
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
            Debug.LogFormat("loaded audio resource {0}", clip.name);
        }
        // Make sure we found everything we need
        Debug.Assert(cam, "Cannot find Main Camera");
        Debug.Assert(playerBulletSystem, "Cannot find PlayerBulletSystem");
        Debug.Assert(enemyBulletSystem, "Cannot find EnemyBulletSystem");
        Debug.Assert(levelProperties, "Cannot find LevelProperties");
        Debug.Assert(ui, "Cannot find UI_Controller");
        Debug.Assert(audioSource);
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = ((1280.0f + 720.0f) / ((float)Screen.width + (float)Screen.height)) * 15.65f;
    }

    // Reached end of level
    public void EndLevel()
    {
        Debug.Log("Ended level, going to next level");
        // Clean up abandoned train cars
        train.deleteAbandoned();
        // Load new level
        SceneManager.LoadScene(levelProperties.nextLevel.scenePath);
    }

    // Make camera follow
    public void CameraFollow(Vector3 pos)
    {
        Vector3 camPos = new Vector3(
            pos.x,
            // Adjust y position to account for depth
            pos.y + (cam.transform.position.z * ((cam.transform.rotation.eulerAngles.x - 360.0f) / -45.0f)),
            cam.transform.position.z
        );
        camPos.x = Mathf.Clamp(camPos.x, levelProperties.viewBox.xMin, levelProperties.viewBox.xMax);
        camPos.y = Mathf.Clamp(camPos.y, levelProperties.viewBox.yMin, levelProperties.viewBox.yMax);
        cam.transform.position = camPos;
    }

    // Play a sound
    public void PlaySound(string name)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioClips[name]);
    }
}

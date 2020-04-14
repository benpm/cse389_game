﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController self;

    private Camera cam;
    public BulletSystem playerBulletSystem;
    public BulletSystem enemyBulletSystem;
    public Train train;
    public LevelProperties levelProperties;
    public UI_Controller ui;

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
        playerBulletSystem = transform.Find("PlayerBulletSystem").GetComponent<BulletSystem>();
        enemyBulletSystem = transform.Find("EnemyBulletSystem").GetComponent<BulletSystem>();
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        train = FindObjectOfType<Train>();
        levelProperties = FindObjectOfType<LevelProperties>();
        ui = GetComponentInChildren<UI_Controller>();
        // Make sure we found everything we need
        Debug.Assert(cam, "Cannot find Main Camera");
        Debug.Assert(playerBulletSystem, "Cannot find PlayerBulletSystem");
        Debug.Assert(enemyBulletSystem, "Cannot find EnemyBulletSystem");
        Debug.Assert(levelProperties, "Cannot find LevelProperties");
        Debug.Assert(ui, "Cannot find UI_Controller");
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

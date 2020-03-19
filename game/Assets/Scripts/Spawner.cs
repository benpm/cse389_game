using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Sprite> sprites;
    public int spawnRate = 120;

    private GameObject enemyPrefab;
    private int spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        spawnTimer = spawnRate;
        Debug.Assert(sprites.Count > 0);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= 1;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnRate;
            GameObject newEnemy = Instantiate(enemyPrefab);
            newEnemy.transform.position = transform.position;
            newEnemy.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }

    // Recovered from attack
    void recovered()
    {
        spawnTimer = spawnRate;
    }
}

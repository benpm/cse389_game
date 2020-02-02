using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    List<TrainCar> cars;

    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        cars = new List<TrainCar>(transform.GetComponentsInChildren<TrainCar>());
        float position = 0.0f;
        foreach (TrainCar car in cars)
        {
            car.position = position;
            car.speed = speed;
            position += car.transform.localScale.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

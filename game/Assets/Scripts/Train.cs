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
            position += 2.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameController.self.CameraFollow(cars[0].transform.position);
    }

    // Returns the closest train car
    public TrainCar getClosestCar(Vector2 pos)
    {
        float mindist = Vector2.Distance(pos, cars[0].transform.position);
        TrainCar closest = cars[0];
        foreach (TrainCar car in cars)
        {
            if (Vector2.Distance(pos, car.transform.position) < mindist)
            {
                mindist = Vector2.Distance(pos, car.transform.position);
                closest = car;
            }
        }
        return closest;
    }

    // Distance to closest train car
    public float distance(Vector2 pos)
    {
        return Vector2.Distance(pos, getClosestCar(pos).transform.position);
    }
}

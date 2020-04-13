using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    List<TrainCar> cars;

    public float speed = 1.0f;
    public TrainCar engineCar { get { return cars[0]; } }

    // Re-create train car list
    void createCarList()
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

    // Start is called before the first frame update
    void Start()
    {
        createCarList();
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

    // A train car was destroyed
    public void trainCarDestroyed(TrainCar car)
    {
        // Stop trailing cars
        List<TrainCar> trailingCars = cars.FindAll(item => item.position > car.position);
        foreach (TrainCar trailingCar in trailingCars)
        {
            trailingCar.abandoned = true;
        }

        // Remove trailing cars
        cars.RemoveAll(item => item.position > car.position);

        // Remove car and instantiate wreckage object
        Debug.Log("Train car destroyed!");
        cars.Remove(car);
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Wreckage"), car.transform.position, Quaternion.identity);
        obj.transform.position = car.transform.position;
    }

    // Remove abandoned train cars
    public void deleteAbandoned()
    {
        foreach (TrainCar car in GetComponentsInChildren<TrainCar>())
        {
            if (car.abandoned)
            {
                Destroy(car.gameObject);
            }
        }
    }
}

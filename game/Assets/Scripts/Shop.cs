using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public void nextLevel()
    {
        GameController.self.EndLevel();
    }

    // Add another train car
    public void AddCar()
    {
        GameController.self.AddCar();
    }

    // Repair everything
    public void RepairTrain()
    {
        GameController.self.RepairTrain();
    }

    // Shootier turrets
    public void ShootierTurrets()
    {
        GameController.self.ShootierTurrets();
    }

    // Overclock train engine
    public void OverclockTrainEngine()
    {
        GameController.self.OverclockTrainEngine();
    }
}

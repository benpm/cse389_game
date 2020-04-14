using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    GameObject bottom;
    GameObject hpBarPrefab;

    // Start is called before the first frame update
    void Start()
    {
        bottom = transform.Find("Bottom").gameObject;
        hpBarPrefab = Resources.Load<GameObject>("Prefabs/HPBar");
    }

    // Update train UI stuff, like train healthbars
    public void UpdateTrainInfo(Train train)
    {
        // Delete all healthbars
        foreach (Transform child in bottom.transform)
        {
            Destroy(child.gameObject);
        }
        // Add healthbars for all train cars
        int ncars = train.cars.Count;
        int n = 0;
        float leftSide = (ncars * 120.0f) / 2.0f;
        foreach (TrainCar car in train.cars)
        {
            GameObject newHPBar = Instantiate(hpBarPrefab, bottom.transform);
            newHPBar.GetComponent<UI_Healthbar>().car = car;
            newHPBar.transform.localPosition = new Vector2(n * 120.0f - leftSide, 0);
            n += 1;
        }
        Debug.Log("Updated train UI info");
    }
}

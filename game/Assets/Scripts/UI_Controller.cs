using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    GameObject bottom;
    GameObject hpBarPrefab;
    Text moneyDisplay;
    int moneyShakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        bottom = transform.Find("Bottom").gameObject;
        hpBarPrefab = Resources.Load<GameObject>("Prefabs/HPBar");
        moneyDisplay = transform.Find("MoneyText").GetComponent<Text>();
    }

    void Update()
    {
        if (moneyShakeTimer > 0)
        {
            Vector3 scale = moneyDisplay.transform.localScale;
            scale.x = Random.Range(0.8f, 1.2f);
            scale.y = Random.Range(0.8f, 1.2f);
            if (moneyShakeTimer == 1)
            {
                scale.y = scale.x = 1;
            }
            moneyDisplay.transform.localScale = scale;
            moneyShakeTimer -= 1;
        }
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
            UI_Healthbar uiHealthBar = newHPBar.GetComponent<UI_Healthbar>();
            uiHealthBar.who = car.GetComponent<Attackable>();
            newHPBar.SetActive(true);
            newHPBar.transform.localPosition = new Vector2(n * 120.0f - leftSide, 0);
            n += 1;
        }
        Debug.Log("Updated train UI info");
    }

    public void setMoney(int money)
    {
        moneyDisplay.text = $"${money}";
        moneyShakeTimer = 15;
    }
}

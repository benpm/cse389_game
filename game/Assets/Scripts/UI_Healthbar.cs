using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    private Image hpBar;
    public TrainCar car;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = transform.Find("BarFG").GetComponent<Image>();
        Debug.Assert(hpBar);
        Debug.Assert(car);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = car.hp / (float)car.maxhp;
        hpBar.color = Color.Lerp(Color.red, Color.yellow, hpBar.fillAmount);
    }
}

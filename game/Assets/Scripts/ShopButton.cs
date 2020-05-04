using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    Button button;
    int cost;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        cost = int.Parse(transform.Find("Cost/CostText").GetComponent<Text>().text.Substring(1));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.self.money < cost)
        {
            button.enabled = false;
            Color color = button.image.color;
            color.a = 0.2f;
            button.image.color = color;
        }
        else
        {
            button.enabled = true;
            Color color = button.image.color;
            color.a = 1.0f;
            button.image.color = color;
        }
    }

    public void buy()
    {
        GameController.self.PlaySound("buy");
        GameController.self.money -= cost;
    }
}

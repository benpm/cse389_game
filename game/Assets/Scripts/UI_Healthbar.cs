using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    private Image hpBar;
    public Attackable who;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = transform.Find("BarFG").GetComponent<Image>();
        Debug.Assert(hpBar);
        Debug.Assert(who);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = who.hp / (float)who.maxHp;
        hpBar.color = Color.Lerp(Color.yellow, Color.green, hpBar.fillAmount);
        if (hpBar.fillAmount < 0.5)
        {
            hpBar.color = Color.Lerp(Color.red, Color.yellow, hpBar.fillAmount * 2.0f);
        }
    }
}

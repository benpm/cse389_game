using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public enum State
    {
        Alive, Dying, Recovering, Dead
    }

    public int maxHp = 100;
    public int hp = 100;
    public State state = State.Alive;
    public int moneyValue = 10;

    private int recoverTimer;
    private Rigidbody2D body;
    private UI_Healthbar hpBar;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Transform hpBarObject = transform.Find("HPBar");
        if (hpBarObject)
        {
            hpBar = hpBarObject.GetComponent<UI_Healthbar>();
            Debug.LogFormat("{0} --> {1}", name, hpBar);
        }
        hp = maxHp;
        Debug.Assert(body);
    }

    void Update()
    {
        switch(state)
        {
            case State.Recovering:
                recoverTimer -= 1;
                if (recoverTimer <= 0)
                {
                    SendMessage("recovered", SendMessageOptions.DontRequireReceiver);
                    state = State.Alive;
                }
                break;
            case State.Dying:
                SendMessageUpwards("dead", gameObject, SendMessageOptions.DontRequireReceiver);
                body.velocity = Vector2.zero;
                Destroy(gameObject, 1.0f);
                body.simulated = false;
                if (gameObject.layer == LayerMask.NameToLayer("Hittable"))
                {
                    GameController.self.money += moneyValue;
                }
                state = State.Dead;
                break;
        }
    }

    public void heal()
    {
        hp = maxHp;
    }

    // Collision with bullet
    void OnParticleCollision(GameObject other)
    {
        if (state != State.Dying)
        {
            state = State.Recovering;
            recoverTimer = 60;
            hp -= 10;
            GameController.self.PlaySound("hit");
            SendMessage("hit");
            if (hpBar)
            {
                Debug.Log("Enabling hp bar");
                hpBar.gameObject.SetActive(true);
            }
        }
        if (hp <= 0 && state != State.Dying)
        {
            state = State.Dying;
        }
    }
}

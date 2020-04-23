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
                SendMessage("dead", SendMessageOptions.DontRequireReceiver);
                body.velocity = Vector2.zero;
                Destroy(gameObject, 1.0f);
                body.simulated = false;
                state = State.Dead;
                break;
        }
    }

    // Collision with bullet
    void OnParticleCollision(GameObject other)
    {
        if (state != State.Dying)
        {
            state = State.Recovering;
            recoverTimer = 60;
            hp -= 10;
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

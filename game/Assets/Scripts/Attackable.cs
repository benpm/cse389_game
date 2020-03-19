using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public enum State
    {
        Alive, Dying, Recovering
    }

    public int hp = 100;
    public State state = State.Alive;

    private int recoverTimer;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
                    SendMessage("recovered");
                    state = State.Alive;
                }
                break;
            case State.Dying:
                transform.localScale *= 0.8f;
                body.velocity = Vector2.zero;
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
        }
        if (hp <= 0 && state != State.Dying)
        {
            state = State.Dying;
            Destroy(gameObject, 1.0f);
        }
    }
}

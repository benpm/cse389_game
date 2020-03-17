using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Running, Attacking, Recovering, Dying
    }

    public int hp = 100;

    private State state;
    private Rigidbody2D body;
    private int recoverTimer;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && state != State.Dying)
        {
            state = State.Dying;
            Destroy(gameObject, 1.0f);
        }
        TrainCar car = GameController.self.train.getClosestCar(transform.position);
        float dist = GameController.self.train.distance(transform.position);
        switch (state)
        {
            case State.Running:
                transform.position = Vector3.MoveTowards(transform.position, car.transform.position, 0.1f);
                if (dist < 2)
                    state = State.Attacking;
                break;
            case State.Attacking:
                if (dist > 4)
                    state = State.Running;
                break;
            case State.Recovering:
                recoverTimer -= 1;
                if (recoverTimer <= 0)
                {
                    state = State.Running;
                }
                break;
            case State.Dying:
                transform.localScale *= 0.8f;
                body.velocity = Vector2.zero;
                break;
        }

        // Rigidbody easing
        body.velocity *= 0.6f;
        body.angularVelocity *= 0.3f;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, 0, 0.01f));
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
    }
}

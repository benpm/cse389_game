using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Running, Attacking
    }
    
    public int attackFreq = 30;
    public float followRadius = 24;
    public int attackDamage = 20;

    private State state;
    private Rigidbody2D body;
    private int recoverTimer;
    private int attackTimer;
    private Attackable attackable;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        attackable = GetComponent<Attackable>();
        attackTimer = attackFreq;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackable.state == Attackable.State.Alive)
        {
            TrainCar car = GameController.self.train.getClosestCar(transform.position);
            float dist = GameController.self.train.distance(transform.position);
            switch (state)
            {
                case State.Running:
                    if (Vector2.Distance(transform.position, car.transform.position) < followRadius)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, car.transform.position, 0.1f);
                        if (dist < 3)
                            state = State.Attacking;
                    }
                    break;
                case State.Attacking:
                    if (dist > 4)
                        state = State.Running;
                    attackTimer -= 1;
                    if (attackTimer <= 0)
                    {
                        attackTimer = attackFreq;
                        Vector3 dir = -Vector3.Normalize(transform.position - car.transform.position) * 10.0f;
                        GameController.self.enemyBulletSystem.emit(transform.position, dir);
                    }
                    break;
            }
        }

        // Rigidbody easing
        body.velocity *= 0.6f;
        body.angularVelocity = 0.0f;
    }

    // Recovered from attack
    void recovered()
    {
        state = State.Running;
    }

    // Dead
    void dead()
    {
    }
}

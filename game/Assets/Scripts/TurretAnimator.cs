﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAnimator : MonoBehaviour
{
    enum State
    {
        Hit, Operating, Dying
    };
    State state = State.Operating;

    int hitTimer = 0;
    const int hitTime = 30;

    // Update is called once per frame
    void Update()
    {
        // Shakey hit animation
        if (state == State.Hit && hitTimer > 0)
        {
            float val = (hitTimer / (float)hitTime);
            Vector3 scale = transform.localScale;
            scale.y = 1.0f + Random.Range(-0.1f, 0.1f) * val;
            scale.x = 1.0f + Random.Range(-0.1f, 0.1f) * val;
            transform.localScale = scale;

            hitTimer -= 1;
        }

        // Death animation
        if (state == State.Dying)
        {
            Vector3 scale = transform.localScale;
            transform.localScale = Vector3.Lerp(scale, Vector3.zero, 0.2f);
        }
    }

    public void dead()
    {
        state = State.Dying;
    }

    public void recovered()
    {
        state = State.Operating;
    }

    public void hit()
    {
        state = State.Hit;
        hitTimer = hitTime;
    }
}
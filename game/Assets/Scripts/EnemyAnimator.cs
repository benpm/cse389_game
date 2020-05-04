using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    enum State
    {
        Hit, Walking, Resting, Dying
    };
    State state = State.Walking;

    int hitTimer = 0;
    const int hitTime = 30;
    Vector3 initScale;

    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Bouncy hit animation
        if (state == State.Hit && hitTimer > 0)
        {
            float val = Mathf.Sin((hitTimer / (float)hitTime) * Mathf.PI) * (hitTimer / (float)hitTime);
            Vector3 scale = transform.localScale;
            scale.y = initScale.y + val;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = val * 20.0f;
            transform.localScale = scale;
            transform.rotation = Quaternion.Euler(rotation);

            hitTimer -= 1;
        }

        // Walking animation
        if (state == State.Walking)
        {
            float t = Time.timeSinceLevelLoad;
            float val = Mathf.Sin(t * 15.0f);
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = val * 8.0f;
            transform.rotation = Quaternion.Euler(rotation);
        }

        // Death animation
        if (state == State.Dying)
        {
            Vector3 scale = transform.localScale;
            transform.localScale = Vector3.Lerp(scale, Vector3.zero, 0.2f);
        }
    }

    public void dead(GameObject who)
    {
        state = State.Dying;
    }

    public void recovered()
    {
        state = State.Walking;
    }

    public void hit()
    {
        state = State.Hit;
        hitTimer = hitTime;
    }
}

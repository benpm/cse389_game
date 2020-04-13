using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void emit(Vector3 pos, Vector3 dir, float velocity = 30.0f)
    {
        ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
        param.position = pos;
        param.velocity = dir.normalized * velocity;
        particles.Emit(param, 1);
    }
}

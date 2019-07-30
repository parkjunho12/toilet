using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCols : MonoBehaviour
{
    public ParticleSystem particleLauncher;
    
    List<ParticleCollisionEvent> collisionEvent;    

    // Start is called before the first frame update
    void Start()
    {
        collisionEvent = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FixedTouchField._uniqueInstance.PRESSED)
        //if (Input.GetMouseButton(0))
        {
            particleLauncher.transform.position = GameObject.FindWithTag("ShootPointer").transform.position;
            ParticleSystem.MainModule psmain = particleLauncher.main;
            particleLauncher.Emit(2);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        //other = GameObject.FindWithTag("Toilet");
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvent);
        Debug.Log("+1점");
    }
}

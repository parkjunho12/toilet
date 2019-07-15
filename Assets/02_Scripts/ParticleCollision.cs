using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
   public ParticleSystem particleLauncher;
    public ParticleSystem splatter;

    List<ParticleCollisionEvent> collisionEvent;
    public Gradient particleGradient;
    // Start is called before the first frame update
    void Start()
    {
        collisionEvent = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvent);
        Debug.Log("파티클 충돌이 일어났습니다");
        for(int i = 0; i < collisionEvent.Count; i++) {
            EmitAtLocation(collisionEvent[i]);
        }
        

    }
    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatter.transform.position = particleCollisionEvent.intersection;
        Debug.Log(splatter.transform.position);
        
        Debug.Log(splatter.transform.position);
        splatter.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);

        ParticleSystem.MainModule psMain = splatter.main;
        psMain.startColor = particleGradient.Evaluate(Random.Range(0f, 1f));
        splatter.Emit(10);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ParticleSystem.MainModule psmain = particleLauncher.main;
            particleLauncher.Emit(1);
        }
       
    }

}

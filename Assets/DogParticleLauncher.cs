using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogParticleLauncher : MonoBehaviour
{
    public static DogParticleLauncher _uniqueInstance;
    public ParticleSystem particleLauncher;
    public ParticleSystem splatter;
    public Gradient particleGradient;

    List<ParticleCollisionEvent> collisionEvent;

    float _sum;

    public float SUM
    {
        get { return _sum; }
        set { _sum = value; }
    }

    void Start()
    {
        _uniqueInstance = this;
        collisionEvent = new List<ParticleCollisionEvent>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY)
        {
            particleLauncher.transform.position = GameObject.FindGameObjectWithTag("DogShootPointer").transform.position;
            ParticleSystem.MainModule psmain = particleLauncher.main;
            psmain.startColor = particleGradient.Evaluate(Random.Range(0, 1));
            particleLauncher.Emit(1);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.CompareTag("Toilet"))
        {
            _sum += 0.01f;
        }
        else if(other.gameObject.CompareTag("Fly"))
        {
            _sum += 2.0f;
        }

        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvent);
        for(int i = 0; i < collisionEvent.Count; i++)
        {
            EmitAtLocation(collisionEvent[i]);
        }
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatter.transform.position = particleCollisionEvent.intersection;
        splatter.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);

        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor = particleGradient.Evaluate(Random.Range(0f, 1f));
        splatter.Emit(10);
    }
}

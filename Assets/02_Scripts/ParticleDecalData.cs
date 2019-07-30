using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool
{
    public Vector3 position;
    public float size;
    public Vector3 rotation;
    public Color color;
}

public class ParticleDecalData : MonoBehaviour
{
    public int maxDecals = 100;
    public float decalsSizeMin = .5f;
    public float decalsSizeMax = 1.5f;

    private ParticleSystem decalParticleSystem;
    private int particleDecalDataIndex;

    private ParticleDecalPool[] particleData;
    private ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        SetParticleData(particleCollisionEvent, colorGradient);
        DisplayParticles();
    }
    
    void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        if(particleDecalDataIndex >= maxDecals)
        {
            particleDecalDataIndex = 0;
        }

        particleData[particleDecalDataIndex].position = particleCollisionEvent.intersection;

        Vector3 particleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
        particleRotationEuler.z = UnityEngine.Random.Range(0, 360);
        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
        particleData[particleDecalDataIndex].size = UnityEngine.Random.Range(decalsSizeMin, decalsSizeMax);
        particleData[particleDecalDataIndex].color = colorGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
        particleDecalDataIndex++;
    }

    void DisplayParticles()
    {
        for(int i = 0; i < particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }

        decalParticleSystem.SetParticles(particles, particles.Length);
    }
}

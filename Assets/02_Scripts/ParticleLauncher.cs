using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleLauncher : MonoBehaviour
{
    public static ParticleLauncher _uniqueInstance;
    public AudioClip _peeSound;
    public AudioClip _hitFlySound;
    [SerializeField] GameObject _peeScore;

    public ParticleSystem particleLauncher;         // 총알 발사되는 파티클 개체
    public ParticleSystem splatter;                 // 발사된 총알개체가 벽에 충돌될때 호출되는 충돌반응 이펙트  

    List<ParticleCollisionEvent> collisionEvent;
    public Gradient particleGradient;

    float _timeCheck;
    float _urinalScore;
    float _flyScore;
    float _sum;
    bool _hit;

    public float URINAL
    {
        get { return _urinalScore; }
        set { _urinalScore = value; }
    }
    public float FLY
    {
        get { return _flyScore; }
        set { _flyScore = value; }
    }
    public float SUM
    {
        get { return _sum; }
        set { _sum = value; }
    }
    public bool HIT
    {
        get { return _hit; }
        set { _hit = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;
        collisionEvent = new List<ParticleCollisionEvent>();
    }

    // 파티클 충돌 이벤트 감지
    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Toilet"))
        {
            _peeScore.GetComponent<Text>().text = string.Format("점수 : {0}", (_urinalScore + _flyScore).ToString("N1"));
            _urinalScore += 0.01f;
        }
        if (other.gameObject.CompareTag("Fly"))
        {
            Debug.Log("2점");
            //SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.HITFLY);
            AudioSource.PlayClipAtPoint(_hitFlySound, transform.position);
            _peeScore.GetComponent<Text>().text = string.Format("점수 : {0}", (_urinalScore + _flyScore).ToString("N1"));
            _flyScore += 5.0f;
        }

        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvent);
        for (int i = 0; i < collisionEvent.Count; i++)
        {
            EmitAtLocation(collisionEvent[i]);
        }
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatter.transform.position = particleCollisionEvent.intersection;
        splatter.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);

        ParticleSystem.MainModule psMain = splatter.main;
        psMain.startColor = particleGradient.Evaluate(Random.Range(0f, 1f));
        splatter.Emit(10);
    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY)
        {
            if (FixedTouchField._uniqueInstance.PRESSED)
            {
                particleLauncher.transform.position = GameObject.FindWithTag("ShootPointer").transform.position;
                ParticleSystem.MainModule psmain = particleLauncher.main;
                psmain.startColor = particleGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
                particleLauncher.Emit(1);

                _timeCheck += Time.deltaTime;
                if(_timeCheck > 0.1f)
                {
                    _timeCheck = 0;
                    AudioSource.PlayClipAtPoint(_peeSound, particleLauncher.transform.position);
                }
            }
            else
            {
                _timeCheck = 0;
            }
        }


    }
}

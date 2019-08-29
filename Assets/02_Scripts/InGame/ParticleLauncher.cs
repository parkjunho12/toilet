using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleLauncher : MonoBehaviour
{
    public static ParticleLauncher _uniqueInstance;
    public AudioClip[] _soundEff;
    [SerializeField] GameObject[] _peeScore;
    [SerializeField] GameObject[] _plus;

    public ParticleSystem particleLauncher;         // 총알 발사되는 파티클 개체
    public ParticleSystem splatter;                 // 발사된 총알개체가 벽에 충돌될때 호출되는 충돌반응 이펙트  

    List<ParticleCollisionEvent> collisionEvent;
    public Gradient particleGradient;

    float _timeCheck;
    float _urinalScore;
    float _flyScore;
    float _sum;
    bool _hit;
    int _rndNum;

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

        _rndNum = PlayerControl._uniqueInstance.RNDNUM;
    }

    // 파티클 충돌 이벤트 감지
    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Toilet"))
        {
            _urinalScore += 0.01f;
            _sum += _urinalScore;
            _peeScore[_rndNum].GetComponent<Text>().text = string.Format("점수 : {0}", _sum.ToString("N1"));
        }
        else if (other.gameObject.CompareTag("Fly"))
        {
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.HITFLY);
            _flyScore += 5.0f;
            _sum += _flyScore;
            _peeScore[_rndNum].GetComponent<Text>().text = string.Format("점수 : {0}", _sum.ToString("N1"));
            _plus[_rndNum].GetComponent<Text>().transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 0.05f, other.transform.position.z + 0.01f);
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
            bool iskeydown = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
            //if (FixedTouchField._uniqueInstance.PRESSED)
            if (iskeydown)
            {
                
                particleLauncher.transform.position = GameObject.FindWithTag("ShootPointer").transform.position;
                ParticleSystem.MainModule psmain = particleLauncher.main;
                psmain.startColor = particleGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
                particleLauncher.Emit(1);

                _timeCheck += Time.deltaTime;
                if(_timeCheck > 0.1f)
                {
                    _timeCheck = 0;
                    AudioSource.PlayClipAtPoint(_soundEff[0], particleLauncher.transform.position);
                }
            }
            else
            {
                _timeCheck = 0;
            }
        }


    }
}

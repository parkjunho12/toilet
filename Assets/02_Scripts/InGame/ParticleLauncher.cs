using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleLauncher : MonoBehaviour
{
    public static ParticleLauncher _uniqueInstance;
    public AudioClip[] _soundEff;
    [SerializeField] GameObject _isDogActive;
    [SerializeField] GameObject[] _peeScore;
    [SerializeField] GameObject[] _plus;
    [SerializeField] GameObject[] _scoreEff;        // 물폭탄, 별폭죽, BAAM, 솜사탕폭죽

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
            _sum += 0.01f;
            _peeScore[_rndNum].GetComponent<Text>().text = string.Format("점수 : {0}", _sum.ToString("N1"));
        }
        else if (other.gameObject.CompareTag("Fly"))
        {
            Handheld.Vibrate();
            //AudioSource.PlayClipAtPoint(_soundEff[1], transform.position);
            if (_isDogActive.activeSelf == true)
            {
                _plus[_rndNum].GetComponent<Text>().text = "+ 3";
                _sum += 3.0f;
                _flyScore += 3.0f;
            }
            else
            {
                _plus[_rndNum].GetComponent<Text>().text = "+ 2";
                _sum += 2.0f;
                _flyScore += 2.0f;
            }

            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.HITFLY);
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

        if (_flyScore % 150 == 0)
        {// 솜사탕
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.COMBO);
            _scoreEff[3].GetComponent<ParticleSystem>().Play();
            _scoreEff[3].transform.position = particleCollisionEvent.intersection;
            _scoreEff[3].transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
        }
        else if (_flyScore % 330 == 0)
        {// BAAM
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.COMBO_YEAHH);
            _scoreEff[2].GetComponent<ParticleSystem>().Play();
            _scoreEff[2].transform.position = particleCollisionEvent.intersection;
            _scoreEff[2].transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
        }
        else if (_flyScore % 810 == 0)
        {// 별
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.COMBO_SHINE);
            _scoreEff[1].GetComponent<ParticleSystem>().Play();
            _scoreEff[1].transform.position = particleCollisionEvent.intersection;
            _scoreEff[1].transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
        }

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

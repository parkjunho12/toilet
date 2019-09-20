using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarTest : MonoBehaviour
{
    [SerializeField] float _movSpeed = 15.0f;
    [SerializeField] float _rotSpeed = 5.0f;
    [SerializeField] GameObject _startPos;
    [SerializeField] GameObject _endPos;
    [SerializeField] GameObject _auraShield;
    [SerializeField] GameObject _carCrashBoom;

    List<Vector3> _carMovePoints;
    Vector3 _posTarget;
    GameObject _prefabPlayer;

    int _curIndex;
    int _nextIndex;
    float _timeCheck;
    bool _distance;

    void Awake()
    {
        _prefabPlayer = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CarPass(Random.Range(5, 15)));
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPos.transform.position, _movSpeed * Time.deltaTime);
            //transform.LookAt(_carMovePoints[_nextIndex]);
            //transform.eulerAngles = new Vector3(270, 0, transform.rotation.z);

            if (Vector3.Distance(transform.position, _endPos.transform.position) <= 0.3f)
            {
                _timeCheck += Time.deltaTime;

                if (_timeCheck >= 1.5f)
                {
                    this.gameObject.transform.position = _startPos.transform.position;
                }
            }
            else if(Vector3.Distance(transform.position, _prefabPlayer.transform.position) < 3.5f)
            {
                SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_HORN);
            }

        }
        else if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator CarPass(float _delaytime)
    {
        yield return new WaitForSeconds(_delaytime);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_PASS, 0.3f);
        StartCoroutine(CarPass(Random.Range(5, 15)));

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            if (LobbyManager._uniqueInstance.ISSHIELD.GetComponent<Text>().text.Equals("1"))
            {
                SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHIELD, 1.0f);
                LobbyManager._uniqueInstance.ISSHIELD.GetComponent<Text>().text = "0";
                _auraShield.SetActive(false);
            }
            else
            {
                _carCrashBoom.GetComponent<ParticleSystem>().Play();
                SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_CRASH);
                LobbyManager._uniqueInstance.PLAYCOUNT -= 25.0f;
                //StartCoroutine(returnIDLE(5.0f));
                //UIFader._uniqueInstance.UIELEMENT.GetComponent<Image>().color = Color.white;
                //UIFader._uniqueInstance.FadeIn(0.4f);
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float _movSpeed = 15.0f;
    [SerializeField] float _rotSpeed = 5.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND /*||
            LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY*/)
        {
            transform.position = Vector3.MoveTowards(transform.position, _carMovePoints[_nextIndex], _movSpeed * Time.deltaTime);
            //transform.LookAt(_carMovePoints[_nextIndex]);
            transform.eulerAngles = new Vector3(270, 0, transform.rotation.z);

            if(Vector3.Distance(transform.position, _carMovePoints[_nextIndex]) <= 0.3f)
            {
                _timeCheck += Time.deltaTime;

                if(_timeCheck >= 1.5f)
                {
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_PASS, 0.3f);
                    Destroy(this.gameObject);
                }
            }
            
            if (Vector3.Distance(transform.position, _prefabPlayer.transform.position) <= 3.5f)
            {
                SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_HORN);
                _distance = true;
            }
        }
    }

    /// <summary>
    /// 이 스크립트가 속한 차가 움직일 위치 초기화.
    /// </summary>
    /// <param name="points"></param>
    public void SettingMovePathRoamming(Transform[] points = null)
    {
        _carMovePoints = new List<Vector3>();

        for(int n = 0; n < points.Length; n++)
        {
            _carMovePoints.Add(points[n].position);
        }
        _nextIndex = _curIndex + 1;
        transform.position = _carMovePoints[_curIndex];
        transform.LookAt(_carMovePoints[_nextIndex]);
    }
}

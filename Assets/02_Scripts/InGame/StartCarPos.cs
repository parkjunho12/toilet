using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCarPos : MonoBehaviour
{
    public static StartCarPos _uniqueInstance;
    [SerializeField] GameObject[] _prefabCar;

    Transform[] _roamPoints;
    List<GameObject> _ltSpawns;
    
    bool _spawnCheck;
    float _timeCheck;
    float _rndTime;

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;

        _ltSpawns = new List<GameObject>();
        GatheringCarRoamPoint();

        _rndTime = UnityEngine.Random.Range(5, 9);
    }

    // Update is called once per frame
    void Update()
    {
        if(LobbyManager._uniqueInstance.ENABLESPAWN)
        {
            _timeCheck += Time.deltaTime;
            if(_timeCheck >= _rndTime)
            {
                _timeCheck = 0;
                SpawnCarMovePath();
            }
        }
    }

    void LateUpdate()
    {
        foreach(GameObject item in _ltSpawns)
        {
            if(item == null)
            {
                _ltSpawns.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    private void SpawnCarMovePath()
    {
        GameObject go;
        CarController car = new CarController();
        int _rndCarNum = UnityEngine.Random.Range(0, _prefabCar.Length);

        go = Instantiate(_prefabCar[_rndCarNum]);
        car = go.GetComponent<CarController>();
        car.transform.parent = _roamPoints[0].transform;
        car.transform.position = _roamPoints[0].transform.position;
        car.SettingMovePathRoamming(_roamPoints);
        _ltSpawns.Add(car.gameObject);
    }

    /// <summary>
    /// 차가 움직일 위치 초기화.
    /// </summary>
    private void GatheringCarRoamPoint()
    {
        if (transform.childCount == 0)
            return;

        _roamPoints = new Transform[transform.childCount];
        for(int n = 0; n < _roamPoints.Length; n++)
        {
            _roamPoints[n] = transform.GetChild(n);
        }
    }

}

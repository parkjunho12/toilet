﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBottlePos : MonoBehaviour
{
    [SerializeField] GameObject _bottle;

    Transform[] _roamPoints;
    List<GameObject> _ltSpawns;

    bool _spawnCheck;

    // Start is called before the first frame update
    void Awake()
    {
        _ltSpawns = new List<GameObject>();
        GatheringBottleRoamPoint();

        _spawnCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(LobbyManager._uniqueInstance.ENABLESPAWN)
        {
            if(_spawnCheck)
            {
                SpawnBottle();
                _spawnCheck = false;
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

    public void SpawnBottle()
    {
        GameObject[] go = new GameObject[_roamPoints.Length];

        for(int n = 0; n < _roamPoints.Length; n++)
        {
            int _rndSetActive = UnityEngine.Random.Range(0, 2);
            go[n] = Instantiate(_bottle);
            go[n].transform.parent = _roamPoints[n].transform;
            go[n].transform.position = _roamPoints[n].transform.position;
            _ltSpawns.Add(go[n]);

            if (_rndSetActive % 2 == 0)
                go[n].SetActive(true);
            else
                go[n].SetActive(false);
        }
    }

    /// <summary>
    /// 빈물병 스폰 위치 초기화.
    /// </summary>
    public void GatheringBottleRoamPoint()
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

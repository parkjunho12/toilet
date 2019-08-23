using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartBtn : MonoBehaviour
{
    public static GameStartBtn _uniqueInstance;

    [SerializeField] GameObject _prefabFly;

    public Renderer lamp;
    private Color originColor;

    Transform _flyrootRoam;         // 파리 포인트.
    Transform[] _flyPoints;
    List<GameObject> _ftSpawns;

    bool spawnCheck;
    bool _clickBtn;
    public bool CLICKBTN
    {
        get { return _clickBtn; }
        set { _clickBtn = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;

        _flyrootRoam = transform.GetChild(9);
        GatheringFlyRoammingPoint();
        _ftSpawns = new List<GameObject>();
        spawnCheck = true;

        originColor = lamp.material.color;
        _clickBtn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager._uniqueInstance.NOWGAMESTATE >= LobbyManager.eGameState.PLAY)
        {
            lamp.material.color = originColor;
            return;
        }
        else if (LobbyManager.INSTANCE.ENABLESPAWN)
        {
            if (spawnCheck)
            {
                SpawnFlyPos();
                spawnCheck = false;
            }
        }


        //if (_clickBtn)
        //{
        //    lamp.material.color = Color.gray;
        //    return;
        //}

        float flicker = Mathf.Abs(Mathf.Sin(Time.time * 10));
        lamp.material.color = originColor * flicker;
    }

    private void SpawnFlyPos()
    {
        FlyController fly;

        GameObject fo = _prefabFly;
        fly = fo.GetComponent<FlyController>();
        fly.SettingFlyMovePathRoamming(_flyPoints);
        _ftSpawns.Add(fo);
    }

    void GatheringFlyRoammingPoint()
    {
        if (_flyrootRoam.childCount == 0)
            return;

        _flyPoints = new Transform[_flyrootRoam.childCount];
        for (int n = 0; n < _flyPoints.Length; n++)
        {
            _flyPoints[n] = _flyrootRoam.GetChild(n);
        }
    }
}

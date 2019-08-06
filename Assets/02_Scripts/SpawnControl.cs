using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] GameObject _prefabFly;

    List<GameObject> _ltSpawns;
    List<GameObject> _ftSpawns;
    Transform _rootRoam;
    Transform[] _roamPoints;
    Transform _flyrootRoam;         // 파리 포인트.
    Transform[] _flyPoints;

    float _timeCheck;
    bool spawnCheck;
    float _timeSpawn = 3;

    public static SpawnControl _uniqueInstance;

    public bool SPAWNCHECK
    {
        get { return spawnCheck; }
        set { spawnCheck = value; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        _ltSpawns = new List<GameObject>();
        _ftSpawns = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rootRoam = transform.GetChild(0);
        _flyrootRoam = transform.GetChild(1);
        GatheringRoammingPoint();
        GatheringFlyRoammingPoint();
        spawnCheck = true;
    }

    void Update()
    {
        if (LobbyManager.INSTANCE.ENABLESPAWN)
        {
            //_timeCheck += Time.deltaTime;
            //if (_timeCheck >= _timeSpawn)
            {
                if (spawnCheck)
                {
                    SpawnObjAtOne();
                    spawnCheck = false;
                }
            }
        }
    }

    void SpawnObjAtOne()
    {
        GameObject go = _prefabPlayer;
        GameObject fo = _prefabFly;
        PlayerControl ply;
        FlyController fly;

        ply = go.GetComponent<PlayerControl>();
        fly = fo.GetComponent<FlyController>();
        ply.SettingRoammingType(_roamPoints);
        fly.SettingFlyMovePathRoamming(_flyPoints);
        _ltSpawns.Add(go);
        _ftSpawns.Add(fo);

        PlayerControl._uniqueInstance.CURSTATE = PlayerControl.ePlayerActState.RUN;
        //Debug.Log("SpawnControl SpawnObj Success");
    }

    void GatheringRoammingPoint()
    {
        if (_rootRoam.childCount == 0)
            return;

        _roamPoints = new Transform[_rootRoam.childCount];
        for (int n = 0; n < _roamPoints.Length; n++)
        {
            _roamPoints[n] = _rootRoam.GetChild(n);
        }
    }

    void GatheringFlyRoammingPoint()
    {
        if (_flyrootRoam.childCount == 0)
            return;

        _flyPoints = new Transform[_flyrootRoam.childCount];
        for(int n = 0; n < _flyPoints.Length; n++)
        {
            _flyPoints[n] = _flyrootRoam.GetChild(n);
        }
    }
}

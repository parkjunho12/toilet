using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] GameObject _prefabPlayer;
    
    List<GameObject> _ltSpawns;
    Transform _rootRoam;
    Transform[] _roamPoints;

    float _timeCheck;
    bool spawnCheck;
    float _timeSpawn = 3;

    void Awake()
    {
        _ltSpawns = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rootRoam = transform.GetChild(0);
        GatheringRoammingPoint();
        spawnCheck = true;
    }

    void Update()
    {
        if (LobbyManager.INSTANCE.ENABLESPAWN)
        {
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= _timeSpawn)
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
        PlayerControl ply;

        ply = go.GetComponent<PlayerControl>();
        ply.SettingRoammingType(_roamPoints);
        _ltSpawns.Add(go);       
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

        Debug.Log("Spawn Success");
    }
}

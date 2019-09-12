using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMovePos : MonoBehaviour
{
    [SerializeField] GameObject _prefabPlayer;
    //[SerializeField] GameObject _prefabDog;

    Transform[] _roamPoints;
    List<GameObject> _ltSpawns;
    //List<GameObject> _ltSpawnss;

    bool spawnCheck;

    // Start is called before the first frame update
    void Start()
    {
        _ltSpawns = new List<GameObject>();
        //_ltSpawnss = new List<GameObject>();
        GatheringRoammingPoint();

        spawnCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager.INSTANCE.ENABLESPAWN)
        {
            if (spawnCheck)
            {
                SpawnPlyWalkPath();
                spawnCheck = false;
            }
        }
    }

    private void SpawnPlyWalkPath()
    {
        GameObject go = _prefabPlayer;
        //GameObject goo = _prefabDog;
        PlayerControl ply;
        //PlayerController dog;

        ply = go.GetComponent<PlayerControl>();
        //dog = goo.GetComponent<PlayerController>();
        ply.SettingWalkPathRoamming(_roamPoints);
        //dog.SettingWalkPathRoamming(_roamPoints);
        _ltSpawns.Add(go);
        //_ltSpawnss.Add(goo);

        //Debug.Log("StartMovePos WalkPath Success");
    }

    public void GatheringRoammingPoint()
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

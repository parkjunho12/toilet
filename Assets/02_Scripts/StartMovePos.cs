using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMovePos : MonoBehaviour
{
    [SerializeField] GameObject _prefabPlayer;

    Transform[] _roamPoints;
    List<GameObject> _ltSpawns;

    bool spawnCheck;

    // Start is called before the first frame update
    void Start()
    {
        _ltSpawns = new List<GameObject>();
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
        PlayerControl ply;

        ply = go.GetComponent<PlayerControl>();
        ply.SettingWalkPathRoamming(_roamPoints);
        _ltSpawns.Add(go);

        Debug.Log("StartMovePos WalkPath Success");
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

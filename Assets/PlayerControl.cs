using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public enum ePlayerActState
    {
        RUN,
        IDEL,
    }

    public static PlayerControl _uniqueInstance;

    Animator aniCtrl;
    NavMeshAgent _naviAgent;

    Transform _lookPos;
    Transform _gameStartBtn;
    List<Vector3> _ltPoints;
    Vector3 _posTarget;
    ePlayerActState _curPlyState;

    int _idxRoamming = 0;
    bool _isActing;

    public ePlayerActState CURSTATE
    {
        get { return _curPlyState; }
        set { _curPlyState = value; }
    }

    void Awake()
    {
        aniCtrl = GetComponent<Animator>();
        _naviAgent = GetComponent<NavMeshAgent>();

        _posTarget = transform.position;
        _lookPos = GameObject.FindWithTag("LookPos").transform;
        _gameStartBtn = GameObject.FindWithTag("GameStartBtn").transform;
        _isActing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(LobbyManager.INSTANCE.NOWGAMESTATE == LobbyManager.eGameState.PLYRUNNING)
        {
            switch(_curPlyState)
            {
                case ePlayerActState.RUN:
                    if (Vector3.Distance(transform.position, _ltPoints[_idxRoamming]) < 0.2f)
                    {
                         ChangedAction(PlayerControl.ePlayerActState.RUN);
                        _idxRoamming++;
                        _isActing = false;
                    }
                    break;
                case ePlayerActState.IDEL:
                    _isActing = true;
                    break;
            }

            ProcessAI();
        }
    }

    public void ProcessAI()
    {
        if (_isActing)
            return;

        if (_idxRoamming == _ltPoints.Count)
        {
            ChangedAction(ePlayerActState.IDEL);
            Vector3 tp = transform.position;
            Quaternion tq = Quaternion.LookRotation(_gameStartBtn.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, tq, Time.deltaTime * 5);
            return;
        }

        _posTarget = _ltPoints[_idxRoamming];
        _naviAgent.SetDestination(_posTarget);
        _isActing = true;
    }

    public void ChangedAction(ePlayerActState state)
    {
        switch(state)
        {
            case ePlayerActState.RUN:
                _naviAgent.enabled = true;
                _naviAgent.speed = 3.5f;
                _naviAgent.stoppingDistance = 0;
                break;
            case ePlayerActState.IDEL:
                _naviAgent.enabled = false;
                break;
            default:
                break;
        }
        aniCtrl.SetInteger("AniState", (int)state);
        _curPlyState = state;
    }

    public void SettingRoammingType(Transform[] points = null)
    {
         _ltPoints = new List<Vector3>();
         for (int n = 0; n < points.Length; n++)
         {
             _ltPoints.Add(points[n].position);
         }
        Debug.Log("SettingRoamming Success");
    }
}

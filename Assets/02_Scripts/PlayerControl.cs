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
        WALK
    }
    
    public static PlayerControl _uniqueInstance;
    public GameObject _shootPos;

    protected float ShootAngle;
    protected float ShootAngleSpeed = 0.2f;

    Animator aniCtrl;
    NavMeshAgent _naviAgent;
    Rigidbody _rigidbody;
    
    Transform _lookPos;
    Transform _gameStartBtn;
    List<Vector3> _ltPoints;
    List<Vector3> _walkPoints;
    Vector3 _posTarget;
    ePlayerActState _curPlyState;

    float _timeCheck;
    int _idxRoamming = 0;
    bool _isActing;

    public ePlayerActState CURSTATE
    {
        get { return _curPlyState; }
        set { _curPlyState = value; }
    }
    public bool ISACTING
    {
        set { _isActing = value; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        aniCtrl = GetComponent<Animator>();
        _naviAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
        _posTarget = transform.position;
        _lookPos = GameObject.FindWithTag("LookPos").transform;
        _isActing = false;
        _timeCheck = 0;
        
        _shootPos.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(LobbyManager.INSTANCE.NOWGAMESTATE == LobbyManager.eGameState.PLYRUNNING)
        if (!SpawnControl._uniqueInstance.SPAWNCHECK)
        {
            switch(_curPlyState)
            {
                case ePlayerActState.RUN:
                    _timeCheck += Time.deltaTime;
                    //Debug.Log(_timeCheck);
                    if(_timeCheck > 2.7f)
                    {
                        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
                        _timeCheck = 0;
                    }

                    if (!SpawnControl._uniqueInstance.SPAWNCHECK)
                    {
                        if (Vector3.Distance(transform.position, _ltPoints[_idxRoamming]) < 0.2f)
                        {
                             ChangedAction(PlayerControl.ePlayerActState.RUN);
                            _idxRoamming++;
                            _isActing = false;
                        }
                    }
                    break;
                case ePlayerActState.IDEL:
                    _isActing = true;
                    break;
                case ePlayerActState.WALK:
                    if (Vector3.Distance(transform.position, _walkPoints[_idxRoamming]) < 0.2f)
                    {
                        _shootPos.SetActive(true);

                        LobbyManager._uniqueInstance.NOWGAMESTATE = LobbyManager.eGameState.START;      // 게임 시작
                        ChangedAction(PlayerControl.ePlayerActState.IDEL);
                        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.ZIPPERDOWN);
                        _isActing = true;
                        //_idxRoamming++;
                        //_isActing = false;
                    }
                    break;
            }

            ProcessAI();
            transform.LookAt(_lookPos);
        }
    }
    
    public void ProcessAI()
    {
        if (_isActing)
            return;

        if (_idxRoamming == _ltPoints.Count)
        {
            _idxRoamming = 0;
            ChangedAction(ePlayerActState.IDEL);
            //Quaternion tq = Quaternion.LookRotation(_lookPos.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, tq, Time.deltaTime * 5);
            //transform.LookAt(_lookPos);
            return;
        }

        _posTarget = _ltPoints[_idxRoamming];
        _naviAgent.SetDestination(_posTarget);
        _isActing = true;
    }

    public void PlayerWalkToToilet()
    {
        if (_isActing)
            return;

        //Debug.Log("hello");
        if (_idxRoamming == _walkPoints.Count)
        {// 다 걸어왔으면 제자리 멈춤 && 발사가능
            ChangedAction(ePlayerActState.IDEL);

            return;
        }

        ChangedAction(PlayerControl.ePlayerActState.WALK);
        _posTarget = _walkPoints[_idxRoamming];
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
            case ePlayerActState.WALK:
                _naviAgent.enabled = true;
                _naviAgent.speed = 1.0f;
                _naviAgent.stoppingDistance = 0;
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
        //Debug.Log("SettingRoamming Success");
    }
    public void SettingWalkPathRoamming(Transform[] points = null)
    {
        _walkPoints = new List<Vector3>();
        for (int n = 0; n < points.Length; n++)
        {
            _walkPoints.Add(points[n].position);
        }
        //Debug.Log("SettingWalkPathRoamming Success");

    }
}

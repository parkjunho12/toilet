using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public enum ePlayerActState
    {
        IDLE,
        WALK,
        RUN
    }
    
    public static PlayerControl _uniqueInstance;
    public GameObject _shootPos;
    public GameObject _unrinal;

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
        Debug.Log(_curPlyState);
        //if(LobbyManager.INSTANCE.NOWGAMESTATE == LobbyManager.eGameState.PLYRUNNING)
        if (!SpawnControl._uniqueInstance.SPAWNCHECK)
        {
            switch(_curPlyState)
            {
                case ePlayerActState.RUN:
                    if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
                    {
                        _timeCheck += Time.deltaTime;
                        if (_timeCheck > 2.7f)
                        {// 숨소리 효과음.
                            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
                            _timeCheck = 0;
                        }

                        if (FixedTouchField._uniqueInstance.PRESSED)
                        {// 화면이 터치될 시 캐릭터 움직임..
                            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
                        }
                        else
                        {// 화면 터치가 안됬을 시 캐릭터 IDLE..
                            ChangedAction(ePlayerActState.IDLE);
                        }
                    }

                    if (Vector3.Distance(transform.position, _unrinal.transform.position) <= 1.5f)
                    {// 내 캐릭터와 소변기 거리가 1.5 이하이면 => 스타트버튼 클릭 후 게임 시작..
                        LobbyManager._uniqueInstance.StartBtn();
                        _curPlyState = ePlayerActState.WALK;
                    }

                    //if (!SpawnControl._uniqueInstance.SPAWNCHECK)
                    //{
                    //    if (Vector3.Distance(transform.position, _ltPoints[_idxRoamming]) < 0.2f)
                    //    {
                    //         ChangedAction(PlayerControl.ePlayerActState.RUN);
                    //        _idxRoamming++;
                    //        _isActing = false;
                    //    }
                    //}
                    break;
                case ePlayerActState.IDLE:
                    _isActing = true;
                    if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
                    {
                        if (FixedTouchField._uniqueInstance.PRESSED)
                        {
                            ChangedAction(ePlayerActState.RUN);
                        }
                        else
                        {
                            ChangedAction(ePlayerActState.IDLE);
                        }
                    }
                    break;
                case ePlayerActState.WALK:
                    if (Vector3.Distance(transform.position, _walkPoints[_idxRoamming]) < 0.2f)
                    {
                        _shootPos.SetActive(true);

                        LobbyManager._uniqueInstance.NOWGAMESTATE = LobbyManager.eGameState.START;      // 게임 시작
                        ChangedAction(PlayerControl.ePlayerActState.IDLE);
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
            ChangedAction(ePlayerActState.IDLE);
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
        
        if (_idxRoamming == _walkPoints.Count)
        {// 다 걸어왔으면 제자리 멈춤 && 발사가능
            ChangedAction(ePlayerActState.IDLE);
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
            case ePlayerActState.IDLE:
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

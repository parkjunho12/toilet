using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public enum ePlayerActState
    {
        IDLE,
        WALK,
        RUN,
    }
    
    public static PlayerControl _uniqueInstance;
    public GameObject _shootPos;
    public GameObject[] _unrinal;
    public GameObject _arrow;

    protected float ShootAngle;
    protected float ShootAngleSpeed = 0.2f;

    Animator aniCtrl;
    NavMeshAgent _naviAgent;
    Rigidbody _rigidbody;

    GameObject _car;
    Transform _lookPos;
    Transform _gameStartBtn;
    List<Vector3> _walkPoints;
    Vector3 _posTarget;
    ePlayerActState _curPlyState;

    float _timeCheck;
    int _idxRoamming = 0;
    int _rndNumber;
    bool _isActing;
    bool _crash;

    public ePlayerActState CURSTATE
    {// 플레이어 에니메이션 상태.
        get { return _curPlyState; }
        set { _curPlyState = value; }
    }
    public bool ISACTING
    {
        set { _isActing = value; }
    }
    public int RNDNUM
    {// 랜덤하게 켜질 변기번호 및 플레이어가 걸어갈 위치번호..
        get { return _rndNumber; }
        set { _rndNumber = value; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        aniCtrl = GetComponent<Animator>();
        _naviAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
        _posTarget = transform.position;

        _isActing = false;
        _timeCheck = 0;

        //_shootPos.SetActive(false);

        //_rndNumber = Random.Range(0, _unrinal.Length);
        _rndNumber = 1;
        _unrinal[_rndNumber].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_curPlyState);
        //if (!SpawnControl._uniqueInstance.SPAWNCHECK)
        //if(LobbyManager.INSTANCE.NOWGAMESTATE == LobbyManager.eGameState.PLYRUNNING)
        {
            switch (_curPlyState)
            {
                case ePlayerActState.RUN:
                    if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
                    {
                        _timeCheck += Time.deltaTime;
                        if (_timeCheck > 2.7f)
                        {// 숨소리 효과음.
                            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
                            _timeCheck = 0;

                            if (_crash)
                            {
                                _crash = false;
                                UIFader._uniqueInstance.UIELEMENT.GetComponent<Image>().color = Color.yellow;
                                UIFader._uniqueInstance.FadeIn(LobbyManager._uniqueInstance.FADENUM);
                            }
                        }

                        if (FixedTouchField._uniqueInstance.PRESSED)
                        {// 화면이 터치될 시 캐릭터 움직임..
                         // 시간차에 따른 캐릭터 달리기 속도 저하..
                            if (LobbyManager._uniqueInstance.PLAYCOUNT > 50)
                            {
                                transform.Translate(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)
                                    * 8.5f * Time.deltaTime);
                            }
                            else if (LobbyManager._uniqueInstance.PLAYCOUNT <= 50
                                && LobbyManager._uniqueInstance.PLAYCOUNT > 10)
                            {
                                transform.Translate(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)
                                                                    * 6.0f * Time.deltaTime);
                            }
                            else
                            {
                                transform.Translate(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)
                                                                    * 5.0f * Time.deltaTime);
                            }
                        }
                        else
                        {// 화면 터치가 안됬을 시 캐릭터 IDLE..
                            ChangedAction(ePlayerActState.IDLE);
                        }
                    }

                    if (Vector3.Distance(transform.position, _unrinal[_rndNumber].transform.position) <= 1.5f)
                    {// 내 캐릭터와 소변기 거리가 1.5 이하이면 => 스타트버튼 클릭 후 게임 시작..
                        LobbyManager._uniqueInstance.StartBtn();
                        _curPlyState = ePlayerActState.WALK;
                        _arrow.SetActive(false);
                    }
                    
                    break;
                case ePlayerActState.IDLE:
                    _isActing = true;
                    if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
                    {
                        if (FixedTouchField._uniqueInstance.PRESSED)
                        {
                            aniCtrl.enabled = true;
                            ChangedAction(ePlayerActState.RUN);
                        }
                        else
                        {
                            ChangedAction(ePlayerActState.IDLE);
                        }
                    }
                    else if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY)
                    {
                        aniCtrl.enabled = false;
                    }
                    break;
                case ePlayerActState.WALK:
                    if (Vector3.Distance(transform.position, _walkPoints[_rndNumber]) < 0.15f)
                    {
                        _shootPos.SetActive(true);

                        LobbyManager._uniqueInstance.NOWGAMESTATE = LobbyManager.eGameState.START;      // 게임 시작
                        ChangedAction(ePlayerActState.IDLE);
                        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.ZIPPERDOWN);
                        LobbyManager._uniqueInstance.PLAYCOUNT = 50.0f;
                        _isActing = true;
                    }
                    break;
            }
            
        }
    }

    public void PlayerWalkToToilet()
    {
        if (_isActing)
            return;
        
        if (_rndNumber == _walkPoints.Count)
        {// 다 걸어왔으면 제자리 멈춤 && 발사가능
            ChangedAction(ePlayerActState.IDLE);
            return;
        }

        ChangedAction(PlayerControl.ePlayerActState.WALK);
        _posTarget = _walkPoints[_rndNumber];
        _naviAgent.SetDestination(_posTarget);
        _isActing = true;
    }

    /// <summary>
    /// 플레이어 에니메이션 체인지.
    /// </summary>
    /// <param name="state"></param>
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

    /// <summary>
    /// 플레이어가 소변기 주변에 갈 때 걸어갈 위치스폰 초기화.
    /// </summary>
    /// <param name="points"></param>
    public void SettingWalkPathRoamming(Transform[] points = null)
    {
        _walkPoints = new List<Vector3>();
        for (int n = 0; n < points.Length; n++)
        {
            _walkPoints.Add(points[n].position);
        }
        //Debug.Log("SettingWalkPathRoamming Success");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bottle"))
        {
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.FINISHPEE);
            LobbyManager._uniqueInstance.PLAYCOUNT += 15.0f;
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Car"))
        {
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_CRASH);
            UIFader._uniqueInstance.UIELEMENT.GetComponent<Image>().color = Color.white;
            UIFader._uniqueInstance.FadeIn(0.4f);
            _crash = true;
            Destroy(other.gameObject);
            LobbyManager._uniqueInstance.PLAYCOUNT -= 10.0f;
        }
    }
}

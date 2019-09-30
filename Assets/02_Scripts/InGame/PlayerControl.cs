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
        CRASH_TO_CAR,
    }
    
    public static PlayerControl _uniqueInstance;
    public GameObject _shootPos;
    public GameObject[] _unrinal;
    public GameObject _arrow;
    public GameObject _auraShield;
    public GameObject centerEye;
    protected float ShootAngle;
    protected float ShootAngleSpeed = 0.2f;
    public Vector2 joystick;
    public static Vector3 _movedir;
    public Text _isShield; 

    Animator aniCtrl;
    NavMeshAgent _naviAgent;
    Rigidbody _rigidbody;
    Transform _controllerPos;
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
    float dirX = 0;
    float dirZ = 0;

    public ePlayerActState CURSTATE
    {// 플레이어 에니메이션 상태.
        get { return _curPlyState; }
        set { _curPlyState = value; }
    }
    public bool ISACTING
    {
        get { return _isActing; }
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
        _controllerPos = GameObject.FindGameObjectWithTag("ControllerSpawn").transform;
        _shootPos.SetActive(false);
        //_rndNumber = 0;
        //_unrinal[0].SetActive(true);
        _rndNumber = Random.Range(0, _unrinal.Length);
        _unrinal[_rndNumber].SetActive(true);

        StartCoroutine(Breath(7.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //if (!SpawnControl._uniqueInstance.SPAWNCHECK)
        //if(LobbyManager.INSTANCE.NOWGAMESTATE == LobbyManager.eGameState.PLYRUNNING)
        {
            switch (_curPlyState)
            {
                case ePlayerActState.RUN:
                    if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
                    {
                        //_timeCheck += Time.deltaTime;
                        //if (_timeCheck > 2.7f)
                        {// 숨소리 효과음.
                            //SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
                            //_timeCheck = 0;
                            //if (_crash)
                            //{
                            //    _crash = false;
                            //    UIFader._uniqueInstance.UIELEMENT.GetComponent<Image>().color = Color.yellow;
                            //    UIFader._uniqueInstance.FadeIn(LobbyManager._uniqueInstance.FADENUM);
                            //}
                        }

                        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || Input.GetMouseButton(0))
                        {
                            PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.RUN);
                            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);
                            transform.eulerAngles = new Vector3(0, centerEye.transform.localEulerAngles.y, 0);
                            var absX = Mathf.Abs(coord.x);
                            var absY = Mathf.Abs(coord.y);
                            if (absX > absY)
                            {
                                if (coord.x > 0)
                                    dirX = +1;
                                else
                                    dirX = -1;
                            }
                            else
                            {
                                if (coord.y > 0)
                                    dirZ = +1;
                                else
                                    dirZ = -1;
                            }

                            if (LobbyManager._uniqueInstance.PLAYCOUNT > 50)
                            {

                                _movedir = new Vector3(dirX * 8.5f, 0, dirZ *8.5f);
                                transform.Translate(_movedir * Time.smoothDeltaTime);
                            }
                            else if (LobbyManager._uniqueInstance.PLAYCOUNT <= 50
                                && LobbyManager._uniqueInstance.PLAYCOUNT > 10)
                            {
                                bool iskeydown = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
                                //if (FixedTouchField._uniqueInstance.PRESSED)
                                if (iskeydown)
                                {
                                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.EAAA);
                                    _movedir = new Vector3(dirX * 16.5f, 0, dirZ * 16.5f);                           
                                }
                                else
                                {
                                  
                                    _movedir = new Vector3(dirX * 7.0f, 0, dirZ * 7.0f);
                                }

                                transform.Translate(_movedir * Time.smoothDeltaTime);
                            }
                            else
                            {
                                _movedir = new Vector3(dirX * 5.5f, 0, dirZ * 5.5f);
                                transform.Translate(_movedir * Time.smoothDeltaTime);
                            }
                        }
                        else
                        {// 화면 터치가 안됬을 시 캐릭터 IDLE..
                            PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.IDLE);
                            dirX = 0;
                            dirZ = 0;
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
                        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) )
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
                    StopCoroutine(Breath(2.7f));
                    if (Vector3.Distance(transform.position, _walkPoints[_rndNumber]) < 0.15f)
                    {
                        _isActing = true;
                        _shootPos.SetActive(true);
                        _auraShield.SetActive(false);

                        ChangedAction(ePlayerActState.IDLE);
                        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.ZIPPERDOWN);
                        LobbyManager._uniqueInstance.NOWGAMESTATE = LobbyManager.eGameState.START;      // 게임 시작
                        LobbyManager._uniqueInstance.PLAYCOUNT = 50.0f;
                    }
                    break;
                default:
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
                _naviAgent.enabled = true;                
                break;
            case ePlayerActState.WALK:
                _naviAgent.enabled = true;
                _naviAgent.speed = 1.0f;
                _naviAgent.stoppingDistance = 0;
                break;
            case ePlayerActState.CRASH_TO_CAR:
                _naviAgent.enabled = false;
                StartCoroutine(returnIDLE(4.0f));
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

    //float Timer;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bottle"))
        {
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.FINISHPEE);
            LobbyManager._uniqueInstance.PLAYCOUNT += 15.0f;
            Destroy(other.gameObject);
        }

        //if (other.gameObject.CompareTag("Car"))
        //{
        //    Destroy(other.gameObject);
        //    if (_isShield.GetComponent<Text>().text.Equals("1"))
        //    {
        //        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHIELD, 1.0f);
        //        _auraShield.SetActive(false);
        //       // Debug.Log(Timer);
        //        //if (Timer > 0.0227f)
        //        {
        //           //Timer = 0.0f;
        //            _isShield.GetComponent<Text>().text = "0";
        //        }
        //    }
        //    if(_isShield.GetComponent<Text>().text != "1")
        //    {
        //        _crash = true;
        //        ChangedAction(ePlayerActState.CRASH_TO_CAR);
        //        StartCoroutine(returnIDLE(5.0f));
        //        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.CAR_CRASH);
        //        UIFader._uniqueInstance.UIELEMENT.GetComponent<Image>().color = Color.white;
        //        UIFader._uniqueInstance.FadeIn(0.4f);
        //        LobbyManager._uniqueInstance.PLAYCOUNT -= 25.0f;
        //    }
        //}
    }

    IEnumerator Breath(float _delayTime)
    {
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH, 0.3f);
        yield return new WaitForSeconds(_delayTime);
        StartCoroutine(Breath(7.0f));
        //if (_crash)
        //{
        //    _crash = false;
        //    UIFader._uniqueInstance.UIELEMENT.GetComponent<Image>().color = Color.yellow;
        //    UIFader._uniqueInstance.FadeIn(LobbyManager._uniqueInstance.FADENUM);
        //}
    }

    IEnumerator returnIDLE(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        _curPlyState = ePlayerActState.IDLE;
    }
}

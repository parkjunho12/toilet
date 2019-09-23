using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public enum eGameState
    {
        READY   = 0,
        MAPSETTING,
        PLYRUNNING,
        STARTFIND,
        START,
        PLAY,
        RESULT,
        HAP_RESULT,
        END,
        NONE,
    }

    public static LobbyManager _uniqueInstance;
    public GameObject[] _startPosition;
    public GameObject _Arrow;
    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] GameObject _prefabDog;
    [SerializeField] GameObject _toiletWaterFall;
    [SerializeField] GameObject _bottle;
    [SerializeField] GameObject _touchShootUI;
    [SerializeField] GameObject _prefabCarPoints;
    [SerializeField] GameObject _auraShield;
    [SerializeField] GameObject[] _gameStateUI;
    [SerializeField] GameObject[] _gameStateTxt;
    [SerializeField] GameObject[] _ctf3Light;
    [SerializeField] Text[] _timer;
    [SerializeField] Text[] _myScore;
    [SerializeField] Text[] _Plus;
    [SerializeField] Text _findTimer;
    public Text _isShield;
    BaseGameManager.eStageState _curStageIdx;
    PlayerControl _player;

    eGameState _curState;
    string cAddress = "http://dbwo4011.cafe24.com/unity/Check2.php";
    float _timeCheck;
    float _score;
    float _fadeNum;
    bool _isSpawn;
    int _rndNum;
    int _final_time;

    public static LobbyManager INSTANCE
    {
        get { return _uniqueInstance; }
    }
    public eGameState NOWGAMESTATE
    {
        get { return _curState; }
        set { _curState = value; }
    }
    public bool ENABLESPAWN
    {
        get { return _isSpawn; }
        set { _isSpawn = value; }
    }
    public float PLAYCOUNT
    {
        get { return _timeCheck; }
        set { _timeCheck = value; }
    }
    public float FADENUM
    {
        get { return _fadeNum; }
        set { _fadeNum = value; }
    }
    public Text ISSHIELD
    {
        get { return _isShield; }
        set { _isShield = value; }
    }
    public int FINAL_TIME
    {
        get { return _final_time; }
        set { _final_time = value; }
    }
    public GameObject[] GAMESTATE_TEXT
    {
        get { return _gameStateTxt; }
        set { _gameStateTxt = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;
        SettingPlayer();

        _rndNum = PlayerControl._uniqueInstance.RNDNUM;
        _score = 0.0f;
        _timer[_rndNum].text = _timeCheck.ToString("N2");
        _myScore[_rndNum].text = "점수 : " + _score.ToString();

        _touchShootUI.SetActive(true);
        _gameStateUI[_rndNum].SetActive(false);
        _gameStateTxt[_rndNum].GetComponent<Text>();
        _gameStateTxt[_rndNum].SetActive(false);       
    }

    // Update is called once per frame
    void Update()
    {
        _prefabPlayer.transform.rotation = Quaternion.Euler(0, this.transform.rotation.y, 0);
        //_miniMap.transform.position = new Vector3(_prefabPlayer.transform.position.x, 30, _prefabPlayer.transform.position.z);
        //_miniMap.transform.rotation = Quaternion.Euler(90, 0, 0);

        //Debug.Log(_curState);
        switch (_curState)
        {
            case eGameState.READY:
                GameReady();
                GameMapSetting();
                break;
            case eGameState.MAPSETTING:                
                _timeCheck = 95.0f;
                _curState = eGameState.STARTFIND;               
                break;
            case eGameState.STARTFIND:
                _timeCheck -= Time.deltaTime;
                _findTimer.text = _timeCheck.ToString("N2");
                if (_timeCheck <= 90 && _timeCheck > 50)
                {
                    _fadeNum = 0.1f;
                    UIFader._uniqueInstance.FadeIn(_fadeNum);
                }
                else if (_timeCheck <= 50 && _timeCheck > 40)
                {
                    _fadeNum = 0.2f;
                    UIFader._uniqueInstance.FadeIn(_fadeNum);
                }
                else if (_timeCheck <= 40 && _timeCheck > 30)
                {
                    _fadeNum = 0.3f;
                    UIFader._uniqueInstance.FadeIn(_fadeNum);
                }
                else if (_timeCheck <= 30 && _timeCheck > 20)
                {
                    _fadeNum = 0.5f;
                    UIFader._uniqueInstance.FadeIn(_fadeNum);
                }
                else if (_timeCheck <= 20 && _timeCheck > 10)
                {
                    _fadeNum = 0.7f;
                    UIFader._uniqueInstance.FadeIn(_fadeNum);
                }
                else if (_timeCheck <= 10 && _timeCheck > 0)
                {
                    _fadeNum = 0.9f;
                    UIFader._uniqueInstance.FadeIn(_fadeNum);
                }
                else if (_timeCheck <= 0)
                {// 게임오버 표시,플레이어 에니메이션(IDLE), 자동차들 안보이게, 화면 노랗게 유지.
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.GAMEOVER, 1.0f);
                    PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.IDLE);
                    _prefabCarPoints.SetActive(false);
                    UIFader._uniqueInstance.FadeIn(1.0f);
                    _findTimer.text = "GameOver";
                    _timeCheck = 0;
                    _curState = eGameState.END;
                    //_curState = eGameState.REPLAY_IFNOT_FINISH;
                }
                _final_time = (int)_timeCheck * 10;
                break;
            case eGameState.START:
                _gameStateUI[_rndNum].SetActive(true);
                _gameStateTxt[_rndNum].SetActive(true);                               // 현재 게임상태 등장.
                _gameStateTxt[_rndNum].GetComponent<Text>().text = "GameStart!";      // GameStart! 문구 나옴.
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 51.5f)
                {
                    _prefabCarPoints.SetActive(false);
                    _findTimer.enabled = false;
                    _gameStateTxt[_rndNum].SetActive(false);                          // 현재 게임상태 가림.
                    _ctf3Light[_rndNum].SetActive(false);
                    _timeCheck = 50.0f;
                    UIFader._uniqueInstance.FadeOut();
                    _curState = eGameState.PLAY;
                }
                break;
            case eGameState.PLAY:
                _timeCheck -= Time.deltaTime;
                _timer[_rndNum].text = _timeCheck.ToString("N2");        // 소수점 두번째 까지 표현.                

                if (_timeCheck <= 0)
                {
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.FINISHPEE);

                    _timeCheck = 0;
                    _timer[_rndNum].text = _timeCheck.ToString("N2");                // 소수점 두번째 까지 표현.       
                    _gameStateTxt[_rndNum].SetActive(true);
                    _gameStateTxt[_rndNum].GetComponent<Text>().text = "GameOver~";  // 현재 게임상태.
                    _curState = eGameState.RESULT;
                }
                break;
            case eGameState.RESULT:
                _timeCheck += 10;
                _gameStateTxt[_rndNum].GetComponent<Text>().text = "Score : " + _timeCheck.ToString();
                if (_timeCheck >= ParticleLauncher._uniqueInstance.SUM)
                {
                    _timeCheck = 0;
                    _gameStateTxt[_rndNum].GetComponent<Text>().text = "Score : " + ParticleLauncher._uniqueInstance.SUM.ToString() +"\nTime Bonus : " +_final_time;
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BREATH);
                    ParticleLauncher._uniqueInstance.SUM = ParticleLauncher._uniqueInstance.SUM + _final_time;
                    _touchShootUI.SetActive(false);
                    _Plus[_rndNum].gameObject.SetActive(false);
                    _curState = eGameState.HAP_RESULT;
                }
                break;
            case eGameState.HAP_RESULT:
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 3)
                {
                    StartCoroutine(this.Call(cAddress));
                    StartCoroutine(this.PlusGold("http://dbwo4011.cafe24.com/unity/PlusGold.php"));
                    _curState = eGameState.END;
                }
                break;
            case eGameState.END:
                _curState = eGameState.NONE;
                StartCoroutine(GoToLobby(3.0f));
                //SceneChanger._uniqueInstance.FadeToLevel(1);
                break;
        }
    }

    IEnumerator GoToLobby(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        //SceneChanger._uniqueInstance.FadeToLevel02(1);
        SceneChanger._uniqueInstance.OnFadeCompleteInGame();
    }

    public void GameReady()
    {
        _curState = eGameState.READY;
    }

    public void GameMapSetting()
    {
        _curState = eGameState.MAPSETTING;
        // 스폰 포인트 활성화.
        _isSpawn = true;
        // 플레이어 소변기에 접근시 걸어갈 위치활성화.
        // 빈물병 스폰 위치활성화.
       
        // 카메라 워킹 위치 설정.
        //Transform tf = GameObject.FindGameObjectWithTag("CameraPosRoot").transform;
        //Camera.main.GetComponent<ActionCamera>().SetCameraActionRoot(tf);
        UIFader._uniqueInstance.FadeOut();
        _timeCheck = 0;
    }

    /// <summary>
    /// 게임시작시 플레이어 스폰위치. 
    /// </summary>
    public void SettingPlayer()
    {
        _prefabPlayer.transform.position = _startPosition[0].transform.position;
        _prefabPlayer.transform.rotation = _startPosition[0].transform.rotation;
        _prefabDog.transform.position = _startPosition[1].transform.position;
        _prefabDog.transform.rotation = _startPosition[1].transform.rotation;
        StartCoroutine(this.PlayerItem("http://dbwo4011.cafe24.com/unity/PlayerItem.php"));

    }
    public IEnumerator Call(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("UID", SystemInfo.deviceUniqueIdentifier);
        cForm.AddField("id", ((int)ParticleLauncher._uniqueInstance.SUM));
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        Debug.Log(wwwUrl.text);
    }
    public IEnumerator PlusGold(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("UID", SystemInfo.deviceUniqueIdentifier);
        cForm.AddField("id", (int)ParticleLauncher._uniqueInstance.SUM );
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        Debug.Log(wwwUrl.text);
    }
    public IEnumerator PlayerItem(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("UID", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        String Arrow = "";
        String Shield = "";
        String Pet = "";
        string[] split_text;
        split_text = wwwUrl.text.Split(' ');
        Arrow = split_text[0];
        Shield = split_text[1];
        Pet = split_text[2];

        if (int.Parse(Arrow) == 1)
        {
            _Arrow.SetActive(true);
        }
        else
        {
            _Arrow.SetActive(false);
        }

        if (int.Parse(Pet) == 1)
        {
            _prefabDog.SetActive(true);
        }
        else
        {
            _prefabDog.SetActive(false);
        }

        if (int.Parse(Shield) > 0)
        {
            Debug.Log(Shield);
            StartCoroutine(this.UseShield("http://dbwo4011.cafe24.com/unity/UseShield.php", int.Parse(Shield)));
            _isShield.GetComponent<Text>().text = "1";
            _auraShield.SetActive(true);
        }

    }

    public IEnumerator UseShield(string _address,int Sheildnum)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("UID", SystemInfo.deviceUniqueIdentifier);
        cForm.AddField("Shield", Sheildnum - 1);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
    }

    /// <summary>
    /// 게임 시작 메소드.
    /// 플레이어가 변기 주변에 가까이 갈시
    /// 물이 내려가고, 플레이어가 걸어간다.
    /// </summary>
    public void StartBtn()
    {
        _touchShootUI.SetActive(true);
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.TOILET_SOUND);

        // 소변기 물내려가는 이펙트 및 소리
        Transform tf = GameObject.FindGameObjectWithTag("ToiletWaterFall").transform;
        GameObject go = Instantiate(_toiletWaterFall, tf.position, tf.rotation);
        Destroy(go, 4);
        _prefabPlayer.transform.LookAt(tf);
        
        PlayerControl._uniqueInstance.ISACTING = false;
        PlayerControl._uniqueInstance.PlayerWalkToToilet();
    }
    /// <summary>
    /// 소변기에서 게임을 완료했을시 게임다시시작.
    /// </summary>
    public void RestartBtn()
    {
        int stageIdx = UnityEngine.Random.Range(1, 2);
        if ((int)_curStageIdx != stageIdx)
            _curStageIdx = (BaseGameManager.eStageState)stageIdx;
        else
            _curStageIdx = 0;

        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.ZIPPERUP);
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.TOILET_SOUND);

        // 소변기 물내려가는 이펙트 및 소리
        Transform tf = GameObject.FindGameObjectWithTag("ToiletWaterFall").transform;
        GameObject go = Instantiate(_toiletWaterFall, tf.position, tf.rotation);
        Destroy(go, 7);

        BaseGameManager._uniqueinstance.SceneRestart(_curStageIdx);
    }
    /// <summary>
    /// 소변기를 찾지 못했을 시 게임다시시작 메소드.
    /// </summary>
    public void RestartClick()
    {
        int stageIdx = UnityEngine.Random.Range(1, 2);
        if ((int)_curStageIdx != stageIdx)
            _curStageIdx = (BaseGameManager.eStageState)stageIdx;
        else
            _curStageIdx = 0;
        
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        BaseGameManager._uniqueinstance.SceneRestart(_curStageIdx);
    }

    public void QuitBtn()
    {
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        //_prefabeQuitGame.SetActive(true);
    }
}

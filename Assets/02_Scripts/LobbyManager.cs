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
        END,
        RESULT,
    }

    public static LobbyManager _uniqueInstance;
    public GameObject[] _startPosition;

    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] GameObject _toiletWaterFall;
    [SerializeField] GameObject _touchShootUI;
    [SerializeField] Text _findTimer;
    [SerializeField] GameObject[] _gameStateUI;
    [SerializeField] GameObject[] _gameStateTxt;
    [SerializeField] Text[] _timer;
    [SerializeField] Text[] _myScore;

    BaseGameManager.eStageState _curStageIdx;
    PlayerControl _player;
    eGameState _curState;

    float _timeCheck;
    float _score;
    bool _isSpawn;
    int _rndNum;

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
        _prefabPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Debug.Log(_curState);
        switch(_curState)
        {
            case eGameState.READY:
                GameReady();
                GameMapSetting();
                break;
            case eGameState.MAPSETTING:                
                _timeCheck = 180.0f;
                _curState = eGameState.STARTFIND;               
                break;
            case eGameState.STARTFIND:
                _timeCheck -= Time.deltaTime;
                _findTimer.text = _timeCheck.ToString("N2");
                if (_timeCheck <= 140 && _timeCheck > 70)
                {
                    UIFader._uniqueInstance.FadeIn(0.1f);
                }
                else if (_timeCheck <= 70 && _timeCheck > 60)
                {
                    UIFader._uniqueInstance.FadeIn(0.2f);
                }
                else if (_timeCheck <= 60 && _timeCheck > 50)
                {
                    UIFader._uniqueInstance.FadeIn(0.3f);
                }
                else if (_timeCheck <= 50 && _timeCheck > 40)
                {
                    UIFader._uniqueInstance.FadeIn(0.4f);
                }
                else if (_timeCheck <= 40 && _timeCheck > 30)
                {
                    UIFader._uniqueInstance.FadeIn(0.6f);
                }
                else if (_timeCheck <= 30 && _timeCheck > 20)
                {
                    UIFader._uniqueInstance.FadeIn(0.8f);
                }
                else if (_timeCheck <= 20 && _timeCheck > 0)
                {
                    UIFader._uniqueInstance.FadeIn(1.0f);
                }
                //if (_timeCheck == 85.0f)
                break;
            case eGameState.START:
                _gameStateUI[_rndNum].SetActive(true);
                _gameStateTxt[_rndNum].SetActive(true);                  // 현재 게임상태 등장.
                _gameStateTxt[_rndNum].GetComponent<Text>().text = "GameStart!";      // GameStart! 문구 나옴.
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 51.5f)
                {
                    _findTimer.enabled = false;
                    _gameStateTxt[_rndNum].SetActive(false);             // 현재 게임상태 가림.
                    _timeCheck = 50.0f;
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
                    _curState = eGameState.END;
                }
                break;
            case eGameState.END:
                //_gameReStartBtn.SetActive(true);
                _timeCheck += Time.deltaTime;

                if(_timeCheck >= 1.0f)
                {
                    _timeCheck = 0;
                    _gameStateTxt[_rndNum].GetComponent<Text>().text = "Score : " + ParticleLauncher._uniqueInstance.SUM.ToString("N1");
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BREATH);
                    _touchShootUI.SetActive(false);
                    _curState = eGameState.RESULT;
                }
                break;
            case eGameState.RESULT:
                break;
        }
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
    }

    /// <summary>
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
        Destroy(go, 7);
        
        PlayerControl._uniqueInstance.ISACTING = false;
        PlayerControl._uniqueInstance.PlayerWalkToToilet();
    }
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
    public void QuitBtn()
    {
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        //_prefabeQuitGame.SetActive(true);
    }
    public void YesClick()
    {
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        //Application.Quit();
    }
    public void NoClick()
    {
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        //_prefabeQuitGame.SetActive(false);
    }
}

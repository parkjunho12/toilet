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
        START,
        PLAY,
        END,
        RESULT,
    }

    public static LobbyManager _uniqueInstance;
    public GameObject[] _startPosition;

    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] GameObject _gameStartBtn;
    [SerializeField] GameObject _gameReStartBtn;
    [SerializeField] GameObject _fly;
    [SerializeField] GameObject _toiletWaterFall;
    [SerializeField] GameObject _touchShootUI;
    [SerializeField] GameObject _gameStateUI;
    [SerializeField] GameObject _gameStateTxt;
    [SerializeField] Text _timer;
    [SerializeField] Text _myScore;

    BaseGameManager.eStageState _curStageIdx;
    List<Vector3> _flyMovePos;
    PlayerControl _player;
    eGameState _curState;

    float _timeCheck;
    float _score;
    bool _isSpawn;

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

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;
        SettingPlayer();

        _timeCheck = 50.0f;
        _score = 0.0f;
        _timer.text = _timeCheck.ToString("N2");
        _myScore.text = "점수 : " + _score.ToString();

        _touchShootUI.SetActive(false);
        _gameStateUI.SetActive(false);
        _gameStateTxt.GetComponent<Text>();
        _gameStateTxt.SetActive(false);
        _gameReStartBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(_curState)
        {
            case eGameState.READY:
                GameReady();
                GameMapSetting();
                break;
            case eGameState.START:
                _gameStateUI.SetActive(true);
                _gameStateTxt.SetActive(true);                  // 현재 게임상태 등장.
                _gameStateTxt.GetComponent<Text>().text = "GameStart!";      // GameStart! 문구 나옴.
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 51.5f)
                {
                    _gameStateTxt.SetActive(false);             // 현재 게임상태 가림.
                    _timeCheck = 50.0f;
                    _curState = eGameState.PLAY;
                }
                break;
            case eGameState.PLAY:
                _timeCheck -= Time.deltaTime;
                _timer.text = _timeCheck.ToString("N2");        // 소수점 두번째 까지 표현.                

                if (_timeCheck <= 0)
                {
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.FINISHPEE);

                    _timeCheck = 0;
                    _timer.text = _timeCheck.ToString("N2");                // 소수점 두번째 까지 표현.       
                    _gameStateTxt.SetActive(true);
                    _gameStateTxt.GetComponent<Text>().text = "GameOver~";  // 현재 게임상태.
                    _curState = eGameState.END;
                }
                break;
            case eGameState.END:
                _gameReStartBtn.SetActive(true);
                _timeCheck += Time.deltaTime;

                if(_timeCheck >= 1.0f)
                {
                    _timeCheck = 0;
                    _gameStateTxt.GetComponent<Text>().text = "Score : " + ParticleLauncher._uniqueInstance.SUM.ToString("N1");
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
    }

    public void SettingPlayer()
    {
        //if(BaseGameManager.INSTANCE.CURSTAGE == BaseGameManager.eStageState.INGAME01)
        {
            _prefabPlayer.transform.position = _startPosition[0].transform.position;
            _prefabPlayer.transform.rotation = _startPosition[0].transform.rotation;
        }
        //else if (BaseGameManager.INSTANCE.CURSTAGE == BaseGameManager.eStageState.INGAME02)
        //{
        //    _prefabPlayer.transform.position = _startPosition[1].transform.position;
        //    _prefabPlayer.transform.rotation = _startPosition[1].transform.rotation;
        //}
        //else if (BaseGameManager.INSTANCE.CURSTAGE == BaseGameManager.eStageState.INGAME03)
        //{
        //    _prefabPlayer.transform.position = _startPosition[2].transform.position;
        //    _prefabPlayer.transform.rotation = _startPosition[2].transform.rotation;
        //}
    }

    // Game관련 버튼.
    public void StartBtn()
    {
        //Debug.Log("Start Button Working");
        _touchShootUI.SetActive(true);
        _gameStartBtn.SetActive(false);
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.TOILET_SOUND);
        //BaseSceneManager.INSTANCE.SceneMoveAtLobby(_curStageIdx);

        // 소변기 물내려가는 이펙트 및 소리
        Transform tf = GameObject.FindGameObjectWithTag("ToiletWaterFall").transform;
        GameObject go = Instantiate(_toiletWaterFall, tf.position, tf.rotation);
        Destroy(go, 7);

        GameStartBtn._uniqueInstance.CLICKBTN = true;
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

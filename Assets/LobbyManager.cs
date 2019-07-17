using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        RESULT
    }

    public static LobbyManager _uniqueInstance;
    public GameObject[] _startPosition;

    [SerializeField] GameObject _prefabPlayer;

    PlayerControl _player;
    eGameState _curState;

    float _timeCheck;
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
        SoundManager.INSTANCE.PlayBGMSound(SoundManager.eBGMType.LOBBY_GAME01);

        SettingPlayer();
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
            case eGameState.PLYRUNNING:

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
        // 카메라 워킹 위치 설정.
        Transform tf = GameObject.FindGameObjectWithTag("CameraPosRoot").transform;
        Camera.main.GetComponent<ActionCamera>().SetCameraActionRoot(tf);
        // 스폰 포인트 활성화.
        _isSpawn = true;     
    }

    public void SettingPlayer()
    {
        if(BaseGameManager.INSTANCE.CURSTAGE == BaseGameManager.eStageState.INGAME01)
        {
            _prefabPlayer.transform.position = _startPosition[0].transform.position;
            _prefabPlayer.transform.rotation = _startPosition[0].transform.rotation;
        }
        else if (BaseGameManager.INSTANCE.CURSTAGE == BaseGameManager.eStageState.INGAME02)
        {
            _prefabPlayer.transform.position = _startPosition[1].transform.position;
            _prefabPlayer.transform.rotation = _startPosition[1].transform.rotation;
        }
        else if (BaseGameManager.INSTANCE.CURSTAGE == BaseGameManager.eStageState.INGAME03)
        {
            _prefabPlayer.transform.position = _startPosition[2].transform.position;
            _prefabPlayer.transform.rotation = _startPosition[2].transform.rotation;
        }
    }

    // Game관련 버튼.
    public void StartBtn()
    {
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        //BaseSceneManager.INSTANCE.SceneMoveAtLobby(_curStageIdx);
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

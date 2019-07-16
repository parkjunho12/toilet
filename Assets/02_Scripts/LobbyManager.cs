using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public enum eGameState
    {
        READY   = 0,
        MAPSETTING,
        START,
        PLAY,
        END,
        RESULT
    }

    public static LobbyManager _uniqueInstance;

    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] Text _GametxtInfo;
    [SerializeField] GameObject _startPlyPosition;

    eGameState _curState;

    float _timeCheck;
    bool _endGame;

    public static LobbyManager INSTANCE
    {
        get { return _uniqueInstance; }
    }
    public eGameState NOWGAMESTATE
    {
        get { return _curState; }
    }
    public bool ENDGAME
    {
        get { return _endGame; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;

        SoundManager.INSTANCE.PlayBGMSound(SoundManager.eBGMType.LOBBY_GAME01);
    }

    // Update is called once per frame
    void Update()
    {
        if (_endGame)
            return;

        switch(_curState)
        {
            case eGameState.READY:
                GameReady();
                GameMapStting();
                break;
            case eGameState.START:
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 1.2f)
                {
                    Play();
                }
                break;
            case eGameState.END:

                break;
            case eGameState.RESULT:

                break;
        }
    }

    public void GameReady()
    {
        _curState = eGameState.READY;
        _GametxtInfo.text = "READY";
    }

    public void GameMapStting()
    {
        _curState = eGameState.MAPSETTING;
        // 플레이어 생성.
        SettingPlayer();

        GameObject go = Instantiate(_prefabPlayer, _startPlyPosition.transform.position, _startPlyPosition.transform.rotation);
        // 카메라 워킹 위치 설정
        Transform tf = GameObject.FindGameObjectWithTag("CameraPosRoot").transform;
        Camera.main.GetComponent<ActionCamera>().SetCameraActionRoot(tf);

    }

    public void SettingPlayer()
    {
        _prefabPlayer.transform.position = _startPlyPosition.transform.position;
    }

    public void StartGame()
    {
        _curState = eGameState.START;
        _GametxtInfo.text = "GAME START!!";
        _timeCheck = 0;
    }

    public void Play()
    {
        _curState = eGameState.PLAY;
        _GametxtInfo.enabled = false;
    }

    public void EndGame()
    {
        _timeCheck = 0;
    }

    public void StartBtn()
    {
        SoundManager.INSTANCE.PlayEffSound(SoundManager.eEffType.BTN);
        // 두걸음 앞으로
        // 카메라 정적으로 변경.
    }
}

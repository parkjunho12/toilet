using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseGameManager : MonoBehaviour
{
    public enum eLoadingState
    {
        NONE    = 0,
        START,
        UNLOADING,
        LOADING,
        END
    }

    public enum eStageState
    {
        NONE    = 0,
        LOBBY,
        INGAME01,
        INGAME02,
        INGAME03,
    }

    public static BaseGameManager _uniqueinstance;

    GameObject _GameToLobbyWnd;         // 게임 끝난 후 로비 진입 오브젝트.

    eLoadingState _curStateLoading;
    eStageState _curStage;

    public static BaseGameManager INSTANCE
    {
        get { return _uniqueinstance; }
    }
    public eLoadingState LOADSTAGE
    {
        get { return _curStateLoading; }
    }
    public eStageState CURSTAGE
    {
        get { return _curStage; }
        set { _curStage = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uniqueinstance = this;
        SceneManager.LoadSceneAsync("LobbySceneManager", LoadSceneMode.Additive);
    }
    

}

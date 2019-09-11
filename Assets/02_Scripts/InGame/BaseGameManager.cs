using System;
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
        INGAME,
    }

    public static BaseGameManager _uniqueinstance;

    GameObject _GameToLobbyWnd;         // 게임 끝난 후 로비 진입 오브젝트.

    eLoadingState _curStateLoading;
    eStageState _curStage;  
    
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
        SoundManager.INSTANCE.PlayBGMSound(SoundManager.eBGMType.LOBBY);
        _curStage = eStageState.LOBBY;
    }

    /// <summary>
    /// 게임 끝난 후 물내리는 버튼을 누를 때 발생.
    /// </summary>
    public void SceneRestart(eStageState stage)
    {
        _curStage = stage;

        string[] unloadStage = new string[1];
        unloadStage[0] = _curStage.ToString();

        string[] loadStage = new string[1];
        loadStage[0] = stage.ToString();

        SceneManager.UnloadSceneAsync(unloadStage[0].ToString());
        SceneManager.LoadSceneAsync(loadStage[0].ToString(), LoadSceneMode.Additive);
        //StartCoroutine(LoadingScene(loadStage, unloadStage));
    }

    IEnumerator LoaddingScene(string[] loadName = null, string[] unloadName = null)
    {
        AsyncOperation AO;

        int amount;
        if (unloadName == null)
        {
            amount = 0;
        }
        else
        {
            amount = unloadName.Length;
        }
        _curStateLoading = eLoadingState.START;
        // Unload 실행.
        _curStateLoading = eLoadingState.UNLOADING;
        for (int i = 0; i < amount; i++)
        {
            AO = SceneManager.UnloadSceneAsync(unloadName[i]);
            while (!AO.isDone)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }

        _curStateLoading = eLoadingState.LOADING;
        // Load 실행.
        if (loadName == null)
        {
            amount = 0;
        }
        else
        {
            amount = loadName.Length;
        }
        for (int i = 0; i < amount; i++)
        {
            AO = SceneManager.LoadSceneAsync(loadName[i], LoadSceneMode.Additive);
            while (!AO.isDone)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadName[amount - 1]));

        // BGM 사운드
        if(_curStage == eStageState.LOBBY)
        {
            SoundManager._uniqueinstance.PlayBGMSound(SoundManager.eBGMType.LOBBY, SoundManager._uniqueinstance.BGM.volume);
        }
        else if (_curStage == eStageState.INGAME)
        {
            SoundManager._uniqueinstance.PlayBGMSound(SoundManager.eBGMType.INGAME, SoundManager._uniqueinstance.BGM.volume);
        }

        _curStateLoading = eLoadingState.END;
    }

    public void SceneMoveAtLobby(eStageState stage)
    {
        _curStage = stage;

        string[] unloadStage = new string[1];
        unloadStage[0] = "LobbySceneManager";

        string[] loadStage = new string[1];
        loadStage[0] = "InGameSceneManager";

        StartCoroutine(LoaddingScene(loadStage, unloadStage));
    }

    public void SceneMoveAtStage(eStageState stage)
    {
        _curStage = stage;

        string[] unloadStage = new string[1];
        string[] loadStage = new string[1];
        if(stage == eStageState.LOBBY)
        {
            unloadStage[0] = "InGameSceneManager";
            loadStage[0] = "LobbySceneManager";
        }
        else
        {
            Debug.Log("error");
        }

        StartCoroutine(LoaddingScene(loadStage, unloadStage));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger _uniqueInstance;
    public Animator animator;
    public GameObject _fadeImage;
    int levelToLoad;

    BaseGameManager.eStageState _curStageIdx;

    void Awake()
    {
        _uniqueInstance = this;    
    }

    public GameObject IMAGE
    {
        get { return _fadeImage; }
        set { _fadeImage = value; }
    }

    public void StartScene()
    {
        _fadeImage.SetActive(false);
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeCompleteLobby()
    {
        int stageIdx = 2;
        if ((int)_curStageIdx != stageIdx)
            _curStageIdx = (BaseGameManager.eStageState)stageIdx;
        else
            _curStageIdx = 0;

        BaseGameManager._uniqueinstance.SceneMoveAtLobby(_curStageIdx);
    }
    public void OnFadeCompleteInGame()
    {
        BaseGameManager._uniqueinstance.SceneMoveAtStage(BaseGameManager.eStageState.LOBBY);
    }
}

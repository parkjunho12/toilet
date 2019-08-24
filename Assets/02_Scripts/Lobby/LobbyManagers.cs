using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManagers : MonoBehaviour
{
    BaseGameManager.eStageState _curStageIdx;

    public void StartBtn()
    {
        int stageIdx = 2;
        if ((int)_curStageIdx != stageIdx)
            _curStageIdx = (BaseGameManager.eStageState)stageIdx;
        else
            _curStageIdx = 0;


        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        BaseGameManager._uniqueinstance.SceneMoveAtLobby(_curStageIdx);
    }

    public void QuitBtn()
    {
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        Application.Quit();
    }
}

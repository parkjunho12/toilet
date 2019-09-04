using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManagers : MonoBehaviour
{
    public void StartBtn()
    {
        SceneChanger._uniqueInstance.IMAGE.SetActive(true);

        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        SceneChanger._uniqueInstance.FadeToLevel(1);
    }

    public void QuitBtn()
    {
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
        #else
                Application.Quit();
        #endif
    }
}

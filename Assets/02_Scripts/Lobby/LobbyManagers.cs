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
    private void Update()
    {
       
    }
    public void QuitBtn()
    {
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        Application.Quit();
    }
}

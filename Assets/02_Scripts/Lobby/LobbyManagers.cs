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
        if (Input.GetMouseButtonDown(0))
        {
            StartBtn();
        }
    }
    public void QuitBtn()
    {
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        Application.Quit();
    }
}

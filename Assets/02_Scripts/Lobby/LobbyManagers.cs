using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManagers : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _optionMenu;
    [SerializeField] GameObject _volumeGraphicMenu;
    [SerializeField] GameObject _shopMenu;

    public void StartBtn()
    {
        SceneChanger._uniqueInstance.IMAGE.SetActive(true);

        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        SceneChanger._uniqueInstance.FadeToLevel(1);
    }

    public void OptionBtn()
    {// 메인메뉴 => 옵션.
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        _mainMenu.SetActive(false);
        _optionMenu.SetActive(true);
        _volumeGraphicMenu.SetActive(true);
        _shopMenu.SetActive(false);
    }
    public void Volume_GrphicBTN()
    {// 음향 및 그래픽 조절
        _volumeGraphicMenu.SetActive(true);
        _shopMenu.SetActive(false);
    }
    public void ShopBtn()
    {// 상점 메뉴
        _shopMenu.SetActive(true);
        _volumeGraphicMenu.SetActive(false);
    }
    public void BackToMainMenu()
    {// 옵션 => 메인메뉴
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        _optionMenu.SetActive(false);
        _mainMenu.SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManagers : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _optionMenu;
    [SerializeField] GameObject _volumeGraphicMenu;
    [SerializeField] GameObject _shopMenu;

    public Light _gold;

    public void StartBtn()
    {// 게임시작 버튼.
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
    {// 옵션 => 음향 및 그래픽 조절
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        _volumeGraphicMenu.SetActive(true);
        _shopMenu.SetActive(false);
    }
    public void ShopBtn()
    {// 옵션 => 상점 메뉴
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        _shopMenu.SetActive(true);
        _volumeGraphicMenu.SetActive(false);
        StartCoroutine(LightGold(2.0f));
    }
   

    public void BackToMainMenu()
    {// 옵션 => 메인메뉴
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        _optionMenu.SetActive(false);
        _volumeGraphicMenu.SetActive(false);
        _shopMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void QuitBtn()
    {// 게임 나가기.
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
        #else
                Application.Quit();
        #endif
    }

    IEnumerator LightGold(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        _gold.intensity = Random.Range(6, 15);
        StartCoroutine(LightGold(Random.Range(0.2f, 0.5f)));
    }
}

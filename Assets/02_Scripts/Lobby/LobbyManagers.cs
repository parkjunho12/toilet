using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyManagers : MonoBehaviour
{
    string cAddress = "http://dbwo4011.cafe24.com/unity/select.php";
    public Text Text;
    void Awake()
    {
        StartCoroutine(this.Call(cAddress));
        Text.GetComponent<Text>().text ="edd";

    }

    public void StartBtn()
    {

        SceneChanger._uniqueInstance.IMAGE.SetActive(true);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        SceneChanger._uniqueInstance.FadeToLevel(1);

    }
    public IEnumerator Call(string _address)
    {
        WWWForm cForm = new WWWForm();
        WWW wwwUrl = new WWW(_address);
        yield return wwwUrl;
        Text.GetComponent<Text>().text = wwwUrl.text;
        Debug.Log(wwwUrl.text);
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

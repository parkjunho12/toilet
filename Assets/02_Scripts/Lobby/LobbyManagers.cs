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
    public Text _HaveArrow;
    public Text _HaveShield;
    public Text _HavePet_Dog;
    public Light _gold;
    public Text _myGold;
    public Text _buyState;
    public Text _content;


    bool _arrowBought;
    bool _petDogBought;
    int _myProperty;
    int _shield;

    string cAddress = "http://dbwo4011.cafe24.com/unity/select.php";
    string cAddress2 = "http://dbwo4011.cafe24.com/unity/BuyArrow.php";
    string cAddress3 = "http://dbwo4011.cafe24.com/unity/BuyPet.php";
    public Text GOLD
    {
        get { return _myGold; }
        set { _myGold = value; }
    }
    
    void Start()
    {
        StartCoroutine(this.Call(cAddress));     // 강아지펫꺼 추가함.
       
        _content.GetComponent<Text>().text = "edd";
        _buyState.gameObject.SetActive(false);
    }

    public void StartBtn()
    {// 게임시작 버튼.
        SceneChanger._uniqueInstance.IMAGE.SetActive(true);

        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        SceneChanger._uniqueInstance.FadeToLevel(1);
    }
    public IEnumerator Call(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        _content.GetComponent<Text>().text = wwwUrl.text;
    }
    public IEnumerator BuyArrow(string _address2)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address2, cForm);

        yield return wwwUrl;
        _myProperty = int.Parse(wwwUrl.text);
        _myGold.text = _myProperty.ToString();
        Debug.Log(wwwUrl.text);
    }
    public IEnumerator BuyShield(string _address2)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        cForm.AddField("Shield",int.Parse (_HaveShield.text));
        WWW wwwUrl = new WWW(_address2, cForm);
        yield return wwwUrl;
        _shield = int.Parse(wwwUrl.text);
        _myGold.text = _shield.ToString();
        Debug.Log(_myProperty);
    }
    public IEnumerator BuyPet(string _address2)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address2, cForm);

        yield return wwwUrl;
        _myProperty = int.Parse(wwwUrl.text);
        _myGold.text = _myProperty.ToString();
        Debug.Log(wwwUrl.text);
    }
    public IEnumerator FindGold(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        _myProperty = int.Parse(wwwUrl.text);
        _myGold.text = _myProperty.ToString();
        //Debug.Log(wwwUrl.text.ToString());
    }
    public IEnumerator FindArrow(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        if (wwwUrl.text.Equals("1"))
        {
            _arrowBought = true;
            _HaveArrow.GetComponent<Text>().text = "Have";
        }
        Debug.Log(wwwUrl.text);
    }
    public IEnumerator FindShield(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        _HaveShield.GetComponent<Text>().text = wwwUrl.text.ToString();
        Debug.Log(wwwUrl.text);
    }
    public IEnumerator FindPet_Dog(string _address)
    {// 강아지펫꺼 추가함.
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", SystemInfo.deviceUniqueIdentifier);
        WWW wwwUrl = new WWW(_address, cForm);
        yield return wwwUrl;
        if (wwwUrl.text.Equals("1"))
        {
            _petDogBought = true;
            _HavePet_Dog.GetComponent<Text>().text = "Have";
        }
        Debug.Log(wwwUrl.text);
    }

    public void OptionBtn()
    {// 메인메뉴 => 옵션.
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BTN);
        _mainMenu.SetActive(false);
        _optionMenu.SetActive(true);
        _volumeGraphicMenu.SetActive(true);
        _shopMenu.SetActive(false);
        StartCoroutine(this.FindGold("http://dbwo4011.cafe24.com/unity/FindGold.php"));
        StartCoroutine(this.FindArrow("http://dbwo4011.cafe24.com/unity/FindArrow.php"));
        StartCoroutine(this.FindShield("http://dbwo4011.cafe24.com/unity/FindShield.php"));
        StartCoroutine(this.FindPet_Dog("http://dbwo4011.cafe24.com/unity/FindPet.php"));
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
    public void BuyArrow()
    {// 네비 화살표 아이템. 가격 임의
        if (_HaveArrow.GetComponent<Text>().text.Equals("Have"))
        {
            _buyState.gameObject.SetActive(true);
            _buyState.text = "U already have";
            StartCoroutine(TextOff(2.0f));
            return;
        }
        else
        {
            if (int.Parse(_myGold.text) >= 1000)
            {// 살 수 있다.
                StartCoroutine(this.BuyArrow(cAddress2));
                StartCoroutine(this.FindArrow("http://dbwo4011.cafe24.com/unity/FindArrow.php"));
                _HaveArrow.GetComponent<Text>().text = "Have";
                SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHOP_BUY);
                _arrowBought = true;
                _buyState.gameObject.SetActive(true);
                _buyState.text = "(Arrow) Success!";
                StartCoroutine(TextOff(1.5f));
            }
            else
            {// 못 산다.
                _buyState.gameObject.SetActive(true);
                _buyState.text = "Not enough Gold..";
                StartCoroutine(TextOff(1.5f));
            }
        }
    }
    public void BuyShield()
    {// 차를 한 번 막아줄 수 있는 아이템. 가격 임의
        Debug.Log(int.Parse(_myGold.text));
        if (int.Parse(_myGold.text) >= 1000)
        {// 살 수 있다.
            _HaveShield.GetComponent<Text>().text = (int.Parse(_HaveShield.GetComponent<Text>().text) + 1).ToString();
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHOP_BUY);
            StartCoroutine(this.BuyShield("http://dbwo4011.cafe24.com/unity/BuyShield.php"));
            //StartCoroutine(this.FindShield("http://dbwo4011.cafe24.com/unity/FindShield.php"));
            _buyState.gameObject.SetActive(true);
            _buyState.text = "(Shield) Success!";
            StartCoroutine(TextOff(1.5f));
        }
        else
        {// 못 산다.
            _buyState.gameObject.SetActive(true);
            _buyState.text = "Not enough Gold..";
            StartCoroutine(TextOff(1.5f));
        }
    }
    public void BuyPet_Dog()
    {// 강아지펫꺼 추가함.
        if (_HavePet_Dog.GetComponent<Text>().text.Equals("Have"))
        {
            _buyState.gameObject.SetActive(true);
            _buyState.text = "U already have";
            StartCoroutine(TextOff(2.0f));
            return;
        }
        else
        {
            if (int.Parse(_myGold.text) >= 1000)
            {// 살 수 있다.
                StartCoroutine(this.BuyPet(cAddress3));
                StartCoroutine(this.FindPet_Dog("http://dbwo4011.cafe24.com/unity/FindPet.php"));
                SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHOP_BUY);
                _petDogBought = true;
                _buyState.gameObject.SetActive(true);
                _buyState.text = "(Pet.Dog) Success!";
                StartCoroutine(TextOff(1.5f));
            }
            else
            {// 못 산다.
                _buyState.gameObject.SetActive(true);
                _buyState.text = "Not enough Gold..";
                StartCoroutine(TextOff(1.5f));
            }
        }
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

    IEnumerator TextOff(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        _buyState.gameObject.SetActive(false);
    }
}

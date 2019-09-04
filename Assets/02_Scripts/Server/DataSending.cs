using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSending : MonoBehaviour
{
    string cAddress = "http://dbwo4011.cafe24.com/unity/Check.php";
    public static DataSending _uniqueinstance;
    [SerializeField] GameObject[] _gameStateTxt;
    int _rndNum;


    // Start is called before the first frame update
    void Awake()
    {
        _uniqueinstance = this;
        StartCoroutine(this.Call(cAddress));
        _rndNum = PlayerControl._uniqueInstance.RNDNUM;
        _gameStateTxt[_rndNum].GetComponent<Text>();
    }

   
    public IEnumerator Call(string _address)
    {
        WWWForm cForm = new WWWForm();
        cForm.AddField("id", _gameStateTxt[_rndNum].GetComponent<Text>().text);
        WWW wwwUrl = new WWW(_address ,cForm);
        yield return wwwUrl;
        Debug.Log(wwwUrl.text);
    }
    

}

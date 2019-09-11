using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Dictionary<string, GetItem> GetItems = new Dictionary<string, GetItem>();
    static ItemManager _uniqueInstance;

    public Text _buyState; 

    int _myProperty;
    bool _arrowBought;


    public static ItemManager Instance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = GameObject.FindObjectOfType<ItemManager>();
            }
            return ItemManager._uniqueInstance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        CreateMyItemState("Arrow", 1000, false);
    }

    public void CreateMyItemState(string _itemName, int _itemPrice, bool _boughtState)
    {
        GetItem newItem = new GetItem(_itemName, _itemPrice, _boughtState);
        GetItems.Add(_itemName, newItem);
    }

    public void BuyArrow(string itemName)
    {// 네비 화살표 아이템. 가격 임의

        if (GetItems[itemName].MyItem())
        {
            _buyState.gameObject.SetActive(true);
            _buyState.text = "Buy Success!";

            //int tmpMoney = PlayerPrefs.GetInt("Money");
            // PlayerPrefs.SetInt("Money", tmpMoney -= GetItems[itemName].ItemPrice);
            StartCoroutine(TextOff(1.5f));
        }
        else
        {
            _buyState.gameObject.SetActive(true);
            _buyState.text = "Already Buy~";
            StartCoroutine(TextOff(1.5f));
        }
    }
    //public void BuyShield()
    //{// 차를 한 번 막아줄 수 있는 아이템. 가격 임의
    //    if (_myProperty > 1000)
    //    {// 살 수 있다.
    //        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHOP_BUY);
    //        _myProperty -= 1000;
    //        StartCoroutine(TextOff(1.5f));
    //    }
    //    else
    //    {// 못 산다.
    //        StartCoroutine(TextOff(1.5f));
    //    }
    //}

    IEnumerator TextOff(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        _buyState.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    public Light _gold;
    public Text _myGold;
    public Text _buyState;

    string _itemName;
    int _itemPrice;
    bool _arrowBought;
    bool _unlocked;

    public int ItemPrice
    {

    }
    
    public GetItem(string _itemName, int _itemPrice, bool _arrowBought)
    {
        this._itemName = _itemName;
        this._itemPrice = _itemPrice;
        this._arrowBought = _arrowBought;
        this._unlocked = false;
        LoadItemState();
    }

    public bool MyItem()
    {
        if(!_unlocked)
        {
            SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SHOP_BUY);
            _arrowBought = true;
            SaveAchievment(true);
            return true;
        }
        return false;
    }

    public void SaveAchievment(bool value)
    {
        _unlocked = value;

        int tmpMoney = PlayerPrefs.GetInt("Money");

        PlayerPrefs.SetInt("Money", tmpMoney -= 1000);
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadItemState()
    {
        _unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

        if(_unlocked)
        {
            _arrowBought = true;
        }
    }
}

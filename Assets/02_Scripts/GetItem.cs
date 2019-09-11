using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    public static GetItem _uniqueInstance;

    string _itemName;
    int _itemPrice;
    bool _arrowBought;
    bool _unlocked;
  
    public string ItemName
    {
        get { return _itemName; }
        set { _itemName = value; }
    }
    public int ItemPrice
    {
        get { return _itemPrice; }
        set { _itemPrice = value; }
    }
    public bool ArrowBought
    {
        get { return _arrowBought; }
        set { _arrowBought = value; }
    }
    public bool Unlocked
    {
        get { return _unlocked; }
        set { _unlocked = value; }
    }

    void Start()
    {
        _uniqueInstance = this;  
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
            IsMyItemExist._uniqueInstance.ArrowExist = _arrowBought = true;
            SaveAchievment(true);
            return true;
        }
        return false;
    }

    public void SaveAchievment(bool value)
    {
        _unlocked = value;

        _arrowBought = true;
        PlayerPrefs.SetString("ArrowBought", _arrowBought.ToString());
        PlayerPrefs.SetInt(_itemName, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadItemState()
    {
        _unlocked = PlayerPrefs.GetInt(_itemName) == 1 ? true : false;

        string value = PlayerPrefs.GetString("ArrowBought", _arrowBought.ToString());
        _arrowBought = System.Convert.ToBoolean(value);
        if(_unlocked)
        {
            IsMyItemExist._uniqueInstance.ArrowExist = _arrowBought = true;           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMyItemExist : MonoBehaviour
{
    public static IsMyItemExist _uniqueInstance;

    bool _arrowExist;

    public bool ArrowExist
    {
        get { return _arrowExist; }
        set { _arrowExist = value; }
    }

    void Start()
    {
        _uniqueInstance = this;    
    }
}

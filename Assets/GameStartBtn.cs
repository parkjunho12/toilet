using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartBtn : MonoBehaviour
{
    public static GameStartBtn _uniqueInstance;

    public Renderer lamp;
    private Color originColor;

    bool _clickBtn;
    public bool CLICKBTN
    {
        get { return _clickBtn; }
        set { _clickBtn = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uniqueInstance = this;

        originColor = lamp.material.color;
        _clickBtn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (_clickBtn)
        //{
        //    lamp.material.color = Color.gray;
        //    return;
        //}

        float flicker = Mathf.Abs(Mathf.Sin(Time.time * 10));
        lamp.material.color = originColor * flicker;
    }
}

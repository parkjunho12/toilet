﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineControl : MonoBehaviour
{
    public Transform tf;
    public Transform line;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        line.rotation = tf.rotation;
    }
}

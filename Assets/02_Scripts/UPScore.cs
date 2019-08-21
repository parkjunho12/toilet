using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPScore : MonoBehaviour
{
    private float deadtime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = transform.position;
        pos.y += 0.001f;
        transform.position = pos;

        deadtime += Time.deltaTime;

        if (deadtime >= 1.5f)
        {
            pos = new Vector3(0, 0, 0);
            transform.position = pos;
            deadtime = 0;

        }

    }
}

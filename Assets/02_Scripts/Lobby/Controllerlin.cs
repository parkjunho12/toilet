using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllerlin : MonoBehaviour
{
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward,out hit))
        {
            if(hit.collider != null){
                if(go != hit.collider.gameObject)
                {
                    go = hit.transform.gameObject;
                
                }
                else
                {
                    if(go != null)
                    {
                        go.SendMessage("");
                    }
                }
            }
        } 
    }
}

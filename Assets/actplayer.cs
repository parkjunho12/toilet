using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actplayer : MonoBehaviour
{
    Transform tf;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKey("a") && i ==0)
        {
            i = 1;
            //SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.SCREAM);
            PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.RUN);
            tf.Translate(Vector3.forward * 5f * Time.deltaTime);
        }
    }
}

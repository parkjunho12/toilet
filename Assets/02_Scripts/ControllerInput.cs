using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public GameObject _prefabPlayer;
    public int speed= 12;
    public int speedSide = 6;
    private Transform tr;
    private float dirX = 0;
    private float dirZ = 0;
    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");        //왼쪽, 오른쪽 키 
        float ver = Input.GetAxis("Vertical");          //앞, 뒤 키

        transform.Translate(Vector3.forward * speed * ver * Time.deltaTime);      //이동
        transform.Rotate(Vector3.up * speed * hor * Time.deltaTime);
        _prefabPlayer.transform.eulerAngles = transform.localEulerAngles;
        // 회전
    }


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
       
    }
   
}

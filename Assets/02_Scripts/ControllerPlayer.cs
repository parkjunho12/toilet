using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour
{
    public Vector2 joystick;
    public float speed;
    public GameObject centerEye;
    public GameObject project;
    private float dirX = 0;
    private float dirZ = 0;
    private Transform tr;
     //속도

    //void FixedUpdate()
    //{
    //    PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.RUN);

    //    float hor = Input.GetAxis("Horizontal");        //왼쪽, 오른쪽 키 
    //    float ver = Input.GetAxis("Vertical");          //앞, 뒤 키

    //    transform.Translate(Vector3.forward * speed * ver * Time.deltaTime);      //이동
    //    transform.Rotate(Vector3.up * speed * hor * Time.deltaTime);
    //    //transform.eulerAngles = new Vector3(0,  speed * hor, 0);// 회전
    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
       

        MovePlayer();
      

    }
  
    void MovePlayer()
    {
  

        dirX = 0;
        dirZ = 0;
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.RUN);
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);
            transform.eulerAngles = new Vector3(0, centerEye.transform.localEulerAngles.y, 0);
            var absX = Mathf.Abs(coord.x);
            var absY = Mathf.Abs(coord.y);
            if (absX > absY)
            {
                if (coord.x > 0)
                {
                    dirX = +1;

                }
                else
                {
                    dirX = -1;
                }
            }
            else
            {
                if (coord.y > 0)
                {
                    dirZ = +1;

                }
                else
                {
                    dirZ = -1;
                }

            }

            if (LobbyManager._uniqueInstance.PLAYCOUNT > 80)
            {
                Vector3 moveDir = new Vector3(dirX * 8.5f, 0, dirZ * 8.5f);
                transform.Translate(moveDir * Time.smoothDeltaTime);
            }
            else if (LobbyManager._uniqueInstance.PLAYCOUNT <= 80
                && LobbyManager._uniqueInstance.PLAYCOUNT > 30)
            {
                Vector3 moveDir = new Vector3(dirX * 6.5f, 0, dirZ * 6.5f);
                transform.Translate(moveDir * Time.smoothDeltaTime);
            }
            else
            {
                Vector3 moveDir = new Vector3(dirX * 5.5f, 0, dirZ * 5.5f);
                transform.Translate(moveDir * Time.smoothDeltaTime);
            }
        }
        else
        {// 화면 터치가 안됬을 시 캐릭터 IDLE..
            PlayerControl._uniqueInstance.ChangedAction(PlayerControl.ePlayerActState.IDLE);
        }




    }
    }

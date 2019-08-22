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
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
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

            Vector3 moveDir = new Vector3(dirX * speed, 0, dirZ * speed);
            transform.Translate(moveDir * Time.smoothDeltaTime);
        }


      

    }
    }

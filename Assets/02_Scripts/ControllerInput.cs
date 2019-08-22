using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public GameObject _prefabPlayer;
    public int speedForward = 12;
    public int speedSide = 6;
    private Transform tr;
    private float dirX = 0;
    private float dirZ = 0;

    public enum MoveType
    {
            WAY_POINT,
            LOOK_AT,
            DAYDREAM
    }
    public MoveType movetype = MoveType.WAY_POINT;
    private CharacterController cc;
    private Transform[] points;
    private int nextIdx = 1;
    private Transform camTr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        camTr = Camera.main.GetComponent<Transform>();
    
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
       
    }
    void MovePlayer(){
        dirX = 0;
        dirZ = 0;

       // transform.eulerAngles = new Vector3(0, centerEye.transform.localEulerAngles.y, 0);
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);
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

            Vector3 moveDir = new Vector3( dirX * speedSide, 0, dirZ * speedForward);
            _prefabPlayer.transform.Translate(moveDir * Time.smoothDeltaTime);
        }

       // transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public enum eStateCamera
    {
        NONE    = 0,
        WORKING,
        CHANGE_FOLLOW,
        FOLLOW
    }

    public static ActionCamera _uniqueInstance;

    [SerializeField] float _movSpeed;
    [SerializeField] float _rotSpeed;
    [SerializeField] Vector3 _followOffset = new Vector3(0, 1.8f, -2);

    Transform _tfRootPos;
    Transform _posPlayer;
    Transform _lookPos;
    List<Vector3> _ltPositions;
    Vector3 _posGoal;

    int _curIndex;
    int _nextIndex;
    float _timeCheck;

    eStateCamera _curCameraAction;
    
    public eStateCamera CAMERAACTION
    {
        get { return _curCameraAction; }
        set { _curCameraAction = value; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        _ltPositions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(_curCameraAction)
        {
            case eStateCamera.NONE:
                _timeCheck += Time.deltaTime;
                if(_timeCheck > 2)
                    _curCameraAction = eStateCamera.WORKING;

                break;
            case eStateCamera.WORKING:
                if(Vector3.Distance(transform.position, _ltPositions[_nextIndex]) <= 0.3f)
                {
                    _curIndex = _nextIndex;
                    _nextIndex = _curIndex + 1;
                    if(_nextIndex >= _ltPositions.Count)
                    {
                        _nextIndex = _curIndex;
                        _curCameraAction = eStateCamera.CHANGE_FOLLOW;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, _ltPositions[_nextIndex], Time.deltaTime * _movSpeed);
                transform.LookAt(_posPlayer);
                break;
            case eStateCamera.CHANGE_FOLLOW:
                Vector3 tp = _posPlayer.position;
                Quaternion tq = Quaternion.LookRotation(_lookPos.position - tp);
                transform.position = Vector3.Slerp(transform.position, tp, Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, tq, Time.deltaTime * _rotSpeed);
                if(Vector3.Distance(transform.position, tp) <= 0.2f)
                {
                    transform.position = tp;
                    transform.LookAt(_lookPos);
                    _curCameraAction = eStateCamera.FOLLOW;
                }
                break;
            case eStateCamera.FOLLOW:
                float currAngleY = Mathf.LerpAngle(transform.eulerAngles.y, _posPlayer.eulerAngles.y, Time.deltaTime);

                Quaternion rot = Quaternion.Euler(0, currAngleY, 0);
                _posGoal = _posPlayer.position
                    - (rot * Vector3.forward * _followOffset.z) + (Vector3.up * _followOffset.y);

                transform.position = Vector3.MoveTowards(transform.position, _posGoal, Time.deltaTime * 2 * _movSpeed);
                transform.LookAt(_lookPos);

                LobbyManager.INSTANCE.NOWGAMESTATE = LobbyManager.eGameState.PLYRUNNING;
                break;
        }
    }

    public void SetCameraActionRoot(Transform tf)
    {
        _curIndex = 0;
        _tfRootPos = tf;

        for(int n = 0; n < _tfRootPos.childCount; n++)
        {
            _ltPositions.Add(_tfRootPos.GetChild(n).position);
        }

        _posPlayer = GameObject.FindWithTag("PlayerEye").transform;
        _lookPos = GameObject.FindWithTag("LookPos").transform;

        transform.position = _ltPositions[_curIndex];
        transform.LookAt(_posPlayer);
        _nextIndex = _curIndex + 1;
        _curCameraAction = eStateCamera.NONE;
    }

}

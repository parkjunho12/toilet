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
        set { _curCameraAction = value; }
    }

    void Awake()
    {
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

                break;
            case eStateCamera.FOLLOW:

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

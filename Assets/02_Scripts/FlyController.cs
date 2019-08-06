using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    List<Vector3> _flyMovePos;
    float _timeCheck;
    float _movTimeCheck;

    // Update is called once per frame
    void Update()
    {
        if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY)
        {
            //_timeCheck += Time.deltaTime;
            //_movTimeCheck += Time.deltaTime;

            //if(_timeCheck < 5)
            //{
            //    if(_movTimeCheck > 2.5f)
            //    {
            //        _movTimeCheck = 0;
            //        transform.position = _flyMovePos[UnityEngine.Random.Range(0, _flyMovePos.Count)];
            //    }
            //}
            //else if (_timeCheck >= 5 && _timeCheck < 20)
            //{
            //    if (_movTimeCheck > 1.8f)
            //    {
            //        _movTimeCheck = 0;
            //        transform.position = _flyMovePos[UnityEngine.Random.Range(0, _flyMovePos.Count)];
            //    }
            //}
            //else if (_timeCheck >= 20 && _timeCheck < 35)
            //{
            //    if (_movTimeCheck > 1.0f)
            //    {
            //        _movTimeCheck = 0;
            //        transform.position = _flyMovePos[UnityEngine.Random.Range(0, _flyMovePos.Count)];
            //    }
            //}
            //else if (_timeCheck >= 35 && _timeCheck < 50)
            //{
            //    if (_movTimeCheck > 0.5f)
            //    {
            //        _movTimeCheck = 0;
            //        transform.position = _flyMovePos[UnityEngine.Random.Range(0, _flyMovePos.Count)];
            //    }
            //}
        }
    }

    public void SettingFlyMovePathRoamming(Transform[] points = null)
    {
        _flyMovePos = new List<Vector3>();
        for (int n = 0; n < points.Length; n++)
        {
            _flyMovePos.Add(points[n].position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float _movSpeed;
    [SerializeField] float _rotSpeed;

    Transform _tfRootPos;
    Transform _lookPos;

    List<Vector3> _ltPositions;

    int _curIndex;
    int _nextIndex;
    float _timeCheck;

    void Awake()
    {
        _ltPositions = new List<Vector3>();   
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextIndex >= _ltPositions.Count)
            return;

        if (Vector3.Distance(transform.position, _ltPositions[_nextIndex]) <= 0.3f)
        {
            _curIndex = _nextIndex;
            _nextIndex = _curIndex + 1;
        }
        transform.position = Vector3.MoveTowards(transform.position, _ltPositions[_nextIndex], Time.deltaTime * _movSpeed);
        transform.LookAt(_tfRootPos);
    }

    public void SetRunningRoot(Transform tf)
    {

    }
}

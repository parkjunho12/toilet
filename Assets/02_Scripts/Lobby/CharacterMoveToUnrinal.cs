using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveToUnrinal : MonoBehaviour
{
    public enum ePlayerAction
    {
        RUN,
        IDEL,
    }

    [SerializeField] GameObject _stopPoint;
    [SerializeField] AudioClip[] _soundClip;

    Animator _aniCtrl;
    NavMeshAgent _naviMesh;
    ePlayerAction _curState;

    float _timeCheck;

    // Start is called before the first frame update
    void Start()
    {
        _aniCtrl = GetComponent<Animator>();
        _naviMesh = GetComponent<NavMeshAgent>();

        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.RUNNING_BREATH);
    }

    // Update is called once per frame
    void Update()
    {
        switch(_curState)
        {
            case ePlayerAction.RUN:
                if (Vector3.Distance(transform.position, _stopPoint.transform.position) < 0.2f)
                {
                    ChangedAction(ePlayerAction.IDEL);
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.ZIPPERDOWN);
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    _naviMesh.SetDestination(_stopPoint.transform.position);
                }
                break;
            case ePlayerAction.IDEL:
                _timeCheck += Time.deltaTime;
                if (_timeCheck > 0.1f)
                {
                    _timeCheck = 0;
                    //AudioSource.PlayClipAtPoint(_soundClip[0], transform.position);
                    SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.PEE_SOUND);
                }
                break;
        }
    }

    public void ChangedAction(ePlayerAction state)
    {
        switch(state)
        {
            case ePlayerAction.RUN:
                _naviMesh.enabled = true;
                _naviMesh.speed = 6.5f;
                _naviMesh.stoppingDistance = 0;
                break;
            case ePlayerAction.IDEL:
                _naviMesh.enabled = false;
                break;
        }
        _aniCtrl.SetInteger("AniState", (int)state);
        _curState = state;
    }
}

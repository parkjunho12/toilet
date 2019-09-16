using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 18.5f;
    //public float jumpForce = 300;
    //public float timeBeforeNextJump = 1.2f;
    //private float canJump = 0f;
    NavMeshAgent _naviAgent;
    Animator anim;
    Rigidbody rb;
    GameObject _prefabPlayer;

    List<Vector3> _walkPoints;
    Vector3 _posTarget;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _naviAgent = GetComponent<NavMeshAgent>();

        _prefabPlayer = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Bark(Random.Range(3, 7)));
    }

    void Update()
    {
        if (LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
        {
            ControllPlayer();
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }
        //else if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.START)
        //{           
        //    DogWalkToToilet();
        //     //PlayerControl._uniqueInstance.ISACTING = true;        
        //}
        //else if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.PLAY)
        //{
        //    if (Vector3.Distance(transform.position, _walkPoints[PlayerControl._uniqueInstance.RNDNUM]) < 0.5f)
        //    {
        //        _shootPos.SetActive(true);
        //        //transform.rotation = Quaternion.Euler(0, 180, 0);
        //        anim.SetInteger("Walk", 0);
        //    }
        //    else
        //    {
        //        anim.SetInteger("Walk", 1);
        //        _posTarget = _walkPoints[PlayerControl._uniqueInstance.RNDNUM];
        //        _naviAgent.SetDestination(_posTarget);
        //    }
        //}
    }

    void ControllPlayer()
    {
        //float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //float moveVertical = Input.GetAxisRaw("Vertical");

        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if(Vector3.Distance(transform.position, _prefabPlayer.transform.position) < 2.0f)
        {
            transform.LookAt(_prefabPlayer.transform.position);
            anim.SetInteger("Walk", 0);
        }
        else
        {
            anim.SetInteger("Walk", 1);
            transform.position = Vector3.MoveTowards(transform.position, _prefabPlayer.transform.position, movementSpeed * Time.deltaTime);
            transform.LookAt(_prefabPlayer.transform.position);
        }

        //if (movement != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        //    anim.SetInteger("Walk", 1);
        //}
        //else
        //{
        //    anim.SetInteger("Walk", 0);
        //}

        //transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }

    //public void DogWalkToToilet()
    //{
    //    if (PlayerControl._uniqueInstance.ISACTING)
    //        return;

    //    if (PlayerControl._uniqueInstance.RNDNUM == _walkPoints.Count)
    //    {// 다 걸어왔으면 제자리 멈춤 && 발사가능
    //        anim.SetInteger("Walk", 0);
    //        return;
    //    }

    //    anim.SetInteger("Walk", 1);
    //    _posTarget = _walkPoints[PlayerControl._uniqueInstance.RNDNUM];
    //    _naviAgent.SetDestination(_posTarget);
    //    PlayerControl._uniqueInstance.ISACTING = true;
    //}

    //public void SettingWalkPathRoamming(Transform[] points = null)
    //{
    //    _walkPoints = new List<Vector3>();
    //    for (int n = 0; n < points.Length; n++)
    //    {
    //        _walkPoints.Add(points[n].position);
    //    }
    //    //Debug.Log("SettingWalkPathRoamming Success");
    //}

    IEnumerator Bark(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BARK);
        yield return new WaitForSeconds(0.7f);
        SoundManager._uniqueinstance.PlayEffSound(SoundManager.eEffType.BARK);
        StartCoroutine(Bark(Random.Range(3, 7)));
    }
}
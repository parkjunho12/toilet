using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float m_force = 0f;                 // 카메라 흔들림 세기를 결정지을 변수.
    [SerializeField] Vector3 m_offset = Vector3.zero;    // 카메라가 흔들릴 방향을 결정지을 벡터.

    Quaternion m_originRot;             // 카메라의 초기값을 저장할 쿼터니언 변수.

    // Start is called before the first frame update
    void Start()
    {
        m_originRot = transform.rotation;
    }

    void Update()
    {
        //if(LobbyManager._uniqueInstance.NOWGAMESTATE ==
        //    LobbyManager.eGameState.START)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(Reset());
        //}

        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ShakeCoroutine());
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            StopAllCoroutines();
            StartCoroutine(Reset()); 
        }

    }


    IEnumerator ShakeCoroutine()
    {
        Vector3 t_originEuler = transform.eulerAngles;  // 카메라의 오일러 초기값 지정.
        
        // 카메라의 오일러 초기값 지정.
        while(true)
        {
            float t_rotX = Random.Range(-m_originRot.x, m_offset.x);
            float t_rotY = Random.Range(-m_originRot.y, m_offset.y);
            float t_rotZ = Random.Range(-m_originRot.z, m_offset.z);

            // 흔들림 값 = 초기값 + 랜덤값
            Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
            // 흔들림 값을 쿼터니온으로 변환
            Quaternion t_rot = Quaternion.Euler(t_randomRot);

            while(Quaternion.Angle(transform.rotation.normalized, t_rot) > 0.1f)
            {// 목적값까지 움직일 때까지 반복.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t_rot, m_force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator Reset()
    {
        while(Quaternion.Angle(transform.rotation, m_originRot) > 0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_originRot, m_force * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Car"))
        {
            StartCoroutine(ShakeCoroutine());
        }
    }
}

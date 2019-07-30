using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPee : MonoBehaviour
{
    [SerializeField] GameObject pee;

    GameObject _peePos;
    GameObject go;

    void Awake()
    {
        _peePos = GameObject.FindWithTag("ShootPointer");
    }

    void Update()
    {
        if(FixedTouchField._uniqueInstance.PRESSED)
        {
            float _x = Input.GetAxis("Mouse X") * 2000 * Time.deltaTime;
            float _y = Input.GetAxis("Mouse Y") * 2000 * Time.deltaTime;
            go = Instantiate(pee, _peePos.transform.position, _peePos.transform.rotation);
            go.transform.Rotate(_x, _y, 0, Space.World);
            Destroy(go, 0.5f);
        }
    }

    void onCollisionEnter(Collision target)
    {
        if(target.collider.CompareTag("Toilet"))
        {
            Destroy(go.gameObject);
            Debug.Log("+1점");
        }

        if (target.collider.CompareTag("Fly"))
        {
            Destroy(go.gameObject);
            Debug.Log("+5점");
        }
    }
}

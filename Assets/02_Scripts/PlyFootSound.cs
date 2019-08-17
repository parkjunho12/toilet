using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyFootSound : MonoBehaviour
{
    public AudioClip footSound;

    int _footCount;

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            //_footCount++;
            AudioSource.PlayClipAtPoint(footSound, transform.position);
        }
    }
}

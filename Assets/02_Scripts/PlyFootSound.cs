using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyFootSound : MonoBehaviour
{
    public AudioClip footSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AudioSource.PlayClipAtPoint(footSound, transform.position);
        }
    }
}

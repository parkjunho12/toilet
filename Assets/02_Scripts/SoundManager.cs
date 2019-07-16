using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum eBGMType
    {
        LOBBY_GAME01  = 0,
    }

    public enum eEffType
    {
        BTN,
        RUNNING_BREATH,
        ZIPPERDOWN,
        PEE,
        ZIPPERUP,
    }

    public static SoundManager _uniqueinstance;

    [SerializeField] AudioClip[] _bgmClips;
    [SerializeField] AudioClip[] _effClips;

    AudioSource _bgmPlayer;
    List<AudioSource> _ltEffPlayer;

    public static SoundManager INSTANCE
    {
        get { return _uniqueinstance; }
    }

    void Awake()
    {
        _uniqueinstance = this;

        _bgmPlayer = GetComponent<AudioSource>();
        _ltEffPlayer = new List<AudioSource>();
    }

    void LateUpdate()
    {
        foreach(AudioSource item in _ltEffPlayer)
        {
            if(!item.isPlaying)
            {
                _ltEffPlayer.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    public void PlayBGMSound(eBGMType type, float vol = 0.7f, bool isloop = true)
    {
        _bgmPlayer.clip = _bgmClips[(int)type];
        _bgmPlayer.volume = vol;
        _bgmPlayer.loop = isloop;

        _bgmPlayer.Play();
    }

    public void PlayEffSound(eEffType type, float vol = 0.1f, bool isloop = false)
    {
        GameObject go = new GameObject("EffectSound");
        go.transform.SetParent(transform);
        AudioSource AS = go.AddComponent<AudioSource>();
        AS.clip = _effClips[(int)type];
        AS.volume = vol;
        AS.loop = isloop;

        AS.Play();

        _ltEffPlayer.Add(AS);
    }
    
}

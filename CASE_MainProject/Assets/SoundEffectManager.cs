using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField, Header("AudioSourceƒŠƒXƒg"), ReadOnly]
    AudioSource[] audioSourceList;

    int audioSourceNextCount = 0;

    private void Awake()
    {
        audioSourceList = new AudioSource[transform.childCount];

        for(int i  = 0; i < transform.childCount; i++)
        {
            audioSourceList[i] = transform.GetChild(i).GetComponent<AudioSource>();
        }
    }

    public void PlaySoundEffect(AudioClip _audioClip)
    {
        audioSourceList[audioSourceNextCount].PlayOneShot(_audioClip);
        audioSourceNextCount++;
        if(audioSourceNextCount == transform.childCount)
        {
            audioSourceNextCount = 0;
        }
    }
}

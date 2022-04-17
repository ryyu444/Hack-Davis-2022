using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    private AudioSource source;

    public AudioClip[] clips;

    private void Awake()
    {
        instance = this;
        source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayOneShot(int index)
    {
        source.PlayOneShot(clips[index]);
    }
}

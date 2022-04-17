using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetScuffedAssScript : MonoBehaviour
{
    ParticleSystem particles;
    AudioSource source;
    public AudioClip clip;
    public AudioClip clip2;
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
        StartCoroutine(yeet());
    }

    IEnumerator yeet()
    {
        yield return new WaitForSeconds(5.5f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(3.5f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(3.5f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(4f);
        particles.Play();
        source.PlayOneShot(clip);
        //
        yield return new WaitForSeconds(0.5f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(0.3f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(0.2f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(0.2f);
        particles.Play();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(1.5f);
        source.PlayOneShot(clip2);
    }
}

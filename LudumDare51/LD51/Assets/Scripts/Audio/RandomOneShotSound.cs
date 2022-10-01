using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOneShotSound : MonoBehaviour
{
    public AudioSource speaker;
    public List<AudioClip> sounds;

    private void Start()
    {
        speaker.pitch = Random.Range(0.90f, 1.10f);
        speaker.PlayOneShot(sounds[Random.Range(0, sounds.Count)], Random.Range(0.45f, 0.55f));
    }
}

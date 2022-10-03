using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource speaker;
    public List<AudioClip> sounds;

    public void PlayOneShot(int selectedSound, float volume)
    {
        speaker.pitch = Random.Range(0.95f, 1.05f);
        speaker.PlayOneShot(sounds[selectedSound], volume);
    }
}

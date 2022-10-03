using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior to select a sound effect randomly from a list, and then plays with a bit of randomness to pitch and volume.
/// </summary>
public class RandomOneShotSound : MonoBehaviour
{
    #region Variables
    [Header("Settings")]
    public float minVolume;
    public float maxVolume;

    [Header("Audio Clips")]
    public List<AudioClip> sounds;

    [Header("Components")]
    public AudioSource speaker;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        speaker.pitch = Random.Range(0.90f, 1.10f);
        speaker.PlayOneShot(sounds[Random.Range(0, sounds.Count)], Random.Range(minVolume, maxVolume));
    }
    #endregion
}

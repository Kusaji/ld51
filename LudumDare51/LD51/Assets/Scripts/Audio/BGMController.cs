using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public List<AudioClip> songs;
    public AudioSource speaker;

    // Start is called before the first frame update
    void Start()
    {
        StartPlayingRandomSong();
    }


    public void StartPlayingRandomSong()
    {
        int randomSelection = Random.Range(0, songs.Count);
        Debug.Log($"{randomSelection}");
        speaker.clip = songs[randomSelection];
        speaker.Play();
    }

    public IEnumerator CheckIfSongEndedRoutine()
    {
        while (gameObject)
        {
            if (!speaker.isPlaying)
            {
                StartPlayingRandomSong();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


}

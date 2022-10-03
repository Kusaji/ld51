using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;
    public List<AudioClip> songs;
    public AudioSource speaker;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            StartCoroutine(CheckIfSongEndedRoutine());
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }


    public void StartPlayingRandomSong()
    {
        int randomSelection = Random.Range(0, songs.Count);
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

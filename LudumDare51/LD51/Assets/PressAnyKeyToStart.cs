using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKeyToStart : MonoBehaviour
{
    float timeAtScreen = 0f;

    // Update is called once per frame
    void Update()
    {
        timeAtScreen += Time.deltaTime;

        if (timeAtScreen > 1f && Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }
}

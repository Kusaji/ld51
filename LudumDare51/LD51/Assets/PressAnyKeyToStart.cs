using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKeyToStart : MonoBehaviour
{
    float timeAtScreen = 0f;
    private int tutorialStage = 0;
    public Canvas[] canvases = new Canvas[0];
    public GameObject menuObject;
    // Update is called once per frame
    void Update()
    {
        timeAtScreen += Time.deltaTime;

        if (timeAtScreen > 1f && Input.anyKeyDown)
        {
            if (canvases.Length > tutorialStage)
            {
                for (int i = 0; i < canvases.Length; i++)
                    canvases[i].enabled = false;

                canvases[tutorialStage].enabled = true;

                timeAtScreen = 0.5f;
            }
            else
            {
                SceneManager.LoadScene(1);
            }
            tutorialStage++;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}

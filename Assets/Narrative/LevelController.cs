using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class LevelController : MonoBehaviour
{
    public GameObject[] instructions;
    public GameObject[] NPCs;
    public GameObject win;
    public GameObject notify;
    public int level;
    public bool inInitializing;
    public bool inNotify;
    public bool inWin;
    public int correct;
    public int[] answers;
    public Flowchart flowchart;

    public GameObject BWUI;
    public GameObject SSUI;
    public GameObject HammerUI;

    // Start is called before the first frame update
    void Start()
    {
        inInitializing = true;
        level = LevelCounter.counter;
        Time.timeScale = 0;
        instructions[level - 1].SetActive(true);
        NPCs[level - 1].SetActive(true);

        if (level == 2)
        {
            BWUI.SetActive(true);
        }
        else if (level == 3)
        {
            BWUI.SetActive(true);
            SSUI.SetActive(true);
        }
        else if (level == 4)
        {
            BWUI.SetActive(true);
            SSUI.SetActive(true);
            HammerUI.SetActive(true);
        }
        else if (level == 5)
        {
            BWUI.SetActive(true);
            SSUI.SetActive(true);
            HammerUI.SetActive(true);
        }
    }
    private void StartLevel()
    {
        Time.timeScale = 1;
        instructions[level-1].SetActive(false);
        NPCs[level-1].BroadcastMessage("StartLevel");
    }
    void Update()
    {
        if (inInitializing && Input.anyKeyDown)
        {
            inInitializing = false;
            StartLevel();
        }
        if (inNotify)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                inNotify = false;
                Time.timeScale = 1;
                Retry();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                inNotify = false;
                Time.timeScale = 1;
                NPCs[level - 1].SetActive(false);
                Skip();
            }
        }
        if (inWin)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Time.timeScale = 1;
                inWin = false;
                Skip();
            }
        }


    }

    public void Lose()
    {
        inNotify = true;
        Time.timeScale = 0;
        notify.SetActive(true);
    }

    public void Add()
    {
        correct++;
        if (correct == answers[level - 1])
        {
            inWin = true;
            Time.timeScale = 0;
            win.SetActive(true);
        }
    }

    public void Clear()
    {
        Scene currentScene = SceneManager.GetActiveScene();  // Get the current scene
        SceneManager.LoadScene(currentScene.name);  // Load it again
    }

    void Retry()
    {
        Clear();
    }

    void Skip()
    {
        
        if (LevelCounter.counter == 1)
        {
            flowchart.ExecuteBlock("MoveToChp2");
        }
        else if (LevelCounter.counter == 2)
        {
            flowchart.ExecuteBlock("MoveToChp3");
        }
        else if (LevelCounter.counter == 3)
        {
            flowchart.ExecuteBlock("MoveToChp4");
        }
        else if (LevelCounter.counter == 4)
        {
            flowchart.ExecuteBlock("MoveToChp5");
        }
        else if (LevelCounter.counter == 5)
        {
            flowchart.ExecuteBlock("MoveToChp6");
        }
        LevelCounter.counter++;
    }
}

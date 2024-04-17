using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public GameObject constantsGameObject;
    private ConstantsList constantsList;
    public List<GameObject> backToOrigin = new List<GameObject>();
    public List<Vector3> originalPosition = new List<Vector3>();
    public Transform generated;
    public GameObject notify;
    public GameObject win;
    public bool inNotify;
    public bool inWin;
    public GameObject chp2;
    public int level;
    public int corret;

    public bool inInitializing;

    public GameObject FakeBG;
    public GameObject RealBG;

    public GameObject instruction;

    public GameObject WallUI;
    public GameObject CKPUI;
    public GameObject BWUI;
    public GameObject SSUI;
    public GameObject HammerUI;

    public Transform[] levelNPCs;

    public Transform Chp2loc;

    // Start is called before the first frame update

    void Start()
    {
        inInitializing = true;
        constantsList = constantsGameObject.GetComponent<ConstantsList>();
        foreach (Transform child in levelNPCs[level])
        {
            backToOrigin.Add(child.gameObject);
        }
        foreach (GameObject obj in backToOrigin)
        {
            originalPosition.Add(obj.transform.position);
        }
    }

    private void StartLevel()
    {
        FakeBG.SetActive(false);
        instruction.SetActive(false);   
        RealBG.SetActive(true);
        levelNPCs[level].BroadcastMessage("StartLevel");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Skip();
        }
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
                Skip();
            }
        }
        if (inWin)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                inWin = false;
                Time.timeScale = 1;
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

    public void Add() {
        corret++;
        if (level == 0)
        {
            if (corret == 4)
            {
                inWin = true;
                Time.timeScale = 0;
                win.SetActive(true);
            }
        }
    }

    public void Clear()
    {
        constantsList.reset = true;
        foreach (Transform child in levelNPCs[level])
        {
            child.gameObject.SetActive(true);
        }
        levelNPCs[level].BroadcastMessage("ResetLevel");
        notify.SetActive(false);
        win.SetActive(false);
        for (int i = 0; i < backToOrigin.Count; i++)
        {
            backToOrigin[i].transform.position = originalPosition[i];
        }
        foreach (Transform child in generated)
        {
            Destroy(child.gameObject);
        }
    }

    void Retry()
    {
        Clear();
    }

    void Skip()
    {
        chp2.SetActive(true);
        gameObject.SetActive(false);
        chp2.GetComponent<ToChp2>().Go(Chp2loc);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public GameObject constantsGameObject;
    private ConstantsList constantsList;
    public GameObject[] FakeDisplay;
    public GameObject[] ConstantDisplay;
    public GameObject[] GameDisplay;
    public GameObject Normal;
    public List<GameObject> backToOrigin = new List<GameObject>();
    public List<Vector3> originalPosition = new List<Vector3>();
    public Transform generated;
    public GameObject notify;
    public GameObject win;
    public bool inNotify;
    public bool inWin;
    
    // Start is called before the first frame update
    void Start()
    {
        constantsList = constantsGameObject.GetComponent<ConstantsList>();
        foreach (GameObject obj in ConstantDisplay)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        foreach (GameObject obj in ConstantDisplay)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        foreach (GameObject obj in backToOrigin)
        {
            originalPosition.Add(obj.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (GameObject obj in FakeDisplay)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
            foreach (GameObject obj in GameDisplay)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
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

        if (Normal.transform.position.x < constantsList.winX)
        {
            inWin = true;
            Time.timeScale = 0;
            win.SetActive(true);
        }
        else if (Normal.transform.position.x > constantsList.loseX)
        {
            inNotify = true;
            Time.timeScale = 0;
            notify.SetActive(true);
        }
    }

    void Clear()
    {
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
        gameObject.SetActive(false);
    }
}

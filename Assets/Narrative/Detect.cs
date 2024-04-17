using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Detect : MonoBehaviour
{
    public bool hit;
    public Flowchart flowchart;
    public GameObject soundControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit && Input.anyKeyDown)
        {
            hit = true;
            flowchart.ExecuteBlock("ToChp1");
            soundControl.GetComponent<BGMManager>().PlayInGameMusic();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReciever : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLevel()
    {
        gameObject.GetComponent<NPCPathfinder>().enabled = true;
        if (!gameObject.GetComponent<NPCPathfinder>())
        {
            gameObject.GetComponent<AudioSource>().volume = 0.3f;
        }
    }
}

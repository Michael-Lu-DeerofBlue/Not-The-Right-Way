using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConstantsList : MonoBehaviour
{

    public float mapSpeed;
    public float winX;
    public float loseX;
    public float boardcastDelay;
    public GameObject NPCs4;
    public GameObject NPCs3;
    public GameObject NPCs5;
    public GameObject Normal;
    public float checkpointDelay;
    public float policyDelay;
    public float hulkDelay;
    public bool reset;
    public void Boardcast()
    {
        Debug.Log("asd");
        StartCoroutine(BM(boardcastDelay));

    }

    IEnumerator BM(float delay)
    {
        GameObject NPCs = null;
        if (LevelCounter.counter == 4)
        {
            NPCs = NPCs4;
        }
        else if (LevelCounter.counter == 5)
        {
            NPCs = NPCs5;
        }
        else if (LevelCounter.counter == 3)
        {
            NPCs = NPCs3;
        }
        float ori_volume = Normal.GetComponent<AudioSource>().volume;
        Normal.GetComponent<AudioSource>().volume = 0;
        NPCs.BroadcastMessage("Stop");
        float ori_s = mapSpeed;
        mapSpeed = 0;
        yield return new WaitForSeconds(delay);
        NPCs.BroadcastMessage("Recover");
        if (!reset)
        {
            NPCs.BroadcastMessage("Accelerate");
        }
        else
        {
            reset = false;
        }
        mapSpeed = ori_s;
        Normal.GetComponent<AudioSource>().volume = ori_volume;
    }
}

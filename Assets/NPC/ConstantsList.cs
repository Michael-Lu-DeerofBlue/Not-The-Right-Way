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
    public GameObject NPCs;
    public GameObject Normal;

    public void Boardcast()
    {
        Debug.Log("asd");
        StartCoroutine(BM(boardcastDelay));

    }

    IEnumerator BM(float delay)
    {
        float ori_volume = Normal.GetComponent<AudioSource>().volume;
        Normal.GetComponent<AudioSource>().volume = 0;
        NPCs.BroadcastMessage("Stop");
        float ori_s = mapSpeed;
        mapSpeed = 0;
        yield return new WaitForSeconds(delay);
        NPCs.BroadcastMessage("Recover");
        NPCs.BroadcastMessage("Accelerate");
        mapSpeed = ori_s;
        Normal.GetComponent<AudioSource>().volume = ori_volume;
    }
}

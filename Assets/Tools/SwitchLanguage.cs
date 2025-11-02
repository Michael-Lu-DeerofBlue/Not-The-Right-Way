using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SwitchLanguage : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Reciever;
    private void OnMouseDown()
    {
        if (LocalizationManager.language == 0)
        {
            LocalizationManager.language = 1;
        }
        else
        {
            LocalizationManager.language = 0;
        }

        
        foreach (GameObject obj in Reciever)
        {
            obj.SendMessage("LanguageCheck");
        }

    }
}

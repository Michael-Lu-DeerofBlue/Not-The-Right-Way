using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChangeDueToLanguage : MonoBehaviour
{
    public GameObject[] English;
    public GameObject[] Chinese;
    // Start is called before the first frame update
    void Start()
    {
        LanguageCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LanguageCheck()
    {
        if (LocalizationManager.language == 0)
        {
            foreach (GameObject gameObject in English)
            {
                gameObject.SetActive(true);
            }
            foreach (GameObject gameObject in Chinese)
            {
                gameObject.SetActive(false);
            }
           
        }
        else
        {
            foreach (GameObject gameObject in English)
            {
                gameObject.SetActive(false);
            }
            foreach (GameObject gameObject in Chinese)
            {
                gameObject.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;

public class TextSelfCheck : MonoBehaviour
{
    public TMP_FontAsset English;
    public TMP_FontAsset Chinese;
    public string EngStr;
    public string ChnStr;
    // Start is called before the first frame update
    void Start()
    {
        Check();
    }

    public void LanguageCheck()
    {
        Check();
    }

        // Update is called once per frame
        void Update()
    {
        
    }

    void Check()
    {
        if (LocalizationManager.language == 0)
        {
            gameObject.GetComponent<TextMeshPro>().font = English;
            gameObject.GetComponent<TextMeshPro>().text = EngStr;
        }
        else
        {
            gameObject.GetComponent<TextMeshPro>().font = Chinese;
            gameObject.GetComponent<TextMeshPro>().text = ChnStr;
        }
    }
}

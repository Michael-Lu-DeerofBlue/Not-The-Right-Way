using UnityEngine;
using System.Collections;

public class SequDisplay : MonoBehaviour
{
    public GameObject[] objectsToDisplay;  // Array of GameObjects to display sequentially
    public float displayDuration;   // Duration for which to display each GameObject
    public bool autoPopulateChildren = true; // Whether to automatically populate the array with child GameObjects

    void Start()
    {
        if (autoPopulateChildren)
        {
            PopulateChildren();
        }
        StartCoroutine(DisplaySequence());
    }

    void PopulateChildren()
    {
        // Fetch all children and store them in objectsToDisplay
        objectsToDisplay = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            objectsToDisplay[i] = transform.GetChild(i).gameObject;
        }
    }

    IEnumerator DisplaySequence()
    {
        // Initially hide all objects
        foreach (var obj in objectsToDisplay)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Loop through each GameObject and display it
        foreach (var obj in objectsToDisplay)
        {
            if (obj != null)
            {
                obj.SetActive(true);  // Make the current object visible
                yield return new WaitForSeconds(displayDuration);  // Wait for the specified duration
            }
        }
    }
}

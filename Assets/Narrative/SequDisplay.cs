using UnityEngine;
using System.Collections;
using Fungus;
using UnityEngine.SceneManagement;

public class SequDisplay : MonoBehaviour
{
    public GameObject[] objectsToDisplay;  // Array of GameObjects to display sequentially
    public float displayDuration;   // Duration for which to display each GameObject
    public bool autoPopulateChildren = true; // Whether to automatically populate the array with child GameObjects
    public Flowchart flowchart;
    public bool Goable;

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
                Debug.Log("123");
                obj.SetActive(true);  // Make the current object visible
                yield return new WaitForSeconds(displayDuration);  // Wait for the specified duration
            }
        }
        Goable = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && Goable)
        {
            Debug.Log(LevelCounter.counter.ToString());
            if (LevelCounter.counter == 0)
            {
                flowchart.ExecuteBlock("MoveToBattle");
                LevelCounter.counter++;
            }
            if (LevelCounter.counter >= 2)
            {
                flowchart.ExecuteBlock("MoveToGame");
            }
            gameObject.SetActive(false);
        }
        
    }
}

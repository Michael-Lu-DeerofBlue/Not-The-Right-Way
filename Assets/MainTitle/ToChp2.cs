using UnityEngine;
using System.Collections;

public class ToChp2 : MonoBehaviour
{
    public SpriteFader spriteFader;
    public Transform nextchapter;  // Assign the target camera position in the inspector
    public Transform play;  // Assign the target camera position in the inspector
    public Transform mainCamera;  // Assign your main camera in the inspector
    public float cameraMoveSpeed = 100f;  // Speed at which the camera moves
    public float wait;
    public float dateDisplay;
    public float letterReading;
    public GameObject Chp2;
    public GameObject message;
    public bool inReading;
    public GameObject leveler;

    public void Go(Transform n)
    {
        nextchapter = n;
        StartCoroutine(PerformActions());
    }
    void Update()
    {
        if (inReading)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(GoGoGo());
            }
        }
        
    }

    IEnumerator PerformActions()
    {
        // Call the target sprite's FadeIn method
        spriteFader.FadeIn();

        // Wait for 0.5 seconds before moving the camera
        yield return new WaitForSeconds(wait);

        // Move the camera smoothly to the target location
        while (Vector3.Distance(mainCamera.transform.position, nextchapter.position) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, nextchapter.position, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // Once the camera is in position, call FadeOut
        spriteFader.FadeOut();
        Chp2.SetActive(true);
        message.SetActive(true);

        yield return new WaitForSeconds(letterReading);
        inReading = true;
    }

    IEnumerator GoGoGo()
    {
        yield return null;
        spriteFader.FadeIn();
        yield return new WaitForSeconds(wait);
        while (Vector3.Distance(mainCamera.transform.position, play.position) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, play.position, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        
        leveler.SetActive(true);
        leveler.GetComponent<Level1>().Clear();
        // Once the camera is in position, call FadeOut
        spriteFader.FadeOut();
        gameObject.SetActive(false);
    }
}

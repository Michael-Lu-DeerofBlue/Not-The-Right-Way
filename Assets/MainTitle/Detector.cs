using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour
{
    public SpriteFader spriteFader;
    public Transform chapter1;  // Assign the target camera position in the inspector
    public Transform play1;  // Assign the target camera position in the inspector
    public Transform mainCamera;  // Assign your main camera in the inspector
    public float cameraMoveSpeed = 5f;  // Speed at which the camera moves
    public float wait;
    public float dateDisplay;
    public float letterReading;
    public GameObject date;
    public GameObject officialDocument;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(PerformActions());
        }
    }

    IEnumerator PerformActions()
    {
        // Call the target sprite's FadeIn method
        spriteFader.FadeIn();

        // Wait for 0.5 seconds before moving the camera
        yield return new WaitForSeconds(wait);

        // Move the camera smoothly to the target location
        while (Vector3.Distance(mainCamera.transform.position, chapter1.position) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, chapter1.position, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // Once the camera is in position, call FadeOut
        spriteFader.FadeOut();

        yield return new WaitForSeconds(dateDisplay);
        date.SetActive(false);
        officialDocument.SetActive(true);

        yield return new WaitForSeconds(letterReading);

        spriteFader.FadeIn();
        yield return new WaitForSeconds(wait);
        while (Vector3.Distance(mainCamera.transform.position, play1.position) > 0.01f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, play1.position, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // Once the camera is in position, call FadeOut
        spriteFader.FadeOut();
        gameObject.SetActive(false);
    }
}

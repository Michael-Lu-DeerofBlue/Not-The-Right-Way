using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
    public GameObject trapPrefab; // Assign this in the Inspector with your trap prefab
    public GameObject placementIndicatorPrefab; // Assign the dotted circle prefab
    private GameObject currentIndicator; // To keep track of the instantiated indicator

    public string requiredSequence;
    private int currentIndex = 0;
    private bool isWaitingForInput = false;
    private bool isConfirmationRequired = false;
    private Vector2 placementPosition;

    void Update()
    {
        // Check for left mouse click to create or update the placement indicator
        if (Input.GetMouseButtonDown(0))
        {
            if (currentIndicator != null)
            {
                Destroy(currentIndicator); // Ensure only one indicator exists at a time
            }
            placementPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentIndicator = Instantiate(placementIndicatorPrefab, placementPosition, Quaternion.identity);
            isWaitingForInput = true;
            currentIndex = 0;
            isConfirmationRequired = false;
        }

        // Check for right mouse click to cancel placement
        if (Input.GetMouseButtonDown(1) && currentIndicator != null)
        {
            Destroy(currentIndicator);
            isWaitingForInput = false;
            isConfirmationRequired = false;
        }

        // Handle sequence input and check for confirmation
        if (isWaitingForInput && !isConfirmationRequired)
        {
            CheckSequenceInput();
        }

        if (isConfirmationRequired && Input.GetKeyDown(KeyCode.Space))
        {
            PlaceTrap();
            Destroy(currentIndicator); // Remove the indicator upon placing the trap
            isWaitingForInput = false;
            isConfirmationRequired = false;
        }
    }

    void CheckSequenceInput()
    {
        if (Input.GetKeyDown(requiredSequence[currentIndex].ToString()))
        {
            currentIndex++;
            if (currentIndex >= requiredSequence.Length)
            {
                isConfirmationRequired = true; // Require confirmation to place the trap
            }
        }
        else if (Input.anyKeyDown)
        {
            currentIndex = 0; // Reset sequence on any incorrect key press
        }
    }

    void PlaceTrap()
    {
        Instantiate(trapPrefab, placementPosition, Quaternion.identity);
    }
}

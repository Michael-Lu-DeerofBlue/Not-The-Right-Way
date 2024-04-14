using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
    public GameObject trapPrefab; // Assign the actual trap prefab
    public GameObject BWPrefab; // Assign the actual trap prefab
    public GameObject WallPrefab; // Assign the actual trap prefab
    public GameObject SSPrefab; // Assign the actual trap prefab
    public GameObject placementIndicatorPrefab; // Assign the dotted circle prefab
    private GameObject currentIndicator;
    public Transform parentObject;
    public bool isSelected = false;
    private int code;
    void Update()
    {
        if (!isSelected && gameObject.GetComponent<Hammer>().isSelected == false)
        {
            if (Input.GetKeyDown(KeyCode.A)) //trap
            {
                StartPlacement(trapPrefab, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S)) //wall
            {
                StartPlacement(WallPrefab, 2);
            }
            else if (Input.GetKeyDown(KeyCode.D)) //bw
            {
                StartPlacement(BWPrefab, 3);
            }
            else if (Input.GetKeyDown(KeyCode.W)) //ss
            {
                StartPlacement(SSPrefab, 4);
            }
        }
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            CancelPlacement();
        }

        if (isSelected)
        {
            if (currentIndicator != null)
            {
                currentIndicator.transform.position = GetMouseWorldPosition();
            }
            if (Input.GetMouseButtonDown(0))
            {
                FinalizePlacement();
            }
        }
    }

    private void StartPlacement(GameObject obj, int codee)
    {
        currentIndicator = Instantiate(obj, GetMouseWorldPosition(), Quaternion.identity);
        switch (codee)
        {
            case 1:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case 2:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case 3:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case 4:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
        }
        code = codee;
        currentIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); // Make the trap semi-transparent
        isSelected = true;
    }

    private void FinalizePlacement()
    {
        GameObject instance = null;
        Destroy(currentIndicator);
        switch (code)
        {
            case 1:
                instance = Instantiate(trapPrefab, GetMouseWorldPosition(), Quaternion.identity);
                break;
            case 2:
                instance = Instantiate(WallPrefab, GetMouseWorldPosition(), Quaternion.identity);
                break;
            case 3:
                instance = Instantiate(BWPrefab, GetMouseWorldPosition(), Quaternion.identity);
                break;
            case 4:
                instance = Instantiate(SSPrefab, GetMouseWorldPosition(), Quaternion.identity);
                break;
        }
        instance.transform.SetParent(parentObject);
        instance.GetComponent<SpriteRenderer>().color = Color.white; // Remove transparency
        isSelected = false;
        code = 0;
    }

    private void CancelPlacement()
    {
        if (currentIndicator != null)
            Destroy(currentIndicator);
        isSelected = false;
        code = 0;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

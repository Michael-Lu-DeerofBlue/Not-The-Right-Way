using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject hammerPrefab; // Assign the actual trap prefab
    public bool isSelected = false;
    private GameObject currentIndicator;
    public float xOffset;
    public float yOffset;
    // Start is called before the first frame update
    void Update()
    {
        if (!isSelected && gameObject.GetComponent<TrapPlacer>().isSelected == false)
        {
            if (Input.GetKeyDown(KeyCode.Q)) //trap
            {
                StartPlacement(hammerPrefab);
            }
        }
        if (isSelected)
        {
            if (currentIndicator != null)
            {
                currentIndicator.transform.position = GetMouseWorldPosition();
            }
            if (Input.GetMouseButtonDown(0)) // Right mouse button
            {
                Break();
            }
        }
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            CancelPlacement();
        }
    }

    private void Break()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        float xUpper = GetMouseWorldPosition().x + xOffset;
        float xLower = GetMouseWorldPosition().x - xOffset;
        float yUpper = GetMouseWorldPosition().x + yOffset;
        float yLower = GetMouseWorldPosition().x - yOffset;
        foreach (GameObject wall in walls)
        {
            Transform transform = wall.transform;
            if (transform.position.x < xUpper && transform.position.x > xLower)
            {
                if (transform.position.y < yUpper && transform.position.y > yLower)
                {
                    wall.GetComponent<Block>().Split();
                }
            }
        }
    }

    private void StartPlacement(GameObject obj)
    {
        currentIndicator = Instantiate(obj, GetMouseWorldPosition(), Quaternion.identity);
        isSelected = true;
    }
    private void CancelPlacement()
    {
        if (currentIndicator != null)
            Destroy(currentIndicator);
        isSelected = false;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

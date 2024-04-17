using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject hammerPrefab; // Assign the actual trap prefab
    public bool isSelected = false;
    private GameObject currentIndicator;
    public GameObject HammerUI;
    public Sprite HammerYes; // Original sprite
    public Sprite HammerNo; // Temporary sprite
    public bool HYes = true;
    public float HammerCooldown;
    // Start is called before the first frame update
    public Sprite brokenWall;

    
    void Update()
    {
        if (!isSelected && gameObject.GetComponent<TrapPlacer>().isSelected == false && HYes)
        {
            if (Input.GetKeyDown(KeyCode.Alpha5) && LevelCounter.counter >= 4) //trap
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
                StartCoroutine(SwitchHammerSpriteTemporarily());
                Break();
                CancelPlacement();
            }
        }
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            CancelPlacement();
        }
    }

    IEnumerator SwitchHammerSpriteTemporarily()
    {
        SpriteRenderer spriteRenderer = HammerUI.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = HammerNo; // Change the sprite to B
        HYes = false;
        yield return new WaitForSeconds(HammerCooldown); // Wait for 5 seconds
        spriteRenderer.sprite = HammerYes; // Change the sprite back to A
        HYes = true;

    }

    private void Break()
    {
        Dictionary<float, GameObject> distanceToGameObject = new Dictionary<float, GameObject>();
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            float deltax = GetMouseWorldPosition().x - wall.transform.position.x;
            float deltay = GetMouseWorldPosition().y - wall.transform.position.y;
            float distance = Mathf.Sqrt(deltax * deltax + deltay * deltay);
            distanceToGameObject.Add(distance, wall);
        }
        GameObject closest = FindClosest(distanceToGameObject);
        closest.gameObject.GetComponent<SpriteRenderer>().sprite = brokenWall;
        closest.gameObject.GetComponent<Block>().isBroken = true;
        
    }
    GameObject FindClosest(Dictionary<float, GameObject> distanceToGameObject)
    {
        if (distanceToGameObject.Count == 0)
            return null;

        float minDistance = float.MaxValue;
        GameObject closestObject = null;

        foreach (KeyValuePair<float, GameObject> pair in distanceToGameObject)
        {
            if (pair.Key < minDistance)
            {
                minDistance = pair.Key;
                closestObject = pair.Value;
            }
        }

        return closestObject;
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

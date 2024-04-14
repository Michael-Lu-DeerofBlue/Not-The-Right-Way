using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject wallPrefab; // Assign the wall prefab in the inspector
    public float gapHeight = 1.0f; // Height of the gap between the two new walls


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit block");
        if (other.gameObject.CompareTag("NPC")) // Make sure the collider is tagged as "NPC"
        {
            NPCPathfinder npc = other.GetComponent<NPCPathfinder>();
            if (npc != null)
            {
                //npc.Freeze(freezeTime); // Call the Freeze function on the NPC
            }
        }

    }

    // Function to split the wall
    public void Split()
    {
        if (gameObject.transform.localScale.y > 1.2f)
        {
            // Calculate new height for the smaller walls
            float newHeight = 0.13f;

            // Ensure the new height is positive
            if (newHeight <= 0)
            {
                Debug.LogError("Gap too big or wall too small.");
                return;
            }

            // Create two new wall segments
            CreateWallSegment(new Vector2(transform.position.x, transform.position.y + newHeight + gapHeight / 2), newHeight);
            CreateWallSegment(new Vector2(transform.position.x, transform.position.y - newHeight - gapHeight / 2), newHeight);

            // Optionally, disable or destroy the original wall object
            gameObject.SetActive(false);
        }
    }

    // Function to create a wall segment
    private void CreateWallSegment(Vector2 position, float height)
    {
        GameObject newWall = Instantiate(wallPrefab, position, Quaternion.identity);
        newWall.transform.localScale = new Vector3(0.118f, height, transform.localScale.z);
    }


}

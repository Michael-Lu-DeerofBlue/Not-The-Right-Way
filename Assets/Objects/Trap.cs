using UnityEngine;

public class Trap : MonoBehaviour
{
    public float freezeTime; // Time in seconds to freeze the NPC

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("asdasd");
        if (other.gameObject.CompareTag("NPC")) // Make sure the collider is tagged as "NPC"
        {
            NPCPathfinder npc = other.GetComponent<NPCPathfinder>();
            if (npc != null)
            {
                npc.Freeze(freezeTime); // Call the Freeze function on the NPC
            }
        }
        gameObject.SetActive(false);
    }
}
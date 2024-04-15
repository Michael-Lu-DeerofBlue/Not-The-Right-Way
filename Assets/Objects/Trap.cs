using UnityEngine;

public class Trap : MonoBehaviour
{
    public float freezeTime; // Time in seconds to freeze the NPC
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
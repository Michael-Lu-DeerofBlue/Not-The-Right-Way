using UnityEngine;

public class NPCMoveTest : MonoBehaviour
{
    public float speed;  // Speed of the NPC movement

    void Update()
    {
        // Move the NPC to the right at a constant speed
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Optionally, reset the position to the left side of the screen when it goes off the right edge
        if (transform.position.x > Screen.width)
        {
            transform.position = new Vector2(0, transform.position.y);
        }
    }
}
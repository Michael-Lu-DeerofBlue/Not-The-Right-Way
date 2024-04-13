using UnityEditor;
using UnityEngine;
using System.Collections;

public class NPCPathfinder : MonoBehaviour
{
    public GameObject costants;
    public float speed;
    public LayerMask obstacleLayer; // Layer to define what the obstacles are

    private bool isMovingVertically = false;
    private Vector2 movementDirection = Vector2.right;
    private BoxCollider2D npcCollider;
    private ConstantsList constantsList;

    void Start()
    {
        constantsList = costants.GetComponent<ConstantsList>();
        npcCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        NPCProcess();
        MoveNPC();
    }

    private void NPCProcess()
    {
        Vector2 boxSize = new Vector2(npcCollider.size.x * transform.localScale.x, npcCollider.size.y * transform.localScale.y);
        float castDistance = 0; // Distance to cast the box

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, movementDirection, castDistance, obstacleLayer);

        if (hit.collider != null)
        {
            if (!isMovingVertically)
            {
                isMovingVertically = true;
                movementDirection = (transform.position.y < hit.collider.transform.position.y) ? Vector2.down : Vector2.up;
                movementDirection = 5 * movementDirection + new Vector2(-3, 0);
            }
        }
        else
        {
            if (isMovingVertically)
            {
                RaycastHit2D sideCheck = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.right, castDistance, obstacleLayer);
                if (sideCheck.collider == null)
                {
                    isMovingVertically = false;
                    movementDirection = Vector2.right;
                }
            }
        }
    }

    public void Freeze(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    IEnumerator FreezeCoroutine(float duration)
    {
        float originalSpeed = speed;
        speed = 0; // Set speed to 0 to "freeze" the NPC
        yield return new WaitForSeconds(duration); // Wait for the duration of the freeze
        speed = originalSpeed; // Restore the original speed
    }

    public void MoveNPC()
    {
        float actualSpeed = speed + constantsList.mapSpeed;
        transform.Translate(movementDirection * actualSpeed * Time.deltaTime);
    }

}

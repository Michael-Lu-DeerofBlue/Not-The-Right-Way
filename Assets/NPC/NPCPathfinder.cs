using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class NPCPathfinder : MonoBehaviour
{
    public GameObject constantsGameObject;
    private float ori_speed;
    public float speed;
    public float speedDeductor;
    private Vector2 movementDirection = Vector2.right;
    private ConstantsList constantsList;
    private bool isSlow = false;
    private bool isWall;
    public bool isBW;
    public GameObject Win;
    public GameObject Lose;
    public bool isHulk;
    public bool isKid;
    public bool isVIP;
    public float accConstant;

    private GameObject destoryObj;
    void Start()
    {
        ori_speed = speed;
        constantsList = constantsGameObject.GetComponent<ConstantsList>();
    }

    void Update()
    {
        MoveNPC();
        //CheckWin();
    }

    private void MoveNPC()
    {
        float actualSpeed = speed + constantsList.mapSpeed;
        if (isWall)
        {
            float actualV = movementDirection.magnitude;
            transform.Translate(movementDirection.normalized * actualV * Time.deltaTime);
        }
        else
        {
            transform.Translate(movementDirection * actualSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wall":
                if (isHulk)
                {
                    Destroy(collision.gameObject);
                }
                else
                {
                    ChooseVerticalDirection(collision);
                }
                break;
            case "BarberedWire":
                isBW = true;
                speed = speed * speedDeductor;
                break;
            case "Checkpoint":
                StartCoroutine(CheckPointStopMovement(1));
                destoryObj = collision.gameObject;
                break;
            case "Police":
               
                constantsList.Boardcast();
                destoryObj = collision.gameObject;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // Check if the NPC has cleared the top of the wall
            float lowerBound = collision.transform.position.y - (collision.bounds.size.y  + gameObject.GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y)/2 + 0.05f;
            float upperBound = collision.transform.position.y + (collision.bounds.size.y  + gameObject.GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y)/2 - 0.05f;
            if (movementDirection.y > 0  && transform.position.y > upperBound)
            {
                ResumeHorizontalMovement();
            }
            else if (movementDirection.y < 0 && transform.position.y < lowerBound)
            {
                ResumeHorizontalMovement();
            }
        }
        if (collision.gameObject.tag == "BarberedWire")
        {
            // Check if the NPC has cleared the top of the wall
            float lowerBound = collision.transform.position.x - (collision.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 + 0.05f;
            float upperBound = collision.transform.position.x + (collision.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 - 0.05f;
            if (gameObject.transform.position.x > upperBound)
            {
                speed = ori_speed;
                isBW = false;
                destoryObj = collision.gameObject;
                DestroyObject();
            }
        }
    }

    private void ChooseVerticalDirection(Collider2D hit)
    {
        //Time.timeScale = 0f;  
        // Determine the vertical direction based on the relative position to the wall
        movementDirection = (transform.position.y < hit.transform.position.y) ? Vector2.down : Vector2.up;
        Debug.Log(movementDirection.ToString());
        // Set lateral movement to zero while moving vertically
        Vector2 lateralMovement = new Vector2(constantsList.mapSpeed, 0); // The wall's leftward movement
        movementDirection = 2f * movementDirection + lateralMovement;
        isWall = true;
        //Debug.Log(movementDirection.ToString());
    }

    private void ResumeHorizontalMovement()
    {
        // Clear any vertical offset to resume direct horizontal movement
        isWall = false;
        movementDirection = Vector2.right;
    }


    IEnumerator CheckPointStopMovement(float delay)
    {
        float originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = originalSpeed;
        DestroyObject();
    }

    public void DestroyObject()
    {
        Destroy(destoryObj);
    }

    private void CheckWin()
    {
        if (isKid)
        {
            if (gameObject.transform.position.x < constantsList.winX)
            {
                Lose.SetActive(true);
            }
        }
        else if (isVIP)
        {
            if (gameObject.transform.position.x < constantsList.winX)
            {
                Lose.SetActive(true);
            }
            if (gameObject.transform.position.x > constantsList.loseX)
            {
                Lose.SetActive(true);
            }
        }
        else
        {
            if (gameObject.transform.position.x < constantsList.winX)
            {
                Win.SetActive(true);
            }
            if (gameObject.transform.position.x > constantsList.loseX)
            {
                Lose.SetActive(true);
            }
        }
       
    }

    public void Stop()
    {
        speed = 0;
    }

    public void Recover()
    {
        speed = ori_speed;
        DestroyObject();
    }

    public void Accelerate()
    {
        speed = ori_speed;
        speed = accConstant * speed;
        ori_speed = speed;
    }
}

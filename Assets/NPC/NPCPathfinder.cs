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
    public bool isWall;
    private bool inSeperation;
    public bool isBW;
    public GameObject Win;
    public GameObject Lose;
    public bool isHulk;
    public bool isKid;
    public bool isVIP;
    public float accConstant;
    public float separationDuration;
    private Rigidbody2D rb;
    private Vector2 ori_velocity;
    private float ori_angvelo;
    private float ori_inertia;
    public Collider2D theWall;


    private GameObject destoryObj;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ori_velocity = rb.velocity;
        ori_angvelo = rb.angularVelocity;
        ori_inertia = rb.inertia;
        ori_speed = speed;
        constantsList = constantsGameObject.GetComponent<ConstantsList>();
    }

    void Update()
    {
        if (isWall)
        {
            checkWithWall();
        }
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        //Debug.Log(gameObject.name + movementDirection);
        // Check if the Z rotation is not zero
        if (eulerRotation.z != 0)
        {
            if (CheckSeparationSuccess() && !isWall)
            {
                eulerRotation.z = 0;
                movementDirection = new Vector2(1,0);
            }

            // Convert back to a quaternion and apply it to the transform
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
        MoveNPC();
        //CheckWin();
    }

    private void MoveNPC()
    {
        float actualSpeed = speed + constantsList.mapSpeed;
        if (isWall)
        {
            //Debug.Log(gameObject.name + movementDirection);
            float actualV = movementDirection.magnitude;
            transform.Translate(movementDirection.normalized * actualV * Time.deltaTime);
        }/*
        else if (inSeperation)
        {
            transform.Translate(movementDirection * actualSpeed * Time.deltaTime);
            inSeperation = CheckSeparationSuccess();
        }*/
        else
        {
            transform.Translate(movementDirection * actualSpeed * Time.deltaTime);
        }
    }

    void checkWithWall()
    {
        // Check if the NPC has cleared the top of the wall
        float lowerBound = theWall.transform.position.y - (theWall.bounds.size.y + gameObject.GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y) / 2 + 0.05f;
        float upperBound = theWall.transform.position.y + (theWall.bounds.size.y + gameObject.GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y) / 2 - 0.05f;
        if (movementDirection.y > 0 && transform.position.y > upperBound)
        {
            ResumeHorizontalMovement();
        }
        else if (movementDirection.y < 0 && transform.position.y < lowerBound)
        {
            ResumeHorizontalMovement();
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        switch (collision.collider.gameObject.tag)
        {
            case "Wall":
                theWall = collision.collider;
                if (isHulk)
                {
                    Destroy(collision.gameObject);
                }
                else
                {
                    ChooseVerticalDirection(collision.collider);
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

    private void OnCollisionStay2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "BarberedWire")
        {
            // Check if the NPC has cleared the top of the wall
            float lowerBound = collision.transform.position.x - (collision.collider.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 + 0.05f;
            float upperBound = collision.transform.position.x + (collision.collider.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 - 0.05f;
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
        Debug.Log(hit.name);
        //Time.timeScale = 0f;  
        // Determine the vertical direction based on the relative position to the wall
        movementDirection = (transform.position.y < hit.transform.position.y) ? Vector2.down : Vector2.up;
        //Debug.Log(movementDirection.ToString());
        // Set lateral movement to zero while moving vertically
        Vector2 lateralMovement = new Vector2(constantsList.mapSpeed, 0); // The wall's leftward movement
        movementDirection = 2f * movementDirection + lateralMovement;
        isWall = true;
        Debug.Log(gameObject.name + movementDirection);
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


    bool CheckSeparationSuccess()
    {
        float radius = GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x / 2;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius); // Smaller radius for separation check
        foreach (var hit in hits)
        {
            //Debug.Log(hit.gameObject.name);
            if (hit.CompareTag("NPC") && hit.gameObject != gameObject)
            {
                Debug.Log("Separation failed, still too close to another NPC.");
                return false; // Additional logic for failed separation could be implemented here
            }
        }
        return true;
    }

    void OnDrawGizmos()
    {
        float radius = GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x / 2;
        // Set the color of the Gizmos to a visible color, e.g., red
        Gizmos.color = Color.red;

        // Draw a wire sphere at the position of the GameObject with the detection radius
        Gizmos.DrawWireSphere(transform.position, radius);
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

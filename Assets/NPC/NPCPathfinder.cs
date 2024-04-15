using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;

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
    private Collider2D theWall;
    private Collider2D theBW;
    public float[] gapThreshold;
    private Animator animator;
    private GameObject destoryObj;
    void Start()
    {
        animator = GetComponent<Animator>();
        ori_speed = speed;
        constantsList = constantsGameObject.GetComponent<ConstantsList>();
    }

    void Update()
    {
        if (isWall)
        {
            checkWithWall();
        }
        if (isBW)
        {
            checkWithBW();
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
        }
        else
        {
            transform.Translate(movementDirection * actualSpeed * Time.deltaTime);
        }
    }

    void checkWithWall()
    {
        if (theWall.IsDestroyed())
        {
            ResumeHorizontalMovement();
        }
        else
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
       
    }

    void checkWithBW()
    {
        float lowerBound = theBW.transform.position.x - (theBW.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 + 0.05f;
        float upperBound = theBW.transform.position.x + (theBW.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 - 0.05f;
        if (gameObject.transform.position.x > upperBound)
        {
            speed = ori_speed;
            isBW = false;
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
                    StartCoroutine(HulkStopMovement(1));
                }
                else
                {
                    ChooseVerticalDirection(collision.collider);
                }
                break;
            case "BarberedWire":
                theBW = collision.collider; 
                isBW = true;
                speed = speed * speedDeductor;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                break;
            case "Checkpoint":
                StartCoroutine(CheckPointStopMovement(1));
                destoryObj = collision.gameObject;
                break;
            case "Police":
                Debug.Log("AA");
                StartCoroutine(PoliceStopMovement(0.5f));
                constantsList.Boardcast();
                destoryObj = collision.gameObject;
                collision.collider.gameObject.GetComponent<AnimationControl>().ResumeAnimation();
                break;
        }
    }

    private void ChooseVerticalDirection(Collider2D hit)
    {
        //Debug.Log(hit.name);
        //Time.timeScale = 0f;  
        // Determine the vertical direction based on the relative position to the wall
        movementDirection = (transform.position.y < hit.transform.position.y) ? Vector2.down : Vector2.up;
        bool flip = CalculateGap(hit, movementDirection);
        if (flip)
        {
            movementDirection *= -1;
        }
        //Debug.Log(movementDirection.ToString());
        // Set lateral movement to zero while moving vertically
        Vector2 lateralMovement = new Vector2(constantsList.mapSpeed, 0); // The wall's leftward movement
        movementDirection = 2f * movementDirection + lateralMovement;
        isWall = true;
        //Debug.Log(gameObject.name + movementDirection);
    }

    bool CalculateGap(Collider2D hit, Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            float upThreshold = 3.536f;
            float topPoint = hit.transform.position.y + hit.GetComponent<BoxCollider2D>().size.y * hit.transform.localScale.y/2;
            float NPCsize = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.x;
            if (upThreshold - topPoint < NPCsize)
            {
                return true;
            }
            else return false;
        }
        else
        {
            
            float downThreshold = -3.152f;
            float downPoint = hit.transform.position.y - hit.GetComponent<BoxCollider2D>().size.y * hit.transform.localScale.y/2;
            float NPCsize = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y;
            Debug.Log(hit.transform.position.y.ToString());
            Debug.Log(hit.GetComponent<BoxCollider2D>().size.y.ToString());
            Debug.Log(hit.transform.localScale.y.ToString());
            Debug.Log(downPoint.ToString());
            Debug.Log((downPoint - downThreshold).ToString());
            if (downPoint - downThreshold < NPCsize)
            {
                return true;
            }
            else return false;
        }
        
    }

    private void ResumeHorizontalMovement()
    {
        // Clear any vertical offset to resume direct horizontal movement
        isWall = false;
        movementDirection = Vector2.right;
    }

    IEnumerator HulkStopMovement(float delay)
    {
        gameObject.tag = "Wall";
        animator.speed = 0;
        float originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = originalSpeed;
        animator.speed = 1;
        gameObject.tag = "NPC";
        DestroyObject();
    }

    IEnumerator CheckPointStopMovement(float delay)
    {
        gameObject.tag = "Wall";
        animator.speed = 0;
        float originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = originalSpeed;
        animator.speed = 1;
        gameObject.tag = "NPC";
        DestroyObject();
    }

    IEnumerator PoliceStopMovement(float delay)
    {
        gameObject.tag = "Wall";
        animator.speed = 0;
        float originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        DestroyObject();
        gameObject.SetActive(false);
    }


    bool CheckSeparationSuccess()
    {
        float radius = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y / 2;
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
        float radius = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y / 2;
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

        animator.speed = 0;
        speed = 0;
    }

    public void Recover()
    {
        animator.speed = 1;
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

using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;

public class NPCPathfinder : MonoBehaviour
{
    public GameObject constantsGameObject;
    public float ori_speed;
    public float speed;
    public float speedDeductor;
    private Vector2 movementDirection = Vector2.right;
    public ConstantsList constantsList;
    private bool isSlow = false;
    public bool isWall;
    private bool inSeperation;
    public bool isBW;
    public GameObject Win;
    public GameObject Lose;
    public bool isHulk;
    public bool isKid;
    public bool metBrokenWall;
    public bool isVIP;
    public float accConstant;
    public float separationDuration;
    private Collider2D theWall;
    private Collider2D theBW;
    public float[] gapThreshold;
    public Animator animator;
    private GameObject destoryObj;
    public bool inStop;
    private float checkpointDelay;
    private float policyDelay;
    private float hulkDelay;
    public GameObject leveler;
    public bool Added;
    public GameObject NormalExtraSound;
    private void OnEnable()
    {
        
        constantsList = constantsGameObject.GetComponent<ConstantsList>();
        animator = GetComponent<Animator>();
        animator.speed = 1;
        ori_speed = speed;
        checkpointDelay = constantsList.checkpointDelay;
        policyDelay = constantsList.policyDelay;
        hulkDelay = constantsList.hulkDelay;
    }

    void Update()
    {
        if (metBrokenWall)
        {
            BrokenWallSituation();
        }
        else
        {
            if (isWall)
            {
                checkWithWall();
            }
            if (isBW)
            {
                checkWithBW();
            }
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
        CheckLose();
        CheckWin();
    }

    public void ResetLevel() {
        Added = false; 
        isWall = isBW = metBrokenWall = inStop = isSlow = false;
        speed = 2.3f;
        ori_speed = speed;
    }
    private void CheckLose()
    {
        if (isKid)
        {
            if (gameObject.transform.position.x < constantsList.winX)
            {
                leveler.BroadcastMessage("Lose");
            }
        }
        else if (isVIP)
        {
            if (gameObject.transform.position.x < constantsList.winX)
            {
                leveler.BroadcastMessage("Lose");
            }
            if (gameObject.transform.position.x > constantsList.loseX)
            {
                leveler.BroadcastMessage("Lose");
            }
        }
        else
        {
            if (gameObject.transform.position.x > constantsList.loseX)
            {
                leveler.BroadcastMessage("Lose");
            }
        }

    }

    private void CheckWin()
    {
        if (!Added)
        {
            if (isKid)
            {
                if (gameObject.transform.position.x > constantsList.loseX)
                {
                    leveler.BroadcastMessage("Add");
                    Added = true;
                    gameObject.SetActive(false);
                }
            }
            else if (!isVIP)
            {
                if (gameObject.transform.position.x < constantsList.winX)
                {
                    leveler.BroadcastMessage("Add");
                    Added = true;
                    gameObject.SetActive(false);
                }
            }
        }
        

    }

    void BrokenWallSituation()
    {
        if (gameObject.transform.position.y.ToString("F1") == theWall.transform.position.y.ToString("F1"))
        {
            float upperBound = theWall.transform.position.x + (theWall.bounds.size.x + gameObject.GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x) / 2 - 0.05f;
            if (gameObject.transform.position.x > upperBound)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                metBrokenWall = false;
                isWall = false;
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                movementDirection = Vector2.right;
            }
        }
        else
        {
            movementDirection = (transform.position.y < theWall.transform.position.y) ? Vector2.up : Vector2.down;
        }
    }

    private void MoveNPC()
    {
        float actualSpeed = speed + constantsGameObject.GetComponent<ConstantsList>().mapSpeed;
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
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        switch (collision.collider.gameObject.tag)
        {
            case "Wall":
                theWall = collision.collider;
                if (isKid && collision.gameObject.GetComponent<Block>().isBroken)
                {
                    metBrokenWall = true;
                }
                if (isHulk)
                {
                    gameObject.GetComponent<SingleSoundController>().PlaySound();
                    Destroy(collision.gameObject);
                    StartCoroutine(HulkStopMovement(hulkDelay));
                }
                else
                {
                    ChooseVerticalDirection(collision.collider);
                }
                break;
            case "BarberedWire":
                if (isHulk)
                {
                    gameObject.GetComponent<ExtraSoundController>().PlaySound();
                }
                else if (isKid || isVIP)
                {
                    gameObject.GetComponent<SingleSoundController>().PlaySound();
                }
                else
                {
                    NormalExtraSound.GetComponent<SingleSoundController>().PlaySound();
                }
                theBW = collision.collider; 
                isBW = true;
                speed = speed * speedDeductor;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                break;
            case "Checkpoint":
                StartCoroutine(CheckPointStopMovement(checkpointDelay));
                destoryObj = collision.gameObject;
                break;
            case "Police":
                Debug.Log("AA");
                StartCoroutine(PoliceStopMovement(policyDelay));
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
            float upThreshold = 3.817f;
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
            
            float downThreshold = -3.37f;
            float downPoint = hit.transform.position.y - hit.GetComponent<BoxCollider2D>().size.y * hit.transform.localScale.y/2;
            float NPCsize = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y;
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
        inStop = true;
        animator.speed = 0;
        float originalSpeed = speed;
        speed = 0;
        gameObject.tag = "Wall";
        yield return new WaitForSeconds(delay);
        speed = originalSpeed;
        animator.speed = 1;
        gameObject.tag = "NPC";
        inStop = false;
        DestroyObject();
    }

    IEnumerator CheckPointStopMovement(float delay)
    {
        inStop = true;
        gameObject.tag = "Wall";
        animator.speed = 0;
        float originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        speed = originalSpeed;
        animator.speed = 1;
        gameObject.tag = "NPC";
        inStop = false;
        DestroyObject();
    }

    IEnumerator PoliceStopMovement(float delay)
    {
        inStop = true;
        gameObject.tag = "Wall";
        animator.speed = 0;
        float originalSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(delay);
        inStop = false;
        DestroyObject();
        if (isVIP)
        {
            if (!Added)
            {
                gameObject.SetActive(false);
                leveler.BroadcastMessage("Add");
                Added = true;
            }
        }
        else
        {
            leveler.BroadcastMessage("Lose");
        }
        
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
                //Debug.Log("Separation failed, still too close to another NPC.");
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

   

    public void Stop()
    {

        animator.speed = 0;
        speed = 0;
    }

    public void Recover()
    {
        if (inStop == false)
        {
            animator.speed = 1;
            speed = ori_speed;
        }
        
    }

    public void Accelerate()
    {
        speed = ori_speed;
        speed = accConstant * speed;
    }
}

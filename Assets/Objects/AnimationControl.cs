using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = 0;  // Set the playback speed to 0 to pause animations
        }
    }

    public void ResumeAnimation()
    {
        if (animator != null)
        {
            animator.speed = 1;  // Set the playback speed back to 1 to resume animations
        }
    }

}
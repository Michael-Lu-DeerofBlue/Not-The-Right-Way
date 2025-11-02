using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormaltoVIP : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Change()
    {
        animator.Play("VIP-walk");
    }
}

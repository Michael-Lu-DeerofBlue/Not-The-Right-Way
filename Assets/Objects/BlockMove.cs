using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    public GameObject costants;
    public float speed;
    private ConstantsList constantsList;


    void Start()
    {
        constantsList = costants.GetComponent<ConstantsList>();
    }
    void Update()
    {
        Vector2 movementDirection = Vector2.right;
        float actualSpeed = speed + constantsList.mapSpeed;
        transform.Translate(movementDirection * actualSpeed * Time.deltaTime);
    }
}

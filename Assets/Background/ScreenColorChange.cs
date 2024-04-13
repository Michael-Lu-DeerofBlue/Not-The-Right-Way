using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer targetSprite;  // Assign this in the inspector

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change the sprite color to blue
            targetSprite.color = Color.blue;
        }
    }
}

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class TrapPlacer : MonoBehaviour
{
    public GameObject trapPrefab; // Assign the actual trap prefab
    public GameObject trapStillPrefab; // Assign the actual trap prefab
    public GameObject BWPrefab; // Assign the actual trap prefab
    public GameObject BWStillPrefab; // Assign the actual trap prefab
    public GameObject WallPrefab; // Assign the actual trap prefab
    public GameObject SSPrefab; // Assign the actual trap prefab
    public GameObject placementIndicatorPrefab; // Assign the dotted circle prefab
    private GameObject currentIndicator;
    public Transform parentObject;
    public bool isSelected = false;
    private int code;
    public bool[] trapYes = new bool[] { true, true, true, true, true}; // Track the last time a trap was placed
    public float[] trapCooldown;
    public GameObject wallUI;
    public Sprite wallYes; // Original sprite
    public Sprite wallNo; // Temporary sprite
    public GameObject CKPUI;
    public Sprite CKPYes; // Original sprite
    public Sprite CKPNo; // Temporary sprite
    public GameObject BWUI;
    public Sprite BWYes; // Original sprite
    public Sprite BWNo; // Temporary sprite
    public GameObject SSUI;
    public Sprite SSYes; // Original sprite
    public Sprite SSNo; // Temporary sprite
    void Update()
    {
        if (!isSelected && gameObject.GetComponent<Hammer>().isSelected == false)
        {
            if (Input.GetKeyDown(KeyCode.A)) //trap
            {
                if (trapYes[0])
                {
                    StartPlacement(trapStillPrefab, 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S)) //wall
            {
                if (trapYes[1])
                {
                    StartPlacement(WallPrefab, 2);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D)) //bw
            {
                if (trapYes[2])
                {
                    StartPlacement(BWStillPrefab, 3);
                }
            }
            else if (Input.GetKeyDown(KeyCode.W)) //ss
            {
                if (trapYes[3])
                {
                    StartPlacement(SSPrefab, 4);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            CancelPlacement();
        }

        if (isSelected)
        {
            if (currentIndicator != null)
            {
                currentIndicator.transform.position = GetMouseWorldPosition();
            }
            if (Input.GetMouseButtonDown(0))
            {
                FinalizePlacement();
            }
        }

    }

    IEnumerator SwitchCKPSpriteTemporarily()
    {
        SpriteRenderer spriteRenderer = CKPUI.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = CKPNo; // Change the sprite to B
        trapYes[0] = false;
        yield return new WaitForSeconds(trapCooldown[0]); // Wait for 5 seconds
        spriteRenderer.sprite = CKPYes; // Change the sprite back to A
        trapYes[0] = true;

    }
    IEnumerator SwitchWallSpriteTemporarily()
    {
        SpriteRenderer spriteRenderer = wallUI.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = wallNo; // Change the sprite to B
        trapYes[1] = false;
        yield return new WaitForSeconds(trapCooldown[1]); // Wait for 5 seconds
        spriteRenderer.sprite = wallYes; // Change the sprite back to A
        trapYes[1] = true;
    }

    IEnumerator SwitchBWSpriteTemporarily()
    {
        SpriteRenderer spriteRenderer = BWUI.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = BWNo; // Change the sprite to B
        trapYes[2] = false;
        yield return new WaitForSeconds(trapCooldown[2]); // Wait for 5 seconds
        spriteRenderer.sprite = BWYes; // Change the sprite back to A
        trapYes[2] = true;
    }

    IEnumerator SwitchSSSpriteTemporarily()
    {
        SpriteRenderer spriteRenderer = SSUI.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SSNo; // Change the sprite to B
        trapYes[3] = false;
        yield return new WaitForSeconds(trapCooldown[3]); // Wait for 5 seconds
        spriteRenderer.sprite = SSYes; // Change the sprite back to A
        trapYes[3] = true;
    }

    private void StartPlacement(GameObject obj, int codee)
    {
        currentIndicator = Instantiate(obj, GetMouseWorldPosition(), Quaternion.identity);
        switch (codee)
        {
            case 1:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case 2:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case 3:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
            case 4:
                currentIndicator.GetComponent<BoxCollider2D>().enabled = false;
                break;
        }
        code = codee;
        currentIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); // Make the trap semi-transparent
        isSelected = true;
    }

    private void FinalizePlacement()
    {
        GameObject instance = null;
        Destroy(currentIndicator);
        switch (code)
        {
            case 1:
                instance = Instantiate(trapPrefab, GetMouseWorldPosition(), Quaternion.identity);
                StartCoroutine(SwitchCKPSpriteTemporarily());
                break;
            case 2:
                instance = Instantiate(WallPrefab, GetMouseWorldPosition(), Quaternion.identity);
                StartCoroutine(SwitchWallSpriteTemporarily());
                break;
            case 3:
                instance = Instantiate(BWPrefab, GetMouseWorldPosition(), Quaternion.identity);
                StartCoroutine(SwitchBWSpriteTemporarily());
                break;
            case 4:
                instance = Instantiate(SSPrefab, GetMouseWorldPosition(), Quaternion.identity);
                StartCoroutine(SwitchSSSpriteTemporarily());
                break;
        }
        instance.transform.SetParent(parentObject);
        instance.GetComponent<SpriteRenderer>().color = Color.white; // Remove transparency
        isSelected = false;
        code = 0;
    }

    private void CancelPlacement()
    {
        if (currentIndicator != null)
            Destroy(currentIndicator);
        isSelected = false;
        code = 0;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

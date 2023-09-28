using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    public bool nextLifeState;

    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = isAlive;   
    }

    public void UpdateNextLifeState()
    {
        isAlive = nextLifeState;
    }
}


//TODO: Calculate next generation
// check neighbors -> make function?
// different code depending on # of neighbors
// edge cases
// numberOfAliveNeighbors
// CompensateOwnCell
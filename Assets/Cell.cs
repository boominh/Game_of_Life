using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool lifeState;
    public bool nextLifeState;

    //sprite bool=?
    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = lifeState;   
    }

    public void UpdateNextLifeState()
    {
        lifeState = nextLifeState;
    }
}


//TODO: Calculate next generation
// check neighbors -> make function?
// different code depending on # of neighbors
// edge cases
// numberOfAliveNeighbors
// CompensateOwnCell
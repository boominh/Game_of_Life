using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    public bool nextLifeState;
    public bool previousLifeState;

    Color32 aliveColor = new Color32(86, 198, 57, 255);
    Color32 trailColor = new Color32(0, 100, 0, 255);

    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = isAlive;

        TrailEffect();
    }

    private void TrailEffect()
    {
        if (isAlive != previousLifeState)
        {
            if (isAlive == false)
            {
                spriteRenderer.color = trailColor;
                spriteRenderer.enabled = true;
            }

            if (isAlive)
            {
                spriteRenderer.color = aliveColor;
            }
        }
    }

    public void UpdateNextLifeState()
    {
        previousLifeState = isAlive;
        isAlive = nextLifeState;
    }
}


//TODO: Calculate next generation
// check neighbors -> make function?
// different code depending on # of neighbors
// edge cases
// numberOfAliveNeighbors
// CompensateOwnCell
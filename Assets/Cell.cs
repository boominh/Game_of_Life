using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool currentLifeState;
    public bool nextLifeState;
    public bool previousLifeState;

    byte trailFadingConstant = 25;

    Color32 aliveColor = new Color32(86, 198, 57, 255);
    Color32 trailColor = new Color32(0, 100, 0, 255);

    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = currentLifeState;

        TrailEffect();
    }

    private void TrailEffect()
    {
        if (currentLifeState == false)
        {
            spriteRenderer.color = trailColor;
            trailColor.g -= trailFadingConstant;
            Mathf.Clamp(trailColor.g, 0, 100);
            spriteRenderer.enabled = true;
        }

        if (currentLifeState)
        {
            spriteRenderer.color = aliveColor;
        }
    }

    public void UpdateNextLifeState()
    {
        previousLifeState = currentLifeState;
        currentLifeState = nextLifeState;
    }
}


//TODO: Calculate next generation
// check neighbors -> make function?
// different code depending on # of neighbors
// edge cases
// numberOfAliveNeighbors
// CompensateOwnCell
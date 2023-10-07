using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool currentLifeState;
    public bool nextLifeState;
    public bool previousLifeState;

    bool fading = true;
    byte trailFadingConstant = 100/10;

    Color32 aliveColor = new Color32(86, 198, 57, 255);
    Color32 originalTrailColor = new Color32(0, 100, 0, 255);
    Color32 currentTrailColor;

    SpriteRenderer spriteRenderer;

    public void UpdateSpriteRenderer()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = currentLifeState;

        TrailEffect();
    }

    private void TrailEffect()
    {
        // Resets color for when revived 
        if (currentLifeState != previousLifeState)
        {
            currentTrailColor = originalTrailColor;
            fading = true;
        }
        
        // While dead
        if (currentLifeState == false && fading)
        {
            spriteRenderer.color = currentTrailColor;
            spriteRenderer.enabled = true;
            FadeTrailColor();
        }

        // Resets color for when alive
        if (currentLifeState)
        {
            spriteRenderer.color = aliveColor;
        }
    }

    private void FadeTrailColor()
    {
        currentTrailColor.g -= trailFadingConstant;
        if (currentTrailColor.g <= 0)
        {
            fading = false;
        }
    }

    public void UpdateNextLifeState()
    {
        previousLifeState = currentLifeState;
        currentLifeState = nextLifeState;
    }
}
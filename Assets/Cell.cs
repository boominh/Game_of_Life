using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool lifeState;
    public bool nextLifeState;

    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = lifeState;
        lifeState = nextLifeState;
    }
}
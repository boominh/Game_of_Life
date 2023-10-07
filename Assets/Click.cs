using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    bool lifestate;

    GameOfLife gameOfLife;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lifestate = true;
            ChangeLifeStateOnHit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            lifestate = false;
            ChangeLifeStateOnHit();
        }
    }

    void ChangeLifeStateOnHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit;
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject clickedCell = hit.collider.gameObject;
            clickedCell.GetComponent<Cell>().currentLifeState = lifestate;
        }
    }
}

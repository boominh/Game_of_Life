using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int frameRate;
    public GameObject cellPrefab;
    Cell[,] cells;

    float cellSize = 0.01f;
    int numberOfColums, numberOfRows;
    int spawnChancePercentage = 15;

    int numberOfAliveCells;

    void Start()
    {
        QualitySettings.vSyncCount = 0;

        numberOfRows   = (int)Mathf.Floor( Camera.main.orthographicSize * 2 / cellSize);
        numberOfColums = (int)Mathf.Floor((Camera.main.orthographicSize * 2 
                                                    * Camera.main.aspect ) / cellSize);

        cells = new Cell[numberOfColums, numberOfRows];

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize *
                    Camera.main.aspect,
                    y * cellSize - Camera.main.orthographicSize);
                Vector2 offsetVector = new Vector2(cellSize / 2, cellSize / 2);

                var newCell = Instantiate(cellPrefab, newPos + offsetVector, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;
                cells[x, y] = newCell.GetComponent<Cell>();

                if (Random.Range(0, 100) < spawnChancePercentage)
                {
                    cells[x, y].isAlive = true;
                }

                cells[x, y].UpdateStatus();
            }
        }
    }
    
    void Update()
    {
        Application.targetFrameRate = frameRate;

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                CheckNeighborsAndOwnCell(x, y);
                
                if (cells[x, y].isAlive)
                {
                    numberOfAliveCells--;   // Compensation for counting own cell

                    if (numberOfAliveCells < 2)
                    {
                        cells[x, y].nextLifeState = false;
                    }

                    if (numberOfAliveCells == 2 || numberOfAliveCells == 3)
                    {
                        cells[x, y].nextLifeState = true;
                    }

                    if (numberOfAliveCells > 3)
                    {
                        cells[x, y].nextLifeState = false;
                    }
                }

                else
                {
                    if (numberOfAliveCells == 3)
                    {
                        cells[x, y].nextLifeState = true;
                    }
                }

                numberOfAliveCells = 0;

                cells[x, y].UpdateStatus();
            }
        }
        
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                cells[x, y].UpdateNextLifeState();
            }
        }
    }

    void CheckNeighborsAndOwnCell(int x, int y)
    {
        CheckColumn(x - 1, y);
        CheckColumn(x + 0, y);
        CheckColumn(x + 1, y);
    }

    void CheckColumn(int x, int y)
    {
        CheckCellLifeState(x, y - 1);
        CheckCellLifeState(x, y + 0);
        CheckCellLifeState(x, y + 1);
    }

    void CheckCellLifeState(int x, int y)
    {
        // Solving edge cases
        if (y >= 0 && y < numberOfRows &&
            x >= 0 && x < numberOfColums)
        {
            if (cells[x, y].isAlive)
            {
                numberOfAliveCells++;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int frameRate;
    public int spawnChancePercentage = 15;

    float timeOfNextUpdate;
    float cellSize = 0.1f;

    Cell[,] cells;
    int numberOfColums, numberOfRows;
    int numberOfAliveNeighbors;

    public GameObject cellPrefab;
    
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
                    cells[x, y].currentLifeState = true;
                }

                cells[x, y].UpdateSpriteRenderer();
            }
        }
    }
    
    void Update()
    {
        //Application.targetFrameRate = frameRate;
        
        if (Time.time > timeOfNextUpdate)
        {
            timeOfNextUpdate = Time.time + 1;
            UpdateCells();
        }
    }

    private void UpdateCells()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                CheckNeighborsAndOwnCell(x, y);

                if (cells[x, y].currentLifeState)
                {
                    numberOfAliveNeighbors--;   // Compensation for counting own cell

                    if (numberOfAliveNeighbors < 2)
                    {
                        cells[x, y].nextLifeState = false;
                    }

                    if (numberOfAliveNeighbors == 2 || numberOfAliveNeighbors == 3)
                    {
                        cells[x, y].nextLifeState = true;
                    }

                    if (numberOfAliveNeighbors > 3)
                    {
                        cells[x, y].nextLifeState = false;
                    }
                }

                else
                {
                    if (numberOfAliveNeighbors == 3)
                    {
                        cells[x, y].nextLifeState = true;
                    }
                }

                // Reset neighbor counter for the next cell
                numberOfAliveNeighbors = 0;

                cells[x, y].UpdateSpriteRenderer();
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
        // Solves edge cases
        if (y >= 0 && y < numberOfRows &&
            x >= 0 && x < numberOfColums) 
        {
            if (cells[x, y].currentLifeState)
            {
                numberOfAliveNeighbors++;
            }
        }
    }
}
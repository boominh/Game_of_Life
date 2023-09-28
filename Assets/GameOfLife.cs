using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    Cell[,] cells;

    float cellSize = 0.1f;
    int numberOfColums, numberOfRows;
    int spawnChancePercentage = 15;

    int numberOfAliveNeighbors;


    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 4;

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
                    cells[x, y].lifeState = true;
                }

                cells[x, y].UpdateStatus();
            }
        }
    }
    
    void Update()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                CheckNeighborsAndOwnCell(x, y);
                CompensateForCheckingOwnCell(x, y);
                
                if (CurrentLifeState(x, y))
                {
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

                if (CurrentLifeState(x, y) == false)
                {
                    if (numberOfAliveNeighbors == 3)
                    {
                        cells[x, y].nextLifeState = true;
                    }
                }

                numberOfAliveNeighbors = 0;

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
        CheckNeighborLifeState(x, y - 1);
        CheckNeighborLifeState(x, y + 0);
        CheckNeighborLifeState(x, y + 1);
    }

    void CheckNeighborLifeState(int x, int y)
    {
        // Solving edge cases
        if (y >= 0 && y < numberOfRows &&
            x >= 0 && x < numberOfColums)
        {
            if (cells[x, y].lifeState == true)
            {
                numberOfAliveNeighbors++;
            }
        }
    }

    void CompensateForCheckingOwnCell(int x, int y)
    {
        if (cells[x, y].lifeState == true)
        {
            numberOfAliveNeighbors--;
        }
    }

    bool CurrentLifeState(int x, int y)
    {
        return cells[x, y].lifeState;
    }
}
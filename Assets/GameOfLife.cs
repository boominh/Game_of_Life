using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    Cell[,] cells;
    
    float cellSize = 0.25f;
    int numberOfColums, numberOfRows;
    int spawnChancePercentage = 15;

    int numberOfAliveNeighbors;


    void Start()
    {
        //Lower framerate makes it easier to test and see whats happening.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 4;

        //Calculate our grid depending on size and cellSize
        numberOfColums = (int)Mathf.Floor((Camera.main.orthographicSize *
            Camera.main.aspect * 2) / cellSize);
        numberOfRows = (int)Mathf.Floor(Camera.main.orthographicSize * 2 / cellSize);

        //Initiate our matrix array
        cells = new Cell[numberOfColums, numberOfRows];

        //Create all objects

        //For each row
        for (int y = 0; y < numberOfRows; y++)
        {
            //for each column in each row
            for (int x = 0; x < numberOfColums; x++)
            {
                //Create our game cell objects, multiply by cellSize for correct world placement
                Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize *
                    Camera.main.aspect,
                    y * cellSize - Camera.main.orthographicSize);

                var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;
                cells[x, y] = newCell.GetComponent<Cell>();

                //Random check to see if it should be alive
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

        //TODO: Calculate next generation
        // check neighbors -> make function?
        // different code depending on # of neighbors
        // edge cases
        // numberOfAliveNeighbors
        // CompensateOwnCell

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                cells[x, y].UpdateStatus();

                CheckNeighborsAndOwnCell(x, y);
                CompensateForCheckingOwnCell(x, y);
                
                if (CurrentLifeState(x, y))
                {
                    if (numberOfAliveNeighbors < 2)
                    {
                        cells[y, x].nextLifeState = false;
                    }

                    if (numberOfAliveNeighbors == 2 || numberOfAliveNeighbors == 3)
                    {
                        cells[y, x].nextLifeState = true;
                    }

                    if (numberOfAliveNeighbors > 3)
                    {
                        cells[y, x].nextLifeState = false;
                    }
                }

                if (CurrentLifeState(x, y) == false)
                {
                    if (numberOfAliveNeighbors == 3)
                    {
                        cells[y, x].nextLifeState = true;
                    }
                }

                numberOfAliveNeighbors = 0;
            }
        }
    }

    void CheckNeighborsAndOwnCell(int x, int y)
    {
        CheckColumn(x - 1, y);
        CheckColumn(x + 0, y);
        CheckColumn(x + 1, y);
    }

    void CompensateForCheckingOwnCell(int x, int y)
    {
        if (cells[y, x].lifeState == true)
        {
            numberOfAliveNeighbors--;
        }
    }

    void CheckColumn(int x, int y)
    {
        CheckNeighborLifeState(x, y - 1);
        CheckNeighborLifeState(x, y + 0);
        CheckNeighborLifeState(x, y + 1);

        //for (int yn = (y - 1); yn <= (y + 1)  ; yn++)
        //{
        //    CheckCellState(x, yn);
        //}
    }
    void CheckNeighborLifeState(int x, int y)
    {
        if (cells[x, y].lifeState == true)
        {
            numberOfAliveNeighbors++;
        }
    }

    bool CurrentLifeState(int x, int y)
    {
        return cells[x, y].lifeState;
    }
}
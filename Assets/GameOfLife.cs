using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    
    int populationCounter;
    int generation;
    List<int> populations = new List<int>();
    bool populationCountStable;
    bool printedGeneration = false;

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
            timeOfNextUpdate = Time.time + 1 / 16;
            CheckAndUpdateCells();
        }
    }

    private void CheckAndUpdateCells()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                //CheckNeighborsAndOwnCell(x, y);
                NewCheckNeighbors(x, y);

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

        CheckSimulationStability();

        UpdateNextGenLifeState();
    }

    private void CheckSimulationStability()
    {
        generation++;
        populationCounter = 0;

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                if (cells[x, y].currentLifeState)
                {
                    populationCounter++;
                }
            }
        }

        populations.Add(populationCounter);

        // Check only the 5 lateset gens
        if (populations.Count > 5)
        {
            populations.RemoveAt(0);
        }

        populationCountStable = PopulationCountStable(populations);
        
        if (populationCountStable && !printedGeneration)
        {
            Debug.Log($"Simulation now stable. Took {generation - 5}");
            printedGeneration = true;
        }
    }

    bool PopulationCountStable(List<int> populationList)
    {
        if (populationList.Count < 5)
        {
            return false;
        }

        for (int n = 0; n < populationList.Count; n++)
        {
            if (populationList[n] != populationList[0])
            {
                return false;
            }
        }

        return true;
    }
    private void UpdateNextGenLifeState()
    {
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

    void NewCheckNeighbors (int x, int y)
    {
        for (int nX = x - 1; nX <= x + 1; nX++)
        {
            for (int nY = y - 1; nY <= y + 1; nY++)
            {
                CheckCellLifeState(nX, nY);
            }
        }
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
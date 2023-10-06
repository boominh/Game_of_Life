using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TMPro;

public class GameOfLife : MonoBehaviour
{
    public int spawnChancePercentage = 15;
    public float cellSize = 0.1f;
    public GameObject cellPrefab;

    float timeOfNextUpdate;
    float timeBetweenUpdates = 1 / 100;

    Cell[,] cells;
    int numberOfColums, numberOfRows;
    int numberOfAliveNeighbors;
    
    int generation;
    int finalGeneration;
    int inhabitantCounter;
    List<int> populations = new List<int>();
    bool populationCountStable = false;

    bool printMessage = false;
    public TextMeshProUGUI textMeshPro;
    string message;
    float typingSpeed = 0.05f;
    float timer = 0f;
    int characterIndex = 0;

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
        if (Time.time > timeOfNextUpdate)
        {
            timeOfNextUpdate = Time.time + timeBetweenUpdates;
            CheckAndBufferNextLifeState();
            ApplyBufferedLifeState();
            CheckSimulationStability();
        }
    }

    void CheckAndBufferNextLifeState()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                CheckForAliveCells(x, y);

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
    }

    void CheckForAliveCells (int x, int y)
    {
        for (int xCor = x - 1; xCor <= x + 1; xCor++)
        {
            for (int yCor = y - 1; yCor <= y + 1; yCor++)
            {
                CheckCellLifeState(xCor, yCor);
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

    void ApplyBufferedLifeState()
    {
        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                cells[x, y].UpdateNextLifeState();
            }
        }
    }

    void CheckSimulationStability()
    {
        generation++;
        inhabitantCounter = 0;

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                if (cells[x, y].currentLifeState)
                {
                    inhabitantCounter++;
                }
            }
        }

        populations.Add(inhabitantCounter);

        if (populations.Count > 5)
        {
            populations.RemoveAt(0);
        }

        populationCountStable = PopulationCountStable(populations);

        if (populationCountStable)
        {
            finalGeneration = generation - 5;
            message = $"simulation now stable after {finalGeneration} generation(s).";
            printMessage = true;
            populationCountStable = false;
        }

        if (printMessage)
        {
            TypeMessage();
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

    void TypeMessage()
    {
        textMeshPro.enabled = true;
        timer += Time.unscaledDeltaTime;

        if (timer >= typingSpeed && characterIndex < message.Length)
        {
            textMeshPro.text += message[characterIndex];
            characterIndex++;
            timer = 0f;
        }
    }
}
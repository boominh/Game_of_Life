using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    float maxSpeed = 64;
    float minSpeed = 1/16;

    GameOfLife gameOfLife;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale /= 2;

            if (Time.timeScale <= minSpeed)
            {
                Time.timeScale = 0;
            }

            print($"Timescale is: {Time.timeScale}.");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.timeScale == maxSpeed)
            {
                return;
            }

            Time.timeScale = Mathf.Clamp(Time.timeScale * 2, minSpeed, maxSpeed);

            print($"Timescale is: {Time.timeScale}.");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameOfLife ??= GetComponent<GameOfLife>();

            if (gameOfLife.simulationPlay)
            {
                gameOfLife.simulationPlay = false;
                print("Simulation is paused.");
            }

            else
            {
                gameOfLife.simulationPlay = true;
                print("Simulation unpaused.");
            }
        }
    }
}

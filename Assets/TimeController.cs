using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    float previousTimeScale;

    float maxSpeed = 64;
    float minSpeed = 1/16;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.timeScale == maxSpeed)
            {
                return;
            }

            Time.timeScale /= 2;

            if (Time.timeScale <= minSpeed)
            {
                Time.timeScale = 0;
            }

            print($"Timescale is: {Time.timeScale}.");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale * 2, minSpeed, maxSpeed);

            print($"Timescale is: {Time.timeScale}.");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale != 0)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                print("Simulation is paused.");
            }

            else
            {
                Time.timeScale = previousTimeScale;
                print("Simulation unpaused.");
            }
        }
    }
}

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
            Time.timeScale /= 2;

            if (Time.timeScale <= minSpeed)
            {
                Time.timeScale = 0;
            }

            print($"timescale is: {Time.timeScale}");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale * 2, minSpeed, maxSpeed);

            print($"timescale is: {Time.timeScale}");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale != 0)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = previousTimeScale;
            }
        }
    }
}

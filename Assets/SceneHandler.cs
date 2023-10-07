using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void LoadSimulationPaused()
    {
        PlayerPrefs.SetFloat("FillPercentage", 0);
        PlayerPrefs.SetInt("SimulationPlay", 0);
        LoadSimulationScene();
    }

    public void LoadSimulationPlaying()
    {
        PlayerPrefs.SetInt("SimulationPlay", 1);
        LoadSimulationScene();
    }

    public void LoadSimulationScene()
    {
        SceneManager.LoadScene("Simulation");
    }
}

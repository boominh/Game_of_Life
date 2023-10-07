using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreen : MonoBehaviour
{
    // slider fill percentage
    // draw own picture
    // spawn acorn
    // template
    // camera

    public TextMeshProUGUI textMeshPro;
    public Slider sliderPrefab;
    int minValue = 0;
    int maxValue = 100;

    void Start()
    {
        sliderPrefab.minValue = minValue;
        sliderPrefab.maxValue = maxValue;
        sliderPrefab.value = 15;
        sliderPrefab.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(sliderPrefab.value);
    }

    public void OnSliderValueChanged(float newValue)
    {
        PlayerPrefs.SetFloat("FillPercentage", newValue);
        DisplayPercentage(newValue);
    }

    public void DisplayPercentage(float newValue)
    {
        textMeshPro.text = newValue.ToString() + "%";
    }
}

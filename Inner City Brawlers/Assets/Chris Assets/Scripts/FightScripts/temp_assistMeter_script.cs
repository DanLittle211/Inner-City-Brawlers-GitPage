using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class temp_assistMeter_script : MonoBehaviour
{
    public Slider tempAssistSlider;

    public void tempSetMaxAssist(int tempAssistMeter)
    {

        tempAssistSlider.maxValue = tempAssistMeter;
        tempAssistSlider.value = tempAssistMeter;

    }

    public void tempSetCurrentAssistMeter(int tempAssistMeter)
    {

        tempAssistSlider.value = tempAssistMeter;

    }
}

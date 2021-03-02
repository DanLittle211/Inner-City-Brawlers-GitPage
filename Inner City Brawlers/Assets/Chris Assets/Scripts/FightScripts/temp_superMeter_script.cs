using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class temp_superMeter_script : MonoBehaviour
{

    public Slider tempSuperSlider;

    public void tempSetSuperMeter(int tempSuperMeter)
    {

        tempSuperSlider.maxValue = tempSuperMeter;
        tempSuperSlider.value = tempSuperMeter;

    }

    public void tempSetCurrentSuperMeter(int tempSuperMeter)
    {

        tempSuperSlider.value = tempSuperMeter;

    }
}

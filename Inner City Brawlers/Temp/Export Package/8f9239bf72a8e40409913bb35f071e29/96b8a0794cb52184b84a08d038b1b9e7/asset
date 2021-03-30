using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class temp_health_script : MonoBehaviour
{

    public Slider tempSlider;
    public Gradient testGradient;
    public Image testFill;

    public void tempSetMaxHealth(int tempHealth) {

        tempSlider.maxValue = tempHealth;
        tempSlider.value = tempHealth;

        testFill.color = testGradient.Evaluate(1f);
    
    }

    public void tempSetCurrentHealth(int tempHealth) {

        tempSlider.value = tempHealth;

        testFill.color = testGradient.Evaluate(tempSlider.normalizedValue);
    
    }
}

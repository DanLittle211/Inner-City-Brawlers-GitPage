using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterSystem : MonoBehaviour
{
    
    //playerMeter
    public float p1MaxMeter = 4;
    public Slider p1Slider;
    public float p1CurrentMeter;
    public Gradient p1Gradient;
    public Image p1Fill;
    public bool p1MeterFull;
    //opponentMeter
    public Slider p2Slider;
    public float p2MaxMeter = 4;
    public float p2CurrentMeter;
    public Gradient p2Gradient;
    public Image p2Fill;
    public bool p2MeterFull;
    // Start is called before the first frame update
    void Start()
    {
        ResetP1Meter();
        ResetP2Meter();
    }

    public void MakeMeter(float miPoint) // miPoint = meter increase point
    {
        p1CurrentMeter += miPoint;
        SetP1Meter(p1CurrentMeter);

    }
    public void GiveMeter(float miPoint) // miPoint = meter increase point
    {
        p2CurrentMeter += miPoint;
        SetP2Meter(p2CurrentMeter);

    }

    public void SetP1MaxMeter(float MaxMeter)
    {
        p1Slider.maxValue = p1CurrentMeter;
        p1Slider.value = p1CurrentMeter;

        p1Gradient.Evaluate(1f);
    }
    public void SetP2MaxMeter(float MaxMeter)
    {
        p2Slider.maxValue = p2CurrentMeter;
        p2Slider.value = p2CurrentMeter;

        p2Gradient.Evaluate(1f);
    }

    public void SetP1Meter(float Meter)
    {
        p1Slider.value = p1CurrentMeter;
        p1Fill.color = p1Gradient.Evaluate(p1Slider.normalizedValue);
    }
    public void SetP2Meter(float Meter)
    {
        p2Slider.value = p2CurrentMeter;
        p2Fill.color = p2Gradient.Evaluate(p2Slider.normalizedValue);
    }
    public void ResetP1Meter()
    {
        p1Slider.value = 0;
        p1CurrentMeter = 0;
        p1Fill.color = p1Gradient.Evaluate(p1Slider.normalizedValue);
        p1MeterFull = false;
    }
    public void ResetP2Meter()
    {
        p2Slider.value = 0;
        p2CurrentMeter = 0;
        p2Fill.color = p2Gradient.Evaluate(p2Slider.normalizedValue);
        p2MeterFull = false;
    }

    public void SuperCharge()
    {
        //meterFull = true;
    }
    void Update()
    {
        if (p1CurrentMeter >= 4 ^ p1CurrentMeter >= p1MaxMeter)
        {
            p1MeterFull = true;
        }
        if (p2CurrentMeter >= 4 ^ p2CurrentMeter >= p2MaxMeter)
        {
            p2MeterFull = true;
        }
    }
}

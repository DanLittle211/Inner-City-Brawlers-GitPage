using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterSystem : MonoBehaviour
{
    #region Player1 variables
    //playerMeter
    public float p1MaxMeter = 4;
    public Slider p1Slider;
    public float p1CurrentMeter;
    public Gradient p1Gradient;
    public Image p1Fill;
    public bool p1MeterFull;
    #endregion
    #region Player2 variables
    //opponentMeter
    public Slider p2Slider;
    public float p2MaxMeter = 4;
    public float p2CurrentMeter;
    public Gradient p2Gradient;
    public Image p2Fill;
    public bool p2MeterFull;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        ResetP1Meter();
        ResetP2Meter();
        //starting varaibles for start of the game
    }

    public void p1MakeMeter(float miPoint) // miPoint = meter increase point
    {
        p1CurrentMeter += miPoint;
        SetP1Meter(p1CurrentMeter);
        //Formula for making meter for p1

    }
    public void p1DecreaseMeter(float miPoint) // miPoint = meter increase point
    {
        p1CurrentMeter -= miPoint;
        SetP1Meter(p1CurrentMeter);
        //Formula for decrease meter p1
    }
    public void p2MakeMeter(float miPoint) // miPoint = meter increase point
    {
        p2CurrentMeter += miPoint;
        SetP2Meter(p2CurrentMeter);
        //Formula for making meter for p2
    }
    public void p2DecreaseMeter(float miPoint) // miPoint = meter increase point
    {
        p2CurrentMeter -= miPoint;
        SetP2Meter(p2CurrentMeter);
        //Formula for making meter for p2
    }

    public void SetP1MaxMeter(float MaxMeter)
    {
        p1Slider.maxValue = p1CurrentMeter;
        p1Slider.value = p1CurrentMeter;

        p1Gradient.Evaluate(1f);
        //Sets the value of the slider equal to highest value for p1
    }
    public void SetP2MaxMeter(float MaxMeter)
    {
        p2Slider.maxValue = p2CurrentMeter;
        p2Slider.value = p2CurrentMeter;

        p2Gradient.Evaluate(1f);
        //Sets the value of the slider to its highest value for p2
    }

    public void SetP1Meter(float Meter)
    {
        p1Slider.value = p1CurrentMeter;
        p1Fill.color = p1Gradient.Evaluate(p1Slider.normalizedValue);
        //Sets the value of the slider equal to the current meter value for p1
    }
    public void SetP2Meter(float Meter)
    {
        p2Slider.value = p2CurrentMeter;
        p2Fill.color = p2Gradient.Evaluate(p2Slider.normalizedValue);
        //Sets the value of the slider equal to the current meter value for p2
    }
    public void ResetP1Meter()
    {
        p1Slider.value = 0;
        p1CurrentMeter = 0;
        p1Fill.color = p1Gradient.Evaluate(p1Slider.normalizedValue);
        p1MeterFull = false;
        //sets value of meter to 0 for p1
    }
    public void ResetP2Meter()
    {
        p2Slider.value = 0;
        p2CurrentMeter = 0;
        p2Fill.color = p2Gradient.Evaluate(p2Slider.normalizedValue);
        p2MeterFull = false;
        //sets value of meter to 0 for p2
    }

    void Update()
    {
        if (p1CurrentMeter >= 4 ^ p1CurrentMeter >= p1MaxMeter)
        {
            p1MeterFull = true;
            p1CurrentMeter = 4;
        }
        if (p2CurrentMeter >= 4 ^ p2CurrentMeter >= p2MaxMeter)
        {
            p2MeterFull = true;
            p1CurrentMeter = 4;
        }
        //locks value of meter to not exceed 4
        if (p1CurrentMeter <= 0)
        {
            p1CurrentMeter = 0;
        }
        if (p2CurrentMeter <= 0)
        {
            p2CurrentMeter = 0;
        }
        //locks value of meter to not go below 0
    }
}

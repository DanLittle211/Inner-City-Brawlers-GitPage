using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    //HealthManagementVariables
    public PlayerHealth p1Health;
    public PlayerHealth p2Health;

    public float p1HealthFloat;
    public float p2HealthFloat;

    //CalloutAssist Variables
    public float p1CAMeterfloat;
    public float p1CAMeterMaxfloat = 5f;
    public Slider p1CAMeter;
    public bool p1CaIsFull;

    public float p2CAMeterfloat;
    public float p2CAMeterMaxfloat = 5f;
    public Slider p2CAMeter;
    public bool p2CaIsFull;

    

    //Timer Variable
    public float currentTimerInt;
    public float maxTimerInt;
    public Text TimerText;

    // Start is called before the first frame update
    void Start()
    {
        p1HealthFloat = p1Health.currentHealth;
        p2HealthFloat = p2Health.currentHealth;

        SetP1CAMaxMeter(p1CAMeterMaxfloat);
        SetP2CAMaxMeter(p2CAMeterMaxfloat);

        currentTimerInt = maxTimerInt;
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckHealth();
        if (p1HealthFloat <= 0 || p2HealthFloat <= 0)
        {
            CheckHealth();
        }
    }
    void FixedUpdate()
    {
        timerTickDown();
        if (currentTimerInt <= 0)
        {
            TimerText.text = "Round Over";
        }

        if (p1CaIsFull == false || p2CaIsFull == false)
        {
            fillUp();
        }
    }

    public void CheckHealth()
    {
        if (p1HealthFloat > 0 && p2HealthFloat > 0)
        {
            if (p1HealthFloat >= p2HealthFloat)
            {
                Debug.Log("Player 1 is winning");
            }
            if (p2HealthFloat >= p1HealthFloat)
            {
                Debug.Log("Player 2 is winning");
            }
            if (p2HealthFloat == p1HealthFloat)
            {
                Debug.Log("Players are equal");
            }
        }
        else if (p1HealthFloat <= 0 || p2HealthFloat <= 0)
        {
            if (p1HealthFloat >= p2HealthFloat)
            {
                Debug.Log("Player 1 Wins!");
            }
            if (p2HealthFloat >= p1HealthFloat)
            {
                Debug.Log("Player 2 Wins!");
            }
            if (p2HealthFloat == p1HealthFloat)
            {
                Debug.Log("Draw!!");
            }
        }
    }
    //In Game timer Manager
    public void timerTickDown()
    {
        currentTimerInt -= Time.deltaTime;
        TimerText.text = "Time " + (Mathf.Round(currentTimerInt)).ToString();
    }

    public void fillUp()
    {
        p1CAMakeMeter(0.1f);
        p2CAMakeMeter(0.1f);
    }

    public void useP1CaAssist()
    {
        Debug.Log("P1 Assist Called");
        ResetP1CAMeter();
    }

    public void useP2CaAssist()
    {
        Debug.Log("P2 Assist Called");
        ResetP2CAMeter();
    }

   //Callout Assist Meter functions
    public void SetP1CAMaxMeter(float MaxMeter)
    {
        p1CAMeter.maxValue = p2CAMeterfloat;
        p1CAMeter.value = p2CAMeterfloat;

       
    }
    public void SetP2CAMaxMeter(float MaxMeter)
    {
        p2CAMeter.maxValue = p2CAMeterfloat;
        p2CAMeter.value = p2CAMeterfloat;

       
    }
    public void p1CAMakeMeter(float miPoint) // miPoint = meter increase point
    {
        p1CAMeterfloat += miPoint;
        SetP1Meter(p1CAMeterfloat);

    }
    public void p2CAMakeMeter(float miPoint) // miPoint = meter increase point
    {
        p2CAMeterfloat += miPoint;
        SetP2Meter(p2CAMeterfloat);

    }

    public void SetP1Meter(float Meter)
    {
        p1CAMeter.value = p1CAMeterfloat;
        
    }
    public void SetP2Meter(float Meter)
    {
        p2CAMeter.value = p2CAMeterfloat;
       
    }
    public void ResetP1CAMeter()
    {
        p1CAMeter.value = 0;
        p1CAMeterfloat = 0;
        p1CaIsFull = false;
    }
    public void ResetP2CAMeter()
    {
        p2CAMeter.value = 0;
        p2CAMeterfloat = 0;
        p2CaIsFull = false;
    }
}

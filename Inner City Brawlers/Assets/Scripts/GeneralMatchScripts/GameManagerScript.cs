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
    public Image p1CAFill;
    public bool p1CaIsFull;
    public Gradient p1CaGradient;

    public float p2CAMeterfloat;
    public float p2CAMeterMaxfloat = 5f;
    public Slider p2CAMeter;
    public Image p2CAFill;
    public bool p2CaIsFull;
    public Gradient p2CaGradient;

    //Timer Variable
    public float currentTimerInt;
    public float maxTimerInt;
    public Text TimerText;
    public RoundManagement rManager;


    // Start is called before the first frame update
    void Start()
    {
        p1HealthFloat = p1Health.currentHealth;
        p2HealthFloat = p2Health.currentHealth;

        p1CAMeterfloat = p1CAMeterMaxfloat;
        SetP1CaMeter(p1CAMeterfloat);

        p2CAMeterfloat = p1CAMeterMaxfloat;
        SetP2CaMeter(p1CAMeterfloat);

        currentTimerInt = maxTimerInt;
    }

    // Update is called once per frame
    void Update()
    {
        p1HealthFloat = p1Health.currentHealth;
        p2HealthFloat = p2Health.currentHealth;

       
    }
    void FixedUpdate()
    {  
        if (currentTimerInt <= 0)
        {
            TimerText.text = "Round Over";
        }
        else if (p1HealthFloat > 0 || p2HealthFloat > 0)
        {
            timerTickDown();
        }

        if (p1HealthFloat <= 0 || p2HealthFloat <= 0)
        {
            CheckWinner();
        }
        else
        {
            CheckHealth();
        }

        if (p1CAMeterfloat >= p1CAMeterMaxfloat)
        {
            p1CAMeterfloat = p1CAMeterMaxfloat;
            p1CaIsFull = true;
        }
        else
        {
            fillUp();
            p1CaIsFull = false;
        }
        if (p2CAMeterfloat >= p2CAMeterMaxfloat)
        {
            p2CAMeterfloat = p2CAMeterMaxfloat;
            p2CaIsFull = true;
        }
        else
        {
            fillUp();
            p2CaIsFull = false;
        }
    }

    public void CheckHealth()
    {
        if (p2HealthFloat == p1HealthFloat)
        {
            Debug.Log("Players are equal");
        }
        else if (p1HealthFloat >= p2HealthFloat)
        {
            Debug.Log("Player 1 is winning");
        }
        else if (p2HealthFloat >= p1HealthFloat)
        {
            Debug.Log("Player 2 is winning");
        }  
    }
    public void CheckWinner()
    {
        if (p2HealthFloat == p1HealthFloat)
        {
            Debug.Log("Draw!!");
            rManager.playersTie();
        }
        else if (p1HealthFloat >= p2HealthFloat)
        {
            Debug.Log("Player 1 Wins!");
            rManager.player1WinRound();
        }
        else if (p2HealthFloat >= p1HealthFloat)
        {
            Debug.Log("Player 2 Wins!");
            rManager.player2WinRound();
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
        p1CAMakeMeter(0.005f);
        p2CAMakeMeter(0.005f);
    }

    public void useP1CaAssist()
    {
        if (p1CaIsFull == true)
        {
            Debug.Log("P1 Assist Called");
            ResetP1CAMeter();
        }
    }

    public void useP2CaAssist()
    {
        if (p2CaIsFull == true)
        {
            Debug.Log("P2 Assist Called");
            ResetP2CAMeter();
        }
    }

   //Callout Assist Meter functions
    public void SetP1CAMaxMeter(float MaxMeter)
    {
        p1CAMeter.maxValue = p2CAMeterfloat;
        p1CAMeter.value = p2CAMeterfloat;
        p1CAFill.color = p1CaGradient.Evaluate(p1CAMeter.normalizedValue);
        p1CaGradient.Evaluate(1f);
    }
    public void SetP2CAMaxMeter(float MaxMeter)
    {
        p2CAMeter.maxValue = p2CAMeterfloat;
        p2CAMeter.value = p2CAMeterfloat;
        p2CAFill.color = p2CaGradient.Evaluate(p2CAMeter.normalizedValue);
        p2CaGradient.Evaluate(1f);
    }
    public void p1CAMakeMeter(float miPoint) // miPoint = meter increase point
    {
        p1CAMeterfloat += miPoint;
        SetP1CaMeter(p1CAMeterfloat);
    }
    public void p2CAMakeMeter(float miPoint) // miPoint = meter increase point
    {
        p2CAMeterfloat += miPoint;
        SetP2CaMeter(p2CAMeterfloat);
    }

    public void SetP1CaMeter(float Meter)
    {
        p1CAMeter.value = p1CAMeterfloat;
        p1CAFill.color = p1CaGradient.Evaluate(p1CAMeter.normalizedValue);
        p1CaGradient.Evaluate(1f);
    }
    public void SetP2CaMeter(float Meter)
    {
        p2CAMeter.value = p2CAMeterfloat;
        p2CAFill.color = p2CaGradient.Evaluate(p2CAMeter.normalizedValue);
        p2CaGradient.Evaluate(1f);
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

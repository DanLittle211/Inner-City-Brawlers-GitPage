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
    public bool hasFunctionRun;
    public enum playerState {PreRound, InGameMatch, Pause, Knockout,RoundOver,WinnerSelect};
    public playerState currentMatchState;

    // Start is called before the first frame update
    void Start()
    {
        StartMatch();
    }

    void StartMatch()
    {
        p1HealthFloat = p1Health.currentHealth;
        p2HealthFloat = p2Health.currentHealth;

        p1CAMeterfloat = p1CAMeterMaxfloat;
        SetP1CaMeter(p1CAMeterfloat);

        p2CAMeterfloat = p1CAMeterMaxfloat;
        SetP2CaMeter(p1CAMeterfloat);

        currentTimerInt = maxTimerInt;
        hasFunctionRun = false;

        currentMatchState = playerState.PreRound;

        StartCoroutine(CountDownTimer(1f));
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

        if (currentMatchState == playerState.InGameMatch)
        {
            timerTickDown();
        }

        CheckHealth();

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
        if (p2HealthFloat <= 0)
        {
            hasFunctionRun = true;
            if (hasFunctionRun == true)
            {
                //StartCoroutine(CheckResult(1f));
                
            }   
        }
        if (p1HealthFloat == 0)
        {
            hasFunctionRun = true;
            if (hasFunctionRun == true)
            {
                //StartCoroutine(CheckResult(1f));
                
            }
        }
    }

    public void CheckHealth()
    {
        if (p1HealthFloat > 0 && p2HealthFloat > 0)
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
        
    }

    IEnumerator CountDownTimer(float Time)
    {
        yield return new WaitForSeconds(0f);
        
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p1.currentPlayState = PlayerMovement.playerState.Immobile;
        p2.currentPlayState = PlayerMovement.playerState.Immobile;
        yield return new WaitForSeconds(5f);
        currentMatchState = playerState.InGameMatch;
        p1.currentPlayState = PlayerMovement.playerState.Grounded;
        p2.currentPlayState = PlayerMovement.playerState.Grounded;

 
    }
    IEnumerator CheckResult(float Time)
    {
        hasFunctionRun = false;
        yield return new WaitForSeconds(0f);
        currentMatchState = playerState.Knockout;
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p1.currentPlayState = PlayerMovement.playerState.Immobile;
        p2.currentPlayState = PlayerMovement.playerState.Immobile;
        yield return new WaitForSeconds(1f);
        currentMatchState = playerState.WinnerSelect;

        if (currentMatchState == playerState.WinnerSelect)
        {
            if (p2HealthFloat == p1HealthFloat)
            {
                Debug.Log(" Player Tie ! Current Standings: Player 1 " + rManager.Player1currentRWInt + " to Player 2 " + rManager.Player2currentRWInt);
                StartCoroutine(ResetMatch(0f));
                currentMatchState = playerState.RoundOver;
            }
            else if (p1HealthFloat >= p2HealthFloat)
            {
                GameObject player2H = GameObject.Find("Player2");
                PlayerHealth p2H = (PlayerHealth)player2H.GetComponent(typeof(PlayerHealth));
                rManager.Player1currentRWInt += 1;
                rManager.CurrentRoundInt += 1;
                Debug.Log(" Player 1 Wins ! Current Standings: Player 1 " + rManager.Player1currentRWInt + " to Player 2 " + rManager.Player2currentRWInt);
                p2H.currentHealth = -1;
                currentMatchState = playerState.RoundOver;
                StartCoroutine(ResetMatch(0f));
            }
            else if (p2HealthFloat >= p1HealthFloat)
            {
                GameObject player1H = GameObject.Find("Player1");
                PlayerHealth p1H = (PlayerHealth)player1H.GetComponent(typeof(PlayerHealth));
                Debug.Log(" Player 2 Wins ! Current Standings: Player 1 " + rManager.Player1currentRWInt + " to Player 2 " + rManager.Player2currentRWInt);
                rManager.Player2currentRWInt += 1;
                rManager.CurrentRoundInt += 1;
                p1H.currentHealth = -1;
                currentMatchState = playerState.RoundOver;
                StartCoroutine(ResetMatch(0f));
            }

        }
        yield return new WaitForSeconds(5f);
        currentMatchState = playerState.RoundOver;
    }
    IEnumerator ResetMatch(float Time)
    {
        if (currentMatchState == playerState.RoundOver)
        {
            Debug.Log("Match Resetting...");
            yield return new WaitForSeconds(5f);
            //currentMatchState = playerState.InGameMatch;
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

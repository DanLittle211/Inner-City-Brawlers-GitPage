﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    #region Script References
    [Header("Training Variables/Tabs")]
    public GameMasterManager gMM;
    public TrainingPauseManager tPM;
    public MeterSystem mS;
    [SerializeField]private CallOutAssist cOA;
    public RoundManager rM;
    #endregion

    #region Transforms
    [Header("Transform Starter Positions")]
    [SerializeField] public Transform p1Transform;
    [SerializeField] public Transform p2Transform;
    public Transform p1StartPosition;
    public Transform p2StartPosition;
    public Transform fightCam;
    public Transform CenterPoint;
    #endregion

    #region General Player variables
    [Header("Player's Health")]
    public PlayerHealth p1Health;
    public PlayerHealth p2Health;
    public PlayerMovement p1M;
    public PlayerMovement p2M;
    public float p1HealthFloat;
    public float p2HealthFloat;
    #endregion

    #region P1 Variables
    [Header("Player 1's Meter Variables")]
    public float p1CAMeterfloat;
    public float p1CAMeterMaxfloat = 5f;
    public Slider p1CAMeter;
    public Image p1CAFill;
    public bool p1CaIsFull;
    public Gradient p1CaGradient;
    #endregion

    #region P2 Variables
    [Header("Player 2's Meter Variables")]
    public float p2CAMeterfloat;
    public float p2CAMeterMaxfloat = 5f;
    public Slider p2CAMeter;
    public Image p2CAFill;
    public bool p2CaIsFull;
    public Gradient p2CaGradient;
    #endregion

    #region Pause/Time Variables
    [Header("Pause/Time Variables")]
    [SerializeField] public bool isPaused;
    public float currentTimerInt;
    public float maxTimerInt;
    public TMP_Text TimerText;
    public bool TimeStart;
    #endregion

    #region ComboCounter Variables
    [Header("ComboCounter Variables")]
    public int p1comboCounter;
    public TMP_Text p1comboCounterText;
    public int p2comboCounter;
    public TMP_Text p2comboCounterText;
    public float comboLeewayTimer;
    #endregion

    #region Manager Bool
    [Header("Game Manager Variables")]
    public bool hasFunctionRun;
    #endregion

    // Start is called before the first frame update
    public void Start()
    {
        cOA = this.GetComponent<CallOutAssist>();
        rM.Awake();
        tPM.SetinactiveState();
        p1comboCounter = 0;
        p2comboCounter = 0;
        comboLeewayTimer = 0;
        cOA.p1Timer = 0;
        cOA.p2Timer = 0;
        SetHitCounter(p1comboCounterText, p1comboCounter);
        SetHitCounter(p2comboCounterText, p2comboCounter);

        p1Health.currentHealth = p1Health.maxHealth;
        p1Health.SetHealth(p1Health.maxHealth);

        p2Health.currentHealth = p2Health.maxHealth;
        p2Health.SetHealth(p2Health.maxHealth);

        mS.p2CurrentMeter = 0;
        mS.SetP2Meter(mS.p2CurrentMeter);
        mS.p1CurrentMeter = 0;
        mS.SetP1Meter(mS.p1CurrentMeter);
        //resets variables to starting values
    }

    public void SetHitCounter(TMP_Text text,int Hitcount)
    {
        text.SetText(Hitcount.ToString() + " HITS!");
        //Sets value of combo counter to current hit value
    }
    public void LockStartPosition()
    {
        p1Transform.transform.position = p1StartPosition.transform.position;
        p2Transform.transform.position = p2StartPosition.transform.position;
        fightCam.transform.position = CenterPoint.transform.position;
        ImmobilizePlayer();
        //Freezes the camera to the center point to the screen and players to the center of the screen at the start of the game
    }
    void ImmobilizePlayer()
    {
        Debug.Log("Immobilized Player");
        p1M.isDisabled = true;
        p2M.isDisabled = true;
        //freezes characters in place
    }

    public void StartMatch()
    {
        if (gMM.isMultiActive == true)
        {
            rM.GameConcludedPanel.SetActive(false);
            currentTimerInt = rM.roundTimer;
            cOA.p1Timer = 0;
            cOA.p2Timer = 0;
            LockStartPosition();

            p1Health.currentHealth = p1Health.maxHealth;
            p1Health.SetHealth(p1Health.maxHealth);
            p2Health.currentHealth = p2Health.maxHealth;
            p2Health.SetHealth(p2Health.maxHealth);

            mS.ResetP1Meter();
            mS.ResetP2Meter();

            TimeStart = false;
            hasFunctionRun = false;
            tPM.SetinactiveState();
            StartCoroutine(CountDownVersusTimer(1f));

            Debug.Log("ResetGameMode");
            //checks if you're in multiplayer and sets up multiplayer mode with timer and round system
        }
        if (gMM.isMultiActive == false)
        {
            p1Health.currentHealth = p1Health.maxHealth;
            p1Health.SetHealth(p1Health.maxHealth);
            p2Health.currentHealth = p2Health.maxHealth;
            p2Health.SetHealth(p2Health.maxHealth);
            cOA.p1Timer = 0;
            cOA.p2Timer = 0;
            trainingStart();
            //checks if you're in single player, makes the timer disabled and puts you in single player mode
        }
    }
    public void trainingStart()
    {
        StartCoroutine(CountDownTrainingTimer(1f));
        currentTimerInt = maxTimerInt;
        TimerText.text = "Time " + " ∞";
        ResetMode();
        tPM.SetinactiveState();
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        cOA.p1Timer = 0f;
        cOA.p2Timer = 0f;
        //Starts training mode by disabling player 2, settings necessary values to 0, and disabling the timer and round system
    }
    public void ResetMode()
    {
        p1HealthFloat = p1Health.maxHealth;
        p1Health.SetHealth(p1Health.maxHealth);
        p2HealthFloat = p2Health.maxHealth;
        p2Health.SetHealth(p2Health.maxHealth);

        mS.ResetP1Meter();
        mS.ResetP2Meter();
        LockStartPosition();
        p1CAMeterfloat = p1CAMeterMaxfloat;
        SetP1CaMeter(p1CAMeterfloat);

        TimeStart = false;
        p2CAMeterfloat = p2CAMeterMaxfloat;
        SetP2CaMeter(p2CAMeterfloat);
        hasFunctionRun = false;        
        //resets entire gamemode, values, player positions and necessary values
    }
    IEnumerator CountDownTrainingTimer(float Time)
    {
        yield return new WaitForSeconds(0f);
        //p1.groundCheck.SetActive(true);
        p1M.currentPlayState = PlayerMovement.playerState.Grounded;
        //p2.groundCheck.SetActive(true);
        p2M.currentPlayState = PlayerMovement.playerState.Grounded;
    }
    IEnumerator CountDownVersusTimer(float Time)
    {
        yield return new WaitForSeconds(0f);
        //p1.groundCheck.SetActive(true);
        p1M.currentPlayState = PlayerMovement.playerState.Grounded;
        //p2.groundCheck.SetActive(true);
        p2M.currentPlayState = PlayerMovement.playerState.Grounded;
        unlock();
    }
    void unlock()
    {
        Debug.Log("Unlocked Players");
        p1M.isDisabled = false;
        p2M.isDisabled = false;
        //enables players to move and attack
    }

    // Update is called once per frame
    void Update()
    {
        p1HealthFloat = p1Health.currentHealth;
        p2HealthFloat = p2Health.currentHealth;
        SetHitCounter(p1comboCounterText, p1comboCounter);
        SetHitCounter(p2comboCounterText, p2comboCounter);
        //continues to set value of combo counter text equal to the combo counter value
        if (comboLeewayTimer > -0.1)
        {
            comboLeewayTimer-= Time.deltaTime;
            //as the combo counter timer is higher than 0, it will continue to count down
        }
        if (comboLeewayTimer <= 0)
        {
            ResetcomboValues();
            //if less than or equal to 0, it resets to combo values
        }
        if (p1comboCounter <= 1)
        {
            p1comboCounterText.gameObject.SetActive(false);
            //if the combo counter for this character is less than or equal to 1 it goes away
        }
        
        else
        {
            p1comboCounterText.gameObject.SetActive(true);
            //sets combo counter active if it's value is higher than 1
        }
        if (p2comboCounter <= 1)
        {
            p2comboCounterText.gameObject.SetActive(false);
        }
        else
        {
            p2comboCounterText.gameObject.SetActive(true);
        }
        //same for player2
    }
  
    void FixedUpdate()
    {
        if (p1CAMeterfloat >= p1CAMeterMaxfloat)
        {
            p1CAMeterfloat = p1CAMeterMaxfloat;
            p1CaIsFull = true;
        }
        else
        {
            //fillUp();
            p1CaIsFull = false;
        }
        if (p2CAMeterfloat >= p2CAMeterMaxfloat)
        {
            p2CAMeterfloat = p2CAMeterMaxfloat;
            p2CaIsFull = true;
        }
        else
        {
            //fillUp();
            p2CaIsFull = false;
        }
        //monitors Callout assist values and checks current state and max value
    }

    public void ResetcomboValues()
    {
        comboLeewayTimer = 0;
        p1comboCounter = 0;
        p2comboCounter = 0;
        SetHitCounter(p1comboCounterText, p1comboCounter);
        SetHitCounter(p2comboCounterText, p2comboCounter);
        //resets combo counter text and values
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
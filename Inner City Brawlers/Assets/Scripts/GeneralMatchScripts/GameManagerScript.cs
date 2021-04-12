using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    [Header("Training Variables/Tabs")]
    public GameMasterManager gMM;
    public TrainingPauseManager tPM;
    public MeterSystem mS;
    public CallOutAssist cOA;
    public RoundManager rM;

    [Header("Transform Starter Positions")]
    [SerializeField] public Transform p1Transform;
    [SerializeField] public Transform p2Transform;
    public Transform p1StartPosition;
    public Transform p2StartPosition;
    public Transform fightCam;
    public Transform CenterPoint;

    [Header("Player's Health")]
    public PlayerHealth p1Health;
    public PlayerHealth p2Health;

    public float p1HealthFloat;
    public float p2HealthFloat;

    [Header("Player 1's Meter Variables")]
    public float p1CAMeterfloat;
    public float p1CAMeterMaxfloat = 5f;
    public Slider p1CAMeter;
    public Image p1CAFill;
    public bool p1CaIsFull;
    public Gradient p1CaGradient;

    [Header("Player 2's Meter Variables")]
    public float p2CAMeterfloat;
    public float p2CAMeterMaxfloat = 5f;
    public Slider p2CAMeter;
    public Image p2CAFill;
    public bool p2CaIsFull;
    public Gradient p2CaGradient;

    [Header("Pause Variables")]
    [SerializeField] public bool isPaused;


    [Header("Timer Variables")]
    public float currentTimerInt;
    public float maxTimerInt;
    public TMP_Text TimerText;
    public bool TimeStart;
    [Header("ComboCounter Variables")]
    public int p1comboCounter;
    public TMP_Text p1comboCounterText;
    public int p2comboCounter;
    public TMP_Text p2comboCounterText;
    public float comboLeewayTimer;

    [Header("Game Manager Variables")]
    public bool hasFunctionRun;

    // Start is called before the first frame update
    public void Start()
    {
        rM.Awake();
        tPM.SetinactiveState();
        p1comboCounter = 0;
        p2comboCounter = 0;
        comboLeewayTimer = 0;
        SetHitCounter(p1comboCounterText, p1comboCounter);
        SetHitCounter(p2comboCounterText, p2comboCounter);
    }

    public void SetHitCounter(TMP_Text text,int Hitcount)
    {
        text.SetText(Hitcount.ToString() + " HITS!");
    }
    public void LockStartPosition()
    {
        p1Transform.transform.position = p1StartPosition.transform.position;
        p2Transform.transform.position = p2StartPosition.transform.position;
        fightCam.transform.position = CenterPoint.transform.position;
        ImmobilizePlayer();
    }
    void ImmobilizePlayer()
    {
        Debug.Log("Immobilized Player");
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p1.isDisabled = true;
        p2.isDisabled = true;
    }

    public void StartMatch()
    {
        if (gMM.isMultiActive == true)
        {
            LockStartPosition();
            currentTimerInt = rM.roundTimer;

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
            cOA.p1Timer = 0f;
            cOA.p2Timer = 0f;
            
            Debug.Log("ResetGameMode");
        }
        if (gMM.isMultiActive == false)
        {

            trainingStart();
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
    }
    IEnumerator CountDownTrainingTimer(float Time)
    {
        yield return new WaitForSeconds(0f);
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p1.groundCheck.SetActive(true);
        p1.currentPlayState = PlayerMovement.playerState.Grounded;
        p2.groundCheck.SetActive(true);
        p2.currentPlayState = PlayerMovement.playerState.Grounded;
    }
    IEnumerator CountDownVersusTimer(float Time)
    {
        yield return new WaitForSeconds(0f);
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        yield return new WaitForSeconds(5f);
        p1.groundCheck.SetActive(true);
        p1.currentPlayState = PlayerMovement.playerState.Grounded;
        p2.groundCheck.SetActive(true);
        p2.currentPlayState = PlayerMovement.playerState.Grounded;
        unlock();
    }
    void unlock()
    {
        Debug.Log("Unlocked Players");
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        PlayerMovement p1 = (PlayerMovement)player1.GetComponent(typeof(PlayerMovement));
        PlayerMovement p2 = (PlayerMovement)player2.GetComponent(typeof(PlayerMovement));
        p1.isDisabled = false;
        p2.isDisabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        p1HealthFloat = p1Health.currentHealth;
        p2HealthFloat = p2Health.currentHealth;
        SetHitCounter(p1comboCounterText, p1comboCounter);
        SetHitCounter(p2comboCounterText, p2comboCounter);
        if (comboLeewayTimer > 0.1)
        {
            comboLeewayTimer-= Time.deltaTime;

        }
        if (comboLeewayTimer <= 0)
        {
            ResetcomboValues();
        }
        if (p1comboCounter <= 1)
        {
            p1comboCounterText.gameObject.SetActive(false);
        }
        
        else
        {
            p1comboCounterText.gameObject.SetActive(true);
        }
        if (p2comboCounter <= 1)
        {
            p2comboCounterText.gameObject.SetActive(false);
        }
        else
        {
            p2comboCounterText.gameObject.SetActive(true);
        }
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
    }

    /*public void CheckHealth()
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
    }*/
    public void ResetcomboValues()
    {
        comboLeewayTimer = 0;
        p1comboCounter = 0;
        p2comboCounter = 0;
        /*SetHitCounter(p1comboCounterText, p1comboCounter);
        SetHitCounter(p2comboCounterText, p2comboCounter);*/
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

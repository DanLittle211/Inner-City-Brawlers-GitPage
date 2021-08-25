using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    #region Floats
    [SerializeField] public float damageNumber;
    [HideInInspector]
    [SerializeField] private float stunNumber;
    [SerializeField] private float scalingFactor;
    [HideInInspector]
    [SerializeField] private float scalingNumber;
    [HideInInspector]
    [SerializeField] public float gMeterNumber;
    [HideInInspector]
    [SerializeField] public float rMeterNumber;
    [SerializeField] public float xForce, yForce;
    #endregion
    #region Script references
    private PlayerMovement p1M, p2M;
    private GameManagerScript gM;
    #endregion

    void Start()
    {
        p1M = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        p2M = GameObject.Find("Player2").GetComponent<PlayerMovement>();
        gM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        stunNumber = 2;
        scalingNumber = 1;
        //starting values in when script is called or referenced
    }

    private void Update()
    {
        if (damageNumber < 1)
        {
            damageNumber = 1;
            //resets value of damage number in case it reaches too low of a value
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player1")
        {
            if (p1M.currentPlayState == PlayerMovement.playerState.HB || p1M.currentPlayState == PlayerMovement.playerState.LB)
            {
                KnockBack(xForce, yForce, p1M.myRB2D);
                FindObjectOfType<SoundManager>().Play("Attack");
                //Knocks opponent away from the attacker
            }
            else
            {
                GameObject playerHealth = GameObject.Find("Player1");
                PlayerHealth pH = (PlayerHealth)playerHealth.GetComponentInChildren(typeof(PlayerHealth));
                pH.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));
                Debug.Log("Current Damage & Scaling Number: " + damageNumber + ", " + scalingNumber);
                KnockBack(xForce, yForce, p1M.myRB2D);
                gM.p2comboCounter++;
                gM.comboLeewayTimer = (stunNumber * scalingNumber);
                scalingNumber -= scalingFactor;
                Debug.Log("Current stun number: " + stunNumber);
                gM.SetHitCounter(gM.p2comboCounterText, gM.p2comboCounter);
                p1MeterDealing();
                Debug.Log("Hit hitbox");
                this.GetComponentInParent<SoundManager>().Play("Damage");
                this.GetComponentInParent<SoundManager>().Play("Attack");
                //When attacking "Player 1" tag, it finds health, deals damage based on damage value and scaling
                //when hit number increases, the scaling increases thereby decreasing the damage while attacking
                //When being attacked, if the scaling value reaches a certain point, the combo is reset and so is scaling
            }

            if (gM.p1comboCounter > 0 )
            {
                //ResetScale();
            }
            if (gM.comboLeewayTimer <= 0)
            {
                Debug.Log("Ran Scaling Values");
                ResetScale();
            }
            
        }

        if (col.tag == "Player2")
        {
            if (p2M.currentPlayState == PlayerMovement.playerState.HB ^ p2M.currentPlayState == PlayerMovement.playerState.LB)
            {
                KnockBack(xForce, yForce, p2M.myRB2D);
                FindObjectOfType<SoundManager>().Play("Attack");
                //Knocks opponent away from the attacker
            }
            else
            {
                GameObject playerHealth = GameObject.Find("Player2");
                PlayerHealth pH = (PlayerHealth)playerHealth.GetComponentInChildren(typeof(PlayerHealth));
                pH.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));
                p2MeterDealing();
                gM.p1comboCounter++;
                gM.comboLeewayTimer = (stunNumber * scalingNumber);
                scalingNumber -= scalingFactor;
                Debug.Log("Current stun number: " + stunNumber);
                gM.SetHitCounter(gM.p1comboCounterText, gM.p1comboCounter);
                KnockBack(xForce, yForce, p2M.myRB2D);
                Debug.Log("Hit hitbox");
                this.GetComponentInParent<SoundManager>().Play("Damage");
                this.GetComponentInParent<SoundManager>().Play("Attack");
                //When attacking "Player 2" tag, it finds health, deals damage based on damage value and scaling
                //when hit number increases, the scaling increases thereby decreasing the damage while attacking
                //When being attacked, if the scaling value reaches a certain point, the combo is reset and so is scaling
            }
            if (gM.p2comboCounter > 0)
            {
                //ResetScale();
            }
            if (gM.comboLeewayTimer <= 0)
            {
                Debug.Log("Ran Scaling Values");
                ResetScale();
            }
        }
    }
   public void ResetScale()
    {
        scalingNumber = 1;
        gM.p1comboCounter = 0;
        gM.p2comboCounter = 0;
        gM.SetHitCounter(gM.p1comboCounterText, gM.p1comboCounter);
        gM.SetHitCounter(gM.p2comboCounterText, gM.p2comboCounter);
        Debug.Log("Ran Scaling Values");

        //resets the value of scaling value and combo counter
    }
    void p1MeterDealing()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
        mS.p1MakeMeter(gMeterNumber);
        mS.p2MakeMeter(rMeterNumber);
        Debug.Log("p1 Dealt Meter");
        // gives meter to both characters based on when they're attacking and being attacked
    }
    void p2MeterDealing()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
        mS.p1MakeMeter(rMeterNumber);
        mS.p2MakeMeter(gMeterNumber);
        Debug.Log("p2 Dealt Meter");
        // gives meter to both characters based on when they're attacking and being attacked
    }

    public void KnockBack(float knockbackForceX, float knockbackForceY, Rigidbody2D playerRB)
    {
        DirectionLook dLook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
        if (dLook.isFlipped == true)
        {
            playerRB.AddRelativeForce(Vector3.right * knockbackForceX, ForceMode2D.Impulse);
        }
        if (dLook.isFlipped == false)
        {
            playerRB.AddRelativeForce(Vector3.left * knockbackForceX, ForceMode2D.Impulse);
        }
        playerRB.AddRelativeForce(Vector3.up * knockbackForceY, ForceMode2D.Impulse);
        //formula for knocking back enemies away from attacker 
    }
}

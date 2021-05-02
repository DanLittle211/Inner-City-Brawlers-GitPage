using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAttack : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private PlayerMovement p1M;
    [SerializeField] private PlayerMovement p2M;
    [SerializeField] private GameManagerScript gM;
    [SerializeField] private MeterSystem mS;

    [Header("Damage Values")]
    [SerializeField] private float damageNumber;
    [HideInInspector]
    [SerializeField] private float stunNumber;
    [SerializeField] private float scalingFactor;
    [HideInInspector]
    [SerializeField] private float scalingNumber;
    [HideInInspector]
    [SerializeField] private float gMeterNumber;
    [HideInInspector]
    [SerializeField] private float rMeterNumber;
    [SerializeField] public float xForce, yForce;

    void Start()
    {
        mS = GameObject.Find("GameManager").GetComponent<MeterSystem>();
        p1M = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        p2M = GameObject.Find("Player2").GetComponent<PlayerMovement>();
        gM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        stunNumber = 2;
        scalingNumber = 1;
    }

    public void AttackPlayer(PlayerHealth targetHealth, PlayerMovement targetRB2D)//, int targetComboCounter)
    {

        if (this.gameObject.tag == "Player2")
        {
            if (p2M.currentPlayState == PlayerMovement.playerState.HB ^ p2M.currentPlayState == PlayerMovement.playerState.LB)
            {
                KnockBack(xForce, yForce, targetRB2D.myRB2D); //Knocks back chose player
                this.GetComponentInParent<SoundManager>().Play("Attack");
            }
            else
            { 
                Debug.Log(gameObject.tag + p2M.currentPlayState);
                KnockBack(xForce, yForce, targetRB2D.myRB2D); //Knocks back chose player
                targetHealth.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));  //damages player and scales damage number
                                                                                   //targetComboCounter++;  //increases combo counter for attacking player
                gM.comboLeewayTimer = (stunNumber * scalingNumber); // sets combo leeway timer to the stun timer and scales it
                scalingNumber -= scalingFactor; // decreases scaling values by the scaling factor for each hit.
                Debug.Log("Current stun number: " + stunNumber);
                Debug.Log("Current Damage & Scaling Number: " + damageNumber + ", " + scalingNumber);
                this.GetComponentInParent<SoundManager>().Play("Damage");
                p2MeterDealing();
                gM.p1comboCounter++;
                gM.SetHitCounter(gM.p1comboCounterText, gM.p1comboCounter);
                this.GetComponentInParent<SoundManager>().Play("Attack");
            }
        }
        if (this.gameObject.tag == "Player1")
        {
            if (p1M.currentPlayState == PlayerMovement.playerState.HB || p1M.currentPlayState == PlayerMovement.playerState.LB)
            {
                KnockBack(xForce, yForce, targetRB2D.myRB2D); //Knocks back chose player
                this.GetComponentInParent<SoundManager>().Play("Attack");
            }
            else
            { 
                Debug.Log(gameObject.tag + p1M.currentPlayState);
                KnockBack(xForce, yForce, targetRB2D.myRB2D); //Knocks back chose player
                targetHealth.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));  //damages player and scales damage number
                                                                                   //targetComboCounter++;  //increases combo counter for attacking player
                gM.comboLeewayTimer = (stunNumber * scalingNumber); // sets combo leeway timer to the stun timer and scales it
                scalingNumber -= scalingFactor; // decreases scaling values by the scaling factor for each hit.
                Debug.Log("Current stun number: " + stunNumber);
                Debug.Log("Current Damage & Scaling Number: " + damageNumber + ", " + scalingNumber);
                this.GetComponentInParent<SoundManager>().Play("Damage");
                p1MeterDealing();
                gM.p2comboCounter++;
                gM.SetHitCounter(gM.p2comboCounterText, gM.p2comboCounter);
                this.GetComponentInParent<SoundManager>().Play("Attack");
            }
        }
        if (gM.comboLeewayTimer <= 0)
        {
            ResetScale();
        }

        Debug.Log("Hit hitbox");

    }

    public void ResetScale()
    {
        scalingNumber = 1;
        gM.p1comboCounter = 0;
        gM.p2comboCounter = 0;
        gM.SetHitCounter(gM.p1comboCounterText, gM.p1comboCounter);
        gM.SetHitCounter(gM.p2comboCounterText, gM.p2comboCounter);
        Debug.Log("Ran Scaling Values");
    }
    public void p1MeterDealing()
    {
        mS.p1MakeMeter(gMeterNumber);
        mS.p2MakeMeter(rMeterNumber);
    }
    public void p2MeterDealing()
    {
        mS.p1MakeMeter(rMeterNumber);
        mS.p2MakeMeter(gMeterNumber);
    }
    public void KnockBack(float knockbackForceX, float knockbackForceY, Rigidbody2D playerRB)
    {
        DirectionLook dLook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
        if (dLook.isFlipped == false)
        {
            playerRB.AddRelativeForce(Vector3.right * knockbackForceX, ForceMode2D.Impulse);
        }
        if (dLook.isFlipped == true)
        {
            playerRB.AddRelativeForce(Vector3.left * knockbackForceX, ForceMode2D.Impulse);
        }
        playerRB.AddRelativeForce(Vector3.up * knockbackForceY, ForceMode2D.Impulse);
    }
}

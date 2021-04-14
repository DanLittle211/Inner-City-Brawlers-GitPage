using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAttack : MonoBehaviour
{
    [Header("Script References")]
    [HideInInspector]
    [SerializeField] private PlayerMovement p1M, p2M;
    [SerializeField] private GameManagerScript gM;

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
        p1M = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        p2M = GameObject.Find("Player2").GetComponent<PlayerMovement>();
        gM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        stunNumber = 2;
        scalingNumber = 1;
    }

    public void AttackPlayer(PlayerHealth targetHealth, PlayerMovement targetRB2D, int targetComboCounter)
    {
        targetHealth.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));
        //pH.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));
        Debug.Log("Current Damage & Scaling Number: " + damageNumber + ", " + scalingNumber);
        KnockBack(xForce, yForce, targetRB2D.myRB2D);
        //KnockBack(xForce, yForce, p1M.myRB2D);
        targetComboCounter++;
        //gM.p2comboCounter++;
        gM.comboLeewayTimer = (stunNumber * scalingNumber);
        scalingNumber -= scalingFactor;
        Debug.Log("Current stun number: " + stunNumber);
        gM.SetHitCounter(gM.p2comboCounterText, gM.p2comboCounter);
        /*if(thisPlayer <= 1)
        {
            p1MeterDealing();
        }
        if (thisPlayer >= 2)
        {
            p2MeterDealing();
        }*/
        
        Debug.Log("Hit hitbox");


        if (targetComboCounter > 0)
        {
            //ResetScale();
        }
        if (gM.comboLeewayTimer <= 0)
        {
            ResetScale();
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
    }
    public void p1MeterDealing()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
        mS.p1MakeMeter(gMeterNumber);
        mS.p2MakeMeter(rMeterNumber);
        Debug.Log("p1 Dealt Meter");
    }
    public void p2MeterDealing()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
        mS.p1MakeMeter(rMeterNumber);
        mS.p2MakeMeter(gMeterNumber);
        Debug.Log("p2 Dealt Meter");
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

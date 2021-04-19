using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
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
    private PlayerMovement p1M, p2M;
    private GameManagerScript gM;

    void Start()
    {
        p1M = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        p2M = GameObject.Find("Player2").GetComponent<PlayerMovement>();
        gM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        stunNumber = 2;
        scalingNumber = 1;
    }

    private void Update()
    {
       
        if (damageNumber < 1)
        {
            damageNumber = 1;
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player1")
        {
            GameObject playerHealth = GameObject.Find("Player1");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponentInChildren(typeof(PlayerHealth));
            pH.TakeDamage(damageNumber * Mathf.Abs(scalingNumber));
            Debug.Log("Current Damage & Scaling Number: " + damageNumber + ", " + scalingNumber);
            KnockBack(xForce,yForce, p1M.myRB2D);
            gM.p2comboCounter++;
            gM.comboLeewayTimer = (stunNumber * scalingNumber);
            scalingNumber -= scalingFactor;
            Debug.Log("Current stun number: " + stunNumber);
            gM.SetHitCounter(gM.p2comboCounterText, gM.p2comboCounter);
            p1MeterDealing();
            Debug.Log("Hit hitbox");

            if (gM.p1comboCounter > 0 )
            {
                //ResetScale();
            }
            if (gM.comboLeewayTimer <= 0)
            {
                ResetScale();
            }
            
        }

        if (col.tag == "Player2")
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
            if (gM.p2comboCounter > 0)
            {
                //ResetScale();
            }
            if (gM.comboLeewayTimer <= 0)
            {
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
    }
    void p1MeterDealing()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
        mS.p1MakeMeter(gMeterNumber);
        mS.p2MakeMeter(rMeterNumber);
        Debug.Log("p1 Dealt Meter");
    }
    void p2MeterDealing()
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
        if (dLook.isFlipped == true)
        {
            playerRB.AddRelativeForce(Vector3.right * knockbackForceX, ForceMode2D.Impulse);
        }
        if (dLook.isFlipped == false)
        {
            playerRB.AddRelativeForce(Vector3.left * knockbackForceX, ForceMode2D.Impulse);
        }
        playerRB.AddRelativeForce(Vector3.up * knockbackForceY, ForceMode2D.Impulse);
    }
}

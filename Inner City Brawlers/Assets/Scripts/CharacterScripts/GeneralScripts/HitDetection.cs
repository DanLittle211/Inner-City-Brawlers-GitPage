using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] public float damageNumber;
    [SerializeField] public float gMeterNumber;
    [SerializeField] public float rMeterNumber;
    [SerializeField] public float xForce, yForce;
    private PlayerMovement p1M, p2M;

    void Start()
    {
        p1M = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        p2M = GameObject.Find("Player2").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player1")
        {
            GameObject playerHealth = GameObject.Find("Player1");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponent(typeof(PlayerHealth));
            pH.TakeDamage(damageNumber);
            KnockBack(xForce,yForce, p1M.myRB2D);
            p1MeterDealing();
            Debug.Log("Hit hitbox");
        }

        if (col.tag == "Player2")
        {
            GameObject playerHealth = GameObject.Find("Player2");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponent(typeof(PlayerHealth));
            pH.TakeDamage(damageNumber);
            p2MeterDealing();
            KnockBack(xForce, yForce, p2M.myRB2D);
            Debug.Log("Hit hitbox");
        }
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

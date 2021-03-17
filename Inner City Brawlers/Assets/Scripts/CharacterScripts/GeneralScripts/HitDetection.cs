using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] public float damageNumber;
    [SerializeField] public float gMeterNumber;
    [SerializeField] public float rMeterNumber;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player1")
        {
            GameObject playerHealth = GameObject.Find("Player1");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponent(typeof(PlayerHealth));
            pH.TakeDamage(damageNumber);

            p1MeterDealing();
            Debug.Log("Hit hitbox");
        }

        if (col.tag == "Player2")
        {
            GameObject playerHealth = GameObject.Find("Player2");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponent(typeof(PlayerHealth));
            pH.TakeDamage(damageNumber);
            p2MeterDealing();
            
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
}
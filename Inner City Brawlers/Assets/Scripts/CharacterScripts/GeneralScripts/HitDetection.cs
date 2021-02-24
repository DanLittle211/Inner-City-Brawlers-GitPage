using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player1")
        {
            GameObject playerHealth = GameObject.Find("P1");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponent(typeof(PlayerHealth));
            pH.TakeDamage(20f);

            GameObject gameManager = GameObject.Find("GameManager");
            MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
            mS.p1MakeMeter(0.1f);
            mS.p2MakeMeter(0.3f);

            Debug.Log("Hit hitbox");
        }

        if (col.tag == "Player2")
        {
            GameObject playerHealth = GameObject.Find("P2");
            PlayerHealth pH = (PlayerHealth)playerHealth.GetComponent(typeof(PlayerHealth));
            pH.TakeDamage(20f);

            GameObject gameManager = GameObject.Find("GameManager");
            MeterSystem mS = (MeterSystem)gameManager.GetComponent(typeof(MeterSystem));
            mS.p1MakeMeter(0.3f);
            mS.p2MakeMeter(0.1f);
            Debug.Log("Hit hitbox");
        }
    }
}

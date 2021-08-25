using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider slider;
    public float maxHealth = 500;
    public float currentHealth;
    public float currentRecovHealth;
    public Gradient gradient;
    public Image Fill;
    public MeterSystem meterSystem;
    public Image blueHealthFill;
    [SerializeField] private PlayerMovement pM;
    // Start is called before the first frame update
    void Start()
    {
        pM = this.GetComponentInParent<PlayerMovement>();
        currentHealth = maxHealth;
        currentRecovHealth = maxHealth;
        //at the beginning of the game, sets the health to the max value
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        gradient.Evaluate(1f);
        //sets health to max value
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        Fill.color = gradient.Evaluate(slider.normalizedValue);
        //sets health to current value
    }

    public void TakeDamage(float damage)
    {
        if(!(pM.isBlockingHigh == true ^ pM.isBlockingLow == true))
        {
            currentHealth -= damage;
            Mathf.Round(currentHealth);
            SetHealth(currentHealth);
            //decreases health based on damage number
        }   
    }
    public void MakeHealth(float healthPercent)
    {
        currentHealth += healthPercent;
        SetHealth(currentHealth);
        //increases health based on healing value
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "TempHitBox")
        {
            Debug.Log("Hit hitbox");
        }
        //check for damage detection
    }
    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= currentRecovHealth)
        {
            //currentHealth = currentRecovHealth;
        }

        if (currentHealth >= 500)
        {
            currentHealth = 500;
        }
        //caps health at 500
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        //prevents health from reaching values below 0
        if (currentHealth <= 0)
        {
            
            GameObject gManager = GameObject.Find("GameManager");
            GameManagerScript gM = (GameManagerScript)gManager.GetComponent(typeof(GameManagerScript));
            //currentHealth = 0;
            // gM.currentMatchState = GameManagerScript.playerState.Knockout;
        }
        
    }
}

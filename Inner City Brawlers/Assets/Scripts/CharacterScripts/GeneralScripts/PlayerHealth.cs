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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentRecovHealth = maxHealth;
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        Fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Mathf.Round(currentHealth);
        //currentRecovHealth -= (Mathf.Round(damage/2));
        SetHealth(currentHealth);
    }
    public void MakeHealth(float healthPercent)
    {
        currentHealth += healthPercent;
        SetHealth(currentHealth);

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "TempHitBox")
        {
            TakeDamage(20f);
            meterSystem.GiveMeter(0.1f);
            meterSystem.MakeMeter(0.3f);
            Debug.Log("Hit hitbox");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= currentRecovHealth)
        {
            //currentHealth = currentRecovHealth;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Rewired;

public class Health_MeterChangeScript : MonoBehaviour
{
    [Header("Player Health/Meter Variables")]
    public PlayerHealth[] pH;
    public MeterSystem mS;
    [Header("Player 1 Health Object")]
    public Slider p1Slider;
    public Image p1SliderImage;
    public float p1Sliderfloat;
    public Gradient p1Gradient;
    public TextMeshProUGUI p1HealthText;

    [Header("Player 2 Health Object")]
    public Slider p2Slider;
    public Image p2SliderImage;
    public float p2Sliderfloat;
    public TextMeshProUGUI p2HealthText;

    //0 = Player 1
    //1 = Player 2
    [Header("Player 1 Meter Object")]
    public Slider p1MSlider;
    public Image p1MSliderImage;
    public float p1MSliderfloat;
    public TextMeshProUGUI p1MeterText;

    [Header("Player 2 Meter Object")]
    public Slider p2MSlider;
    public Image p2MSliderImage;
    public float p2MSliderfloat;
    public TextMeshProUGUI p2MeterText;

    [SerializeField] private int playerID;
    [SerializeField] private Player player;

    //0 = Player 1
    //1 = Player 2

    /* Player 1 Health Variables*/
    public void Setp1MaxHealth(int health)
    {
        p1Slider = pH[0].slider;
        p1SliderImage = pH[0].Fill;
        pH[0].SetMaxHealth(pH[0].maxHealth);
    }

    public void p1DecreaseHealth(float damage)
    {
        pH[0].TakeDamage(damage);
        Mathf.Round(p1Sliderfloat);
        SetHealth(p1Sliderfloat);
    }
    public void Makep1Health(float healthPercent)
    {
        pH[0].MakeHealth(healthPercent);
        Mathf.Round(p1Sliderfloat);
        SetHealth(p1Sliderfloat);
    }
    public void SetHealth(float health)
    {
        p1Slider.value = p1Sliderfloat;
        p1SliderImage.color = p1Gradient.Evaluate(p1Slider.normalizedValue);
    }
    /* Player 1 Meter Variables*/
    public void p1MakeMeter(float miPoint) // miPoint = meter increase point
    {
        mS.p1MakeMeter(miPoint);
        mS.SetP1Meter(p1MSliderfloat);
        //mS.p1Slider = p1MSlider;
        //mS.p1Fill = p1MSliderImage;
        //mS.p1MakeMeter(p1MSliderfloat);
    }

    public void p1TakeMeter(float miPoint) // miPoint = meter increase point
    {  
        mS.p1DecreaseMeter(miPoint);
        mS.SetP1Meter(p1MSliderfloat);

        //mS.p1Slider = p1MSlider;
        //mS.p1Fill = p1MSliderImage;
        //mS.p1DecreaseMeter(p1MSliderfloat);
    }

    public void SetP1MaxMeter(float MaxMeter)
    {
        p1MSlider = mS.p1Slider;
        p1MSliderImage = mS.p1Fill;
        mS.SetP1MaxMeter(mS.p1MaxMeter);
    }
    public void ResetP1Meter()
    {
        p1MSlider = mS.p1Slider;
        p1MSliderImage = mS.p1Fill;
        mS.ResetP1Meter();
    }

    /* Player 2 Health Variables*/

    public void Setp2MaxHealth(int health)
    {
        p1Slider = pH[1].slider;
        p1SliderImage = pH[1].Fill;
        pH[1].SetMaxHealth(pH[1].maxHealth);
    }

    public void p2DecreaseHealth(float damage)
    {
        pH[1].TakeDamage(damage);
        Mathf.Round(p2Sliderfloat);
        SetHealth(p2Sliderfloat);
    }
    public void Makep2Health(float healthPercent)
    {
        pH[1].MakeHealth(healthPercent);
        Mathf.Round(p2Sliderfloat);
        SetHealth(p2Sliderfloat);
    }
    /* Player 2 Meter Variables*/
    public void p2MakeMeter(float miPoint) // miPoint = meter increase point
    {
        p2MSlider = mS.p2Slider;
        p2MSliderImage = mS.p2Fill;
        mS.p2MakeMeter(0.5f);
    }

    public void p2TakeMeter(float miPoint) // miPoint = meter increase point
    {
        p2MSlider = mS.p2Slider;
        p2MSliderImage = mS.p2Fill;
        mS.p2DecreaseMeter(0.5f);
    }

    public void SetP2MaxMeter(float MaxMeter)
    {
        p2MSlider = mS.p2Slider;
        p2MSliderImage = mS.p2Fill;
        mS.SetP2MaxMeter(mS.p2MaxMeter);
    }

    public void ResetP2Meter()
    {
        p2MSlider = mS.p2Slider;
        p2MSliderImage = mS.p2Fill;
        mS.ResetP2Meter();
    }
    // Start is called before the first frame update
    void Start()
    {
        //player = p1.player;
        //playerID = p1.playerID;
        player = ReInput.players.GetPlayer(playerID);

        pH[0].currentHealth = p1Sliderfloat;

        p2Sliderfloat = pH[1].currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        p1HealthText.text =  pH[0].currentHealth + " /500";
        p2HealthText.text = pH[1].currentHealth + " /500";
        p1MeterText.text = mS.p1CurrentMeter + " /4";
        p2MeterText.text = mS.p2CurrentMeter + " /4";
    }
}

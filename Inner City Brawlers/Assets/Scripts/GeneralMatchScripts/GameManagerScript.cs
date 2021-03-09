using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public float currentTimerInt;

    public float maxTimerInt;

    public Text TimerText;

    // Start is called before the first frame update
    void Start()
    {
        currentTimerInt = maxTimerInt;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timerTickDown();
        if (currentTimerInt <= 0)
        {
            TimerText.text = "Round Over";
        }
    }

    public void timerTickDown()
    {
        currentTimerInt-= Time.deltaTime;
        TimerText.text = "Time " + (Mathf.Round(currentTimerInt)).ToString();
    }
}

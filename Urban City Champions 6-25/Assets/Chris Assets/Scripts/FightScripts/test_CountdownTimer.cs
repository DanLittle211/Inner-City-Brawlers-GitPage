using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_CountdownTimer : MonoBehaviour
{
    public float tempCurrentTime;
    public float tempStartingTime;

    [SerializeField] Text testTimerText;

    void Start() {

        tempCurrentTime = tempStartingTime;

    }

    void Update() {

        tempCurrentTime -= 1 * Time.deltaTime;
        testTimerText.text = tempCurrentTime.ToString("0");

        if (tempCurrentTime <= 0) {

            tempCurrentTime = 0;
        
        }
    }
}

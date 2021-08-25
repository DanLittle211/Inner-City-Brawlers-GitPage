using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bweap_Timer : MonoBehaviour
{
    //public enum whichWeapon { Baseball_Bat, Pipe_Wrench, Pipe}
    //whichWeapon currentBWeapon;
    public bool hasWeapon;
    public float countDown;
    public float StartTime;

    private void Awake()
    {
        countDown = StartTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hasWeapon)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                countDown = 0;
                hasWeapon = false;
            }
        }
        
    }
    public void ResetTimer()
    {
        countDown = StartTime;
    }
}

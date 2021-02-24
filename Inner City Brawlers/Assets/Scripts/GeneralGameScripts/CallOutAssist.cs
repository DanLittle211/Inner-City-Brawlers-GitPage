using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CallOutAssist : MonoBehaviour
    
{
    [Header("Bool Controllers")]
    public bool AssistAllowed;//Tracks if the option is toggled On or Off
    public bool oneTime;//Allows spawning of item when true
    //temporary for testing
    private void Awake()
    {
        AssistAllowed = true;
    }
    void Update()
    {
        //If the Assist is set to be ON the E-key will be available to instantiate the random item to assist the player
        if (AssistAllowed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                setOneTime();//calls method that sets the spawing bool to true
            }
        }
        //what happens when the spawning bool is set true
        if (oneTime)
        {
            //Instantiate();
        }
    }
    public void setOneTime()
    {
        oneTime = true;
    }
}

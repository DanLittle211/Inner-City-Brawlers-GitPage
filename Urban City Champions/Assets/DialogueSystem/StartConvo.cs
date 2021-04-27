using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartConvo : MonoBehaviour
{
    DialogTrigger dialogTrigger;
    void Start()
    {
        dialogTrigger = GetComponent<DialogTrigger>();
    }

    public void StartConverse()
    {
        dialogTrigger.TriggerDialog();
        
    }
}

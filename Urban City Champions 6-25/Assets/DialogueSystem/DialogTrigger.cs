using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog; //connect to Dialog.cs

    public void TriggerDialog()
    {

        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerButtons : MonoBehaviour
{
    public PlayerMovement pM;

    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("LightAttack"))
        {
            Debug.Log("Light attack ");
        }
        if (player.GetButtonDown("MediumAttack"))
        {
            Debug.Log("medium attack ");
        }
        if (player.GetButtonDown("HeavyAttack"))
        {
            Debug.Log("Heavy attack ");
        }
        if (player.GetButtonDown("UniqueAttack"))
        {
            Debug.Log("Unique attack ");
        }
        if (player.GetButtonDown("Throw"))
        {
            Debug.Log("Throw");
        }
        if (player.GetButtonDown("Use Assist"))
        {
            Debug.Log("Use Assist ");
        }
        if (player.GetButtonDown("CalloutAssist"))
        {
            GameObject gameManager = GameObject.Find("GameManager");
            GameManagerScript gM = (GameManagerScript)gameManager.GetComponent(typeof(GameManagerScript));
            gM.useP1CaAssist();
            Debug.Log("Jump " + pM.currentPlayState);
        }
        if (player.GetButtonDown("LifelineAssist"))
        {
            Debug.Log("LifeLine Assist Used ");
        }
        if (player.GetButtonDown("Pause"))
        {
            Debug.Log("Game Paused");
        }  
    }
}

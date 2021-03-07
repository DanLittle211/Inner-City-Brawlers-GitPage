//created by Will McBride III
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoundManager : MonoBehaviour
{
    //round States
    public enum roundState {preRound, duringRound, roundPaused, endRound, continueOption}
    public roundState currentState;
    [Header("Round Win Variables")]
    public int currentRound;
    public int winsNeeded;
    public int p1CurrentWins;
    public int p2CurrentWins;
    [Header("Script references")]
    public GameManagerScript gManager;
    private PlayerHealth p1Health;
    private PlayerHealth p2Health;
    private PlayerMovement p1Movement;
    private PlayerMovement p2Movement;
    [Header("Player References")]
    public GameObject player1;
    public GameObject player2;
    [Header("Round Results")]
    public bool p1Wins;
    public bool p2Wins;
    public bool haveTied;

    public void Awake()
    {
        //Player script 
        p1Health = player1.GetComponent<PlayerHealth>();
        p2Health = player2.GetComponent<PlayerHealth>();
        p1Movement = player1.GetComponent<PlayerMovement>();
        p2Movement = player2.GetComponent<PlayerMovement>();
        //other variables
        currentRound = 1;
        p1CurrentWins = 0;
        p2CurrentWins = 0;
        currentState = roundState.preRound;
    }

    private void Update()
    {
     
        //used for tracking round wins and others 
        if(currentState == roundState.duringRound)
        {
            //checking player 1 health
            if(p1Health.currentHealth <= 0 && !p2Wins && !haveTied)
            {
                p1Wins = true;
                currentState = roundState.endRound;
            }
            //checking player 2 health
            if(p2Health.currentHealth <= 0 && !p1Wins && !haveTied)
            {
                p2Wins = true;
                currentState = roundState.endRound;
            }
            //checking for ties
            if((p1Health.currentHealth == 0 && p2Health.currentHealth == 0) && (!p1Wins && !p2Wins))
            {
                haveTied = true;
                currentState = roundState.endRound;
            }
        }
        //used for adding to round wins
        if(currentState == roundState.endRound)
        {
            //what happens when player 1 wins the round
            if (p1Wins)
            {

            }
            //what happens when player 2 wins the round
            if (p2Wins)
            {

            }
            //what happens when the players have tied
            if (haveTied)
            {

            }
        }
    }

}

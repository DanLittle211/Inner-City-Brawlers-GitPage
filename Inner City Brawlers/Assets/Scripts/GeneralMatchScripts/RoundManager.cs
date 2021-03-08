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
    #region Round Variables
    [Header("Round Win Variables")]
    public int currentRound;
    public int totalRounds;
    public int p1CurrentWins;
    public int p2CurrentWins;
    #endregion
    #region Round Timer Variables
    [Header("Round Timers")]
    private float roundTimer;
    public float roundTimerStart;
    private float preroundTimer;
    public float preroundTimerStart;
    #endregion
    #region Script References
    [Header("Script references")]
    public GameManagerScript gManager;
    private PlayerHealth p1Health;
    private PlayerHealth p2Health;
    private PlayerMovement p1Movement;
    private PlayerMovement p2Movement;
    #endregion
    #region Player Object Referecnes
    [Header("Player References")]
    public GameObject player1;
    public GameObject player2;
    #endregion
    #region Round Result Bools
    [Header("Round Results")]
    public bool p1Wins;
    public bool p2Wins;
    public bool haveTied;
    #endregion

    public void Awake()
    {
        //Player script 
        p1Health = player1.GetComponent<PlayerHealth>();
        p2Health = player2.GetComponent<PlayerHealth>();
        p1Movement = player1.GetComponent<PlayerMovement>();
        p2Movement = player2.GetComponent<PlayerMovement>();
        //other variables
        roundTimer = roundTimerStart;
        preroundTimer = preroundTimerStart;
        currentRound = 1;
        p1CurrentWins = 0;
        p2CurrentWins = 0;
        currentState = roundState.preRound;
    }

    private void Update()
    {
        //preround countdown
        if(currentState == roundState.preRound)
        {
            preroundTimer -= Time.deltaTime; //counts down timer while in this state
            if (preroundTimer <= 0)
            {
                preroundTimer = 0;
                currentState = roundState.duringRound;
            }
        }
        //used for tracking round wins and others 
        if(currentState == roundState.duringRound)
        {
            //round timer functions
            roundTimer -= Time.deltaTime; //counts down timer while in this state
            //what happens when the timer runs out and no-one won
            if(roundTimer <= 0)
            {
                roundTimer = 0;
                //p1 wins
                if(p1Health.currentHealth > p2Health.currentHealth)
                {
                    p1Wins = true;
                    currentState = roundState.endRound;
                }
                //p2 wins
                if(p1Health.currentHealth < p2Health.currentHealth)
                {
                    p2Wins = true;
                    currentState = roundState.endRound;
                }
                //players tie
                if(p1Health.currentHealth == p2Health.currentHealth)
                {
                    haveTied = true;
                    currentState = roundState.endRound;
                }

            }
            //if the round timer is > 0
            else
            {
                //player 1 is knockedout
                if (p1Health.currentHealth <= 0 && !p1Wins && !haveTied)
                {
                    p2Wins = true;
                    currentState = roundState.endRound;
                }
                //player 2 is knockedout
                if (p2Health.currentHealth <= 0 && !p2Wins && !haveTied)
                {
                    p1Wins = true;
                    currentState = roundState.endRound;
                }
                //both are knocked out at same time
                if ((p1Health.currentHealth == 0 && p2Health.currentHealth == 0) && (!p1Wins && !p2Wins))
                {
                    haveTied = true;
                    currentState = roundState.endRound;
                }
            }
           
        }
        //used for adding to round wins
        if(currentState == roundState.endRound)
        {
            //what happens when player 1 wins the round
            if (p1Wins)
            {
                p1CurrentWins = p1CurrentWins + 1;
                currentState = roundState.continueOption;
            }
            //what happens when player 2 wins the round
            if (p2Wins)
            {
                p2CurrentWins = p2CurrentWins + 1;
                currentState = roundState.continueOption;
            }
            //what happens when the players have tied
            if (haveTied)
            {
                currentState = roundState.continueOption;
            }
        }
        //used for restarting rounds and tracking total rounds
        if(currentState == roundState.continueOption)
        {
            //works as a reset for the round
            if(currentRound < totalRounds)
            {
                p1Wins = false;
                p2Wins = false;
                haveTied = false;
                roundTimer = roundTimerStart;
                preroundTimer = preroundTimerStart;
                currentRound = currentRound + 1;
                currentState = roundState.preRound;
            }
            else
            {
                //if p1 has more wins than p2 at the last round, for displaying who wins and giving options to restart or not
                if(p1CurrentWins > p2CurrentWins)
                {
                    Debug.Log("Player 1 Wins");
                }
                //if p2 has more wins than p1 at the last round, for displaying who wins and giving options to restart or not
                if (p2CurrentWins > p1CurrentWins)
                {
                    Debug.Log("Player 2 Wins");
                }
                //if both players have equal wins, for displaying who wins and giving options to restart or not
                if (p1CurrentWins == p2CurrentWins)
                {
                    Debug.Log("The players tied");
                }
            }
        }
    }

}

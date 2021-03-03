using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerButtons : MonoBehaviour
{
    [SerializeField]public PlayerMovement pM;
    public GameObject[] hitboxes;


    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(pM.playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (pM.currentPlayState == PlayerMovement.playerState.Grounded || pM.currentPlayState == PlayerMovement.playerState.Jump)
        {
            if (player.GetButtonDown("LightAttack"))
            {
                StartCoroutine(lightAttack(0.2f));
                Debug.Log("Light attack ");

            }
            if (player.GetButtonDown("MediumAttack"))
            {
                StartCoroutine(mediumAttack(0.6f));
                Debug.Log("medium attack ");
            }
            if (player.GetButtonDown("HeavyAttack"))
            {
                StartCoroutine(heavyAttack(1f));
                Debug.Log("Heavy attack ");
            }
        }
        if (pM.currentPlayState == PlayerMovement.playerState.Grounded)
        {
            if (player.GetButtonDown("UniqueAttack"))
            {
                StartCoroutine(uniqueAttack(1.2f));
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
                CallOutAssist gM = (CallOutAssist)gameManager.GetComponent(typeof(CallOutAssist));
                if(playerID == 0)
                {
                    gM.p1UseCAssist();
                }

                if (playerID == 1)
                {
                    gM.p2UseCAssist();
                }

                Debug.Log("CurrentPlayerState: " + pM.currentPlayState);
            }
            if (player.GetButtonDown("LifelineAssist"))
            {
                Debug.Log("LifeLine Assist Used ");
            }
        }
        if (player.GetButtonDown("Pause"))
        {
            GameObject gameManager = GameObject.Find("GameManager");
            GameManagerScript gM = (GameManagerScript)gameManager.GetComponent(typeof(GameManagerScript));
            //gM.useP1CaAssist();
            Debug.Log("CurrentPlayerState " + pM.currentPlayState);
            Debug.Log("CurrentGameState");
        }  
    }
    IEnumerator lightAttack(float time)
    {
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[0].gameObject.SetActive(false);
        StopCoroutine(lightAttack(1f));
    }
    IEnumerator mediumAttack(float time)
    {
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[1].gameObject.SetActive(false);
        StopCoroutine(mediumAttack(1f));
    }
    IEnumerator heavyAttack(float time)
    {
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[2].gameObject.SetActive(false);
        StopCoroutine(heavyAttack(1f));
    }
    IEnumerator uniqueAttack(float time)
    {
        pM.currentPlayState = PlayerMovement.playerState.Immobile;
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[3].gameObject.SetActive(false);
        StopCoroutine(uniqueAttack(1f));
        pM.currentPlayState = PlayerMovement.playerState.Grounded;
    }
}

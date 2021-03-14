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
    [SerializeField] public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(pM.playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if ((pM.currentPlayState == PlayerMovement.playerState.Grounded || pM.currentPlayState == PlayerMovement.playerState.Jump) && pM.isDisabled != true)
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
        if ((pM.currentPlayState == PlayerMovement.playerState.Grounded) && pM.isDisabled != true)
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
                GameObject gameManagerCA = GameObject.Find("GameManager");
                CallOutAssist gMCA = (CallOutAssist)gameManagerCA.GetComponent(typeof(CallOutAssist));
                if(playerID == 0)
                {
                    gMCA.p1UseCAssist();
                }

                if (playerID == 1)
                {
                    gMCA.p2UseCAssist();
                }

                Debug.Log("CurrentPlayerState: " + pM.currentPlayState);
            }
            if (player.GetButtonDown("LifelineAssist"))
            {
                Debug.Log("LifeLine Assist Used ");
            }
        }

        GameObject gameMasterManager = GameObject.Find("GameMasterManager");
        GameMasterManager gMM = (GameMasterManager)gameMasterManager.GetComponent(typeof(GameMasterManager));

        GameObject gameManager = GameObject.Find("GameManager");
        GameManagerScript gM = (GameManagerScript)gameMasterManager.GetComponent(typeof(GameManagerScript));

        if (player.GetButtonDown("Pause"))
        {
            if (gMM.isMultiActive == true)
            {
                //Gustave put code to reference pausing game for multiplayer here
            }
            if (gMM.isMultiActive == false)
            {
                GameObject TrainingPM = GameObject.Find("GameManager");
                TrainingPauseManager tPM = (TrainingPauseManager)TrainingPM.GetComponent(typeof(TrainingPauseManager));
                tPM.SetPause();
            }
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

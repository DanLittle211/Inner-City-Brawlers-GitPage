using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

public enum AttackType {light = 0, medium = 1, heavy = 2, unique = 3, throwAction = 4, Assist = 5, Lifeline = 6};

public class PlayerButtons : MonoBehaviour
{
    [SerializeField]public PlayerMovement pM;
    public GameObject[] hitboxes;


    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] public Player player;
    private RoundManager rM;

    [Header("Attacks")]
    public Attack lightAttack;
    public Attack MediumAttack;
    public Attack HeavyAttack;
    public Attack UniqueAttack;
    public Attack ThrowAttack;
    public Attack AssistAttack;
    public Attack LifeLineAssist;

    [SerializeField]public List<Combo> combos;
    [SerializeField]public float buttonCheckDuration;

    /*[Header("Components")]
    public Animator ani;*/

    ComboInput LastInput = null;
    Attack curAttack = null;
    public float timer = 0;
    [SerializeField] public List<int> currentCombos = new List<int>();

    void PrimeCombo()
    {
        for (int i = 0; i < combos.Count; i++)
        {
            Combo c = combos[i];
            c.onInputted.AddListener(() =>
            {
                Attack(c.comboAttack);
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rM = GameObject.Find("GameManager").GetComponent<RoundManager>();
        player = ReInput.players.GetPlayer(pM.playerID);
        PrimeCombo();
    }

    void Attack(Attack A)
    {
        curAttack = A;
        timer = A.length;
        //animation.Play(animationName, -1, 0); playAnimation in code and resets anim
    }

    // Update is called once per frame
    void Update()
    {
        PauseButtons();
        if (pM.isDisabled != true)
        {
            if (curAttack != null)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                   
                }
                else
                {
                    curAttack = null;
                }
            }
            ComboInput input = null;

            if (pM.currentPlayState == PlayerMovement.playerState.Grounded)
            {
                if (pM.currentPlayState == PlayerMovement.playerState.Grounded || pM.currentPlayState == PlayerMovement.playerState.Jump)
                {
                    if (player.GetButtonDown("LightAttack"))
                    {
                        StartCoroutine(LightAttack(0.2f));
                        input = new ComboInput(AttackType.light);
                        Debug.Log("Light attack ");
                    }
                    if (player.GetButtonDown("MediumAttack"))
                    {
                        StartCoroutine(mediumAttack(0.6f));
                        input = new ComboInput(AttackType.medium);
                        Debug.Log("medium attack ");
                    }
                    if (player.GetButtonDown("HeavyAttack"))
                    {
                        StartCoroutine(heavyAttack(1f));
                        input = new ComboInput(AttackType.heavy);
                        Debug.Log("Heavy attack ");
                    }
                }
                if (player.GetButtonDown("UniqueAttack"))
                {
                    StartCoroutine(uniqueAttack(1.2f));
                    input = new ComboInput(AttackType.unique);
                    Debug.Log("Unique attack ");
                }
                if (player.GetButtonDown("Throw"))
                {
                    input = new ComboInput(AttackType.throwAction);
                    Debug.Log("Throw");
                }
                if (player.GetButtonDown("Use Assist"))
                {
                    Debug.Log("Use Assist ");
                    input = new ComboInput(AttackType.Assist);
                }
                if (player.GetButtonDown("CalloutAssist"))
                {
                    GameObject gameManagerCA = GameObject.Find("GameManager");
                    CallOutAssist gMCA = (CallOutAssist)gameManagerCA.GetComponent(typeof(CallOutAssist));
                    if (playerID == 0)
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
                    input = new ComboInput(AttackType.Lifeline);
                }

                List<int> remove = new List<int>();
                for(int i = 0; i < currentCombos.Count; i++)
                {
                    Combo c = combos[currentCombos[i]];
                    if (c.continueCombo(input))
                    {
                        //Do Something
                    }
                    else
                    {
                        remove.Add(i);
                    }
                }
                if (currentCombos.Count <= 0)
                {
                    Attack(getAttackFromType(input.type));
                }
                foreach (int i in remove)
                {
                    currentCombos.RemoveAt(i);
                }
            }
            if (input == null)
            {
                LastInput = input;
                return;
            }
        }

    }
    Attack getAttackFromType(AttackType t)
    {
        if (t == AttackType.light)
        {
            return lightAttack;
        }
        if (t == AttackType.medium)
        {
            return MediumAttack;
        }
        if (t == AttackType.heavy)
        {
            return HeavyAttack;
        }
        if (t == AttackType.unique)
        {
            return UniqueAttack;
        }
        if (t == AttackType.throwAction)
        {
            return ThrowAttack;
        }
        if (t == AttackType.Assist)
        {
            return AssistAttack;
        }
        if (t == AttackType.Lifeline)
        {
            return LifeLineAssist;
        }
        return null;
    }
    public void PauseButtons()
    {
        GameObject gameMasterManager = GameObject.Find("GameMasterManager");
        GameMasterManager gMM = (GameMasterManager)gameMasterManager.GetComponent(typeof(GameMasterManager));

        GameObject gameManager = GameObject.Find("GameManager");
        GameManagerScript gM = (GameManagerScript)gameMasterManager.GetComponent(typeof(GameManagerScript));

        if (player.GetButtonDown("Pause"))
        {
            if (gMM.isMultiActive == true)
            {
                rM.PauseGame();
            }
            if (gMM.isMultiActive == false)
            {
                GameObject TrainingPM = GameObject.Find("SettingManager");
                TrainingPauseManager tPM = (TrainingPauseManager)TrainingPM.GetComponent(typeof(TrainingPauseManager));
                tPM.SetPause();
            }
        }

    }
    IEnumerator LightAttack(float time)
    {
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[0].gameObject.SetActive(false);
        StopCoroutine(LightAttack(1f));
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

[System.Serializable]
public class Attack
{
    public string name;
    public float length;
}
[System.Serializable]
public class Combo
{
    public List<ComboInput> inputs;
    public Attack comboAttack;
    public UnityEvent onInputted;
    int curInput = 0;

    public bool continueCombo(ComboInput I)
    {
        if (inputs[curInput].isSameAs(I))
        {
            curInput++;
            if (curInput >= inputs.Count)
            {
                onInputted.Invoke();
                curInput = 0;
            }
            return true;
        }
        else
        {
            curInput = 0;
            return false;
        }
    }
    public ComboInput currentComboInput()
    {
        if (curInput >= inputs.Count)
        {
            return null;
        }
        return inputs[curInput];
    }
    public void ResetCombo()
    {
        curInput = 0;
    }
}
[System.Serializable]
public class ComboInput : MonoBehaviour
{
    public AttackType type;
    //MoveMent input placed here
    public PlayerMovement pM;
    [SerializeField] public float upMovement;
    [SerializeField] public float rightMovement;

    public ComboInput(AttackType e)
    {
        type = e;
    }

    void Start()
    {
        pM = this.GetComponent<PlayerMovement>();
        rightMovement = pM.moveHorizontal;
        upMovement = pM.moveUp;
    }
    public bool isSameAs(ComboInput J)
    {
        return ((type == J.type) && (rightMovement == J.rightMovement) && (upMovement == J.upMovement));
    }
}
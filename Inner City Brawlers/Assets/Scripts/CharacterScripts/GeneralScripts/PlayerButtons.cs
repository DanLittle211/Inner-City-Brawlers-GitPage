using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;


public enum AttackType {light = 0, medium = 1, heavy = 2, unique = 3, throwAction = 4, Assist = 5, Lifeline = 6, movement = 7};

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
    [SerializeField]public float buttonCheckDuration = 0.2f;

    [Header("Components")]
    public Animator myAnim;

    ComboInput LastInput = null;
    Attack curAttack = null;
    float timer = 0;
    float leeway = 0;
    [SerializeField] public List<int> currentCombos = new List<int>();
    bool skip = false;

    void PrimeCombo()
    {
        for (int i = 0; i < combos.Count; i++)
        {
            Combo c = combos[i];
            c.onInputted.AddListener(() =>
            {
                skip = true;
                LastInput = null;
                Attack(c.comboAttack);
                ResetCombos();
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rM = GameObject.Find("GameManager").GetComponent<RoundManager>();
        player = ReInput.players.GetPlayer(pM.playerID);
        PrimeCombo();
        //myAnim = this.GetComponent<Animator>();
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
                return;
            }
            ComboInput input = null;
            if (currentCombos.Count > 0)
            {
                leeway += Time.deltaTime;
                if (leeway >= buttonCheckDuration)
                {
                    if (LastInput != null)
                    {
                        Attack att = getAttackFromType(LastInput.type);
                        if (att != null)
                        {
                            Attack(att);
                            LastInput = null;
                        }
                    }
                    ResetCombos();
                }
            }
            else
            {
                leeway = 0;
            }

            if (player.GetButtonDown("LightAttack"))
            {
                StartCoroutine(LAttack(0.2f));
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                input = new ComboInput(AttackType.light); Debug.Log("Light attack");
            }
            if (player.GetButtonDown("MediumAttack"))
            {
                StartCoroutine(mediumAttack(0.6f));
                transform.position = new Vector2(transform.position.x + 2, transform.position.y);
                input = new ComboInput(AttackType.medium); Debug.Log("medium attack ");
            }
            if (player.GetButtonDown("HeavyAttack"))
            {
                StartCoroutine(heavyAttack(1f));
                transform.position = new Vector2(transform.position.x + 3, transform.position.y);
                input = new ComboInput(AttackType.heavy); Debug.Log("Heavy attack ");
            }
            
            if (pM.currentPlayState == PlayerMovement.playerState.Grounded ^ pM.currentPlayState == PlayerMovement.playerState.Crouch)
            {
                if (player.GetButtonDown("UniqueAttack"))
                {
                    StartCoroutine(uniqueAttack(1.2f));
                    input = new ComboInput(AttackType.unique); Debug.Log("Unique attack ");

                }
                if (player.GetButtonDown("Throw"))
                {
                    input = new ComboInput(AttackType.throwAction); Debug.Log("Throw");

                }
                if (player.GetButtonDown("Use Assist"))
                {
                   
                    input = new ComboInput(AttackType.Assist); Debug.Log("Use Assist ");
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
                    input = new ComboInput(AttackType.Lifeline); Debug.Log("LifeLine Assist Used ");
                }
                Vector2 movement = Vector2.zero;
                if(PlayerMovement.InputDownX())
                {
                    movement.x = PlayerMovement.x;
                    Debug.Log("Move right left");
                }
                if (PlayerMovement.InputDownY())
                {
                    movement.y = PlayerMovement.y;
                    Debug.Log("Move up down");
                }

                if (movement != Vector2.zero)
                {
                    input = new ComboInput(movement);
                }

                if (input == null)
                {
                    return;
                }
                LastInput = input;
                List<int> remove = new List<int>();

                for (int i = 0; i < currentCombos.Count; i++)
                {
                    Combo c = combos[currentCombos[i]];
                    if (c.continueCombo(input))
                    {
                        leeway = 0;
                    }
                    else
                    {
                        remove.Add(i);
                    }
                }
                if (skip != false)
                {
                    skip = false;
                    return;
                }
                for (int i = 0; i < combos.Count; i++)
                {
                    if (currentCombos.Contains(i))
                    {
                        continue;
                    }
                    if (combos[i].continueCombo(input))
                    {
                        currentCombos.Add(i);
                        leeway = 0;
                    }
                }
                Attack att = getAttackFromType(input.type);
                foreach (int i in remove)
                {
                    currentCombos.RemoveAt(i);
                }
                if (att != null && currentCombos.Count <= 0)
                {
                    //Attack(getAttackFromType(input.type));
                    Attack(att);
                }
            }
            
        }

    }
    public void Attack(Attack A)
    {
        curAttack = A;
        timer = A.length;
        myAnim.Play(A.name, -1, 0); //playAnimation in code and resets anim
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

    void ResetCombos()
    {
        leeway = 0;
        for (int i = 0; i < currentCombos.Count; i++)
        {
            Combo c = combos[currentCombos[i]];
            c.ResetCombo();
        }
        currentCombos.Clear();
    }
    public void beginLC()
    {
        StartCoroutine(LAttack(0f));
    }
    public void beginMC()
    {
        StartCoroutine(LAttack(0f));
    }
    public void beginHC()
    {
        StartCoroutine(LAttack(0f));
    }
    public void beginUC()
    {
        StartCoroutine(LAttack(0f));
    }
    IEnumerator LAttack(float time)
    {
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[0].gameObject.SetActive(false);
       // StopCoroutine(LAttack(1f));
    }
    IEnumerator mediumAttack(float time)
    {
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[1].gameObject.SetActive(false);
        //StopCoroutine(mediumAttack(1f));
    }
    IEnumerator heavyAttack(float time)
    {
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[2].gameObject.SetActive(false);
       // StopCoroutine(heavyAttack(1f));
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
      //  StopCoroutine(uniqueAttack(1f));
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
    public string name;
    public List<ComboInput> inputs;
    public Attack comboAttack;
    
    public UnityEvent onInputted;
    int curInput = 0;

    public bool continueCombo(ComboInput I)
    {
        if (inputs[curInput].IsSameAs(I))
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
public class ComboInput 
{
    public AttackType type;
    public Vector2 mType;

    public ComboInput(AttackType e)
    {
        type = e;
        mType = Vector2.zero;
    }
    public ComboInput(Vector2 e)
    {
        type = AttackType.movement;
        mType = e;
    }

    public bool IsSameAs(ComboInput J)
    {
        return ((type == AttackType.movement) ? (validMovemment(J.mType)) : (type == J.type)); 
        //In a single statement akin to an if statement. it checks movement inputs, then buttons
    }

    bool validMovemment(Vector2 move)
    {
        bool valid = true;
        if (mType.x != 0 && mType.x != move.x)
        {
            valid = false;
        }
        if (mType.y != 0 && mType.y != move.y)
        {
            valid = false;
        }
        return valid;
    }
}
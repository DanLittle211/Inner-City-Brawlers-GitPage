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

    public bool isAttacking;

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
                //isAttacking = false;
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pM = this.GetComponent<PlayerMovement>();
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
            switch (pM.currentPlayState)
            {
                case PlayerMovement.playerState.Grounded:
                    {
                        break;
                    }
                case PlayerMovement.playerState.Crouch:
                    {

                        break;
                    }
                case PlayerMovement.playerState.Jump:
                    {

                        break;
                    }
                case PlayerMovement.playerState.Block:
                    {
                        break;
                    }
                case PlayerMovement.playerState.SoftKnockdown:
                    {
                        break;
                    }
                case PlayerMovement.playerState.HardKnockdown:
                    {
                        break;
                    }
                case PlayerMovement.playerState.Immobile:
                    {
                        break;
                    }
            }
            GameControls();
        }
       
    }
    #region SettingState for This Character
    public void SetPlayerStateGrounded()
    {
        pM.currentPlayState = PlayerMovement.playerState.Grounded;
    }
    public void SetPlayerStateCrouch()
    {
        pM.currentPlayState = PlayerMovement.playerState.Crouch;
    }
    public void SetPlayerStateJump()
    {
        pM.currentPlayState = PlayerMovement.playerState.Jump;
    }
    public void SetPlayerStateSKD()
    {
        pM.currentPlayState = PlayerMovement.playerState.SoftKnockdown;
    }
    public void SetPlayerStateHKD()
    {
        pM.currentPlayState = PlayerMovement.playerState.HardKnockdown;
    }
    public void SetPlayerStateImmobile()
    {
        pM.currentPlayState = PlayerMovement.playerState.Immobile;
    }

    #endregion

    void GameControls()
    {

        if (curAttack != null)
        {
            //SetPlayerStateImmobile();
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
        else
        {
            //SetPlayerStateGrounded();
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
                        //isAttacking = false;
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
            input = new ComboInput(AttackType.light, pM.GetInput()); Debug.Log("Light attack");
        }
        if (player.GetButtonDown("MediumAttack"))
        {
            StartCoroutine(mediumAttack(0.6f));
            input = new ComboInput(AttackType.medium, pM.GetInput()); Debug.Log("medium attack ");
        }
        if (player.GetButtonDown("HeavyAttack"))
        {
            StartCoroutine(heavyAttack(1f));
            input = new ComboInput(AttackType.heavy, pM.GetInput()); Debug.Log("Heavy attack ");
        }

        if (pM.currentPlayState == PlayerMovement.playerState.Grounded ^ pM.currentPlayState == PlayerMovement.playerState.Crouch)
        {
            if (player.GetButtonDown("UniqueAttack"))
            {
                StartCoroutine(uniqueAttack(1.2f));
                input = new ComboInput(AttackType.unique, pM.GetInput()); Debug.Log("Unique attack ");

            }
            if (player.GetButtonDown("Throw"))
            {
                input = new ComboInput(AttackType.throwAction, pM.GetInput()); Debug.Log("Throw");

            }
            if (player.GetButtonDown("Use Assist"))
            {

                input = new ComboInput(AttackType.Assist, pM.GetInput()); Debug.Log("Use Assist ");
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
                input = new ComboInput(AttackType.Lifeline, pM.GetInput()); Debug.Log("LifeLine Assist Used ");
            }

            Vector2 movement = Vector2.zero;
            if (pM.InputDownX1())
            {
                DirectionLook dlook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
                if (playerID == 0)
                {
                    if (dlook.isFlipped == true)
                    {
                        movement.x = -(pM.x1);
                    }
                    else
                    {
                        movement.x = (pM.x1);
                    }
                }
                if (playerID == 1)
                {
                    if (dlook.isFlipped == false)
                    {
                        movement.x = -(pM.x1);
                    }
                    else
                    {
                        movement.x = (pM.x1);
                    }
                }
            }
            if (pM.InputDownY1())
            {
                movement.y = pM.y1;
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
                Attack(att);
            }
        }
    }

    public void SetIsAttackingTrue()
    {
        isAttacking = true;
    }

    public void SetIsAttackingFalse()
    {
       //isAttacking = false;
    }

    public void Attack(Attack A)
    {
        curAttack = A;
        timer = A.length;
        myAnim.Play(A.name, -1, 0); //playAnimation in code and resets anim
        Debug.Log(A.name + ": successfuly inputted");
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
        //isAttacking = false;
        currentCombos.Clear();
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

    public ComboInput(AttackType e, Vector2 m)
    {
        type = e;
        mType = Vector2.zero;
        mType = m;
    }
    public ComboInput(Vector2 e)
    {
        type = AttackType.movement;
        mType = e;
    }

    public bool IsSameAs(ComboInput J)
    {
        bool valid = validMovemment(J.mType);
        return ((type == AttackType.movement) ? (valid) : (type == J.type && valid)); 
        //In a single statement akin to an if statement. it checks movement inputs, then buttons and movement
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
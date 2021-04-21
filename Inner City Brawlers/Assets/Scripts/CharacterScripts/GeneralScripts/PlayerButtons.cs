using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;


public enum AttackType {light = 0, medium = 1, heavy = 2, unique = 3, throwAction = 4, Assist = 5, Lifeline = 6, movement = 7};

public class PlayerButtons : MonoBehaviour
{
    [Header("Script References")]
    [HideInInspector]
    [SerializeField] public PlayerMovement pM;
    [SerializeField] private RoundManager rM;
    [SerializeField] private GameMasterManager gMM;
    [SerializeField] private GameManagerScript gM;
    [SerializeField] private TrainingPauseManager tPM;
    [SerializeField] private AnimAttack animAtt;

    public GameObject[] hitboxes;

    [Header("Rewired Variables")]
    //Rewired
    [SerializeField] public int playerID;
    [SerializeField] public Player player;
    

    [Header("Attacks")]
    public Attack lightAttack;
    public Attack MediumAttack;
    public Attack HeavyAttack;
    public Attack UniqueAttack;
    public Attack ThrowAttack;
    public Attack AssistAttack;
    public Attack LifeLineAssist;

    [Header("Enemy Variables for AttackFunction")]
    public PlayerHealth enemyHealth;
    public PlayerMovement enemyMovement;
    private int myComboCounterInt;
    public int currentPlayer = 0;


    [SerializeField] public List<Combo> combos;
    [SerializeField] public float buttonCheckDuration = 0.2f;

    [Header("Components")]
    [SerializeField] private Animator myAnim;
    public GameObject AnimObject;
    public bool onCharacter;

    

    ComboInput LastInput = null;
    Attack curAttack = null;
    float timer = 0;
    float leeway = 0;
    [SerializeField] public List<int> currentCombos = new List<int>();
    bool skip = false;

    public bool isAttacking;

    public Transform attackpoint;
    public float attackRange = 0.5f;
    protected ContactFilter2D contactFilter;
    public LayerMask enemyLayers;
    public LayerMask myMask;

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
        if (playerID == 0)
        {
            currentPlayer = 1;
            myComboCounterInt = gM.p1comboCounter;
            gM.SetHitCounter(gM.p1comboCounterText, myComboCounterInt);
            
        }
        if (playerID == 1)
        {
            currentPlayer = 2;
            myComboCounterInt = gM.p2comboCounter;
            gM.SetHitCounter(gM.p2comboCounterText, myComboCounterInt);
        }
        myAnim = AnimObject.GetComponent<Animator>();
        animAtt = this.GetComponent<AnimAttack>();
        pM = this.GetComponentInParent<PlayerMovement>();
        rM = GameObject.Find("GameManager").GetComponent<RoundManager>();
        gMM = GameObject.Find("GameMasterManager").GetComponent<GameMasterManager>();
        gM = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        tPM = GameObject.Find("SettingManager").GetComponent<TrainingPauseManager>();

        player = ReInput.players.GetPlayer(pM.playerID);
        PrimeCombo();
        //myAnim = this.GetComponent<Animator>();
    }

    void SetAnimator(Animator curAnim)
    {
        myAnim = curAnim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PauseButtons();
        if (pM.isDisabled == false)
        {
            #region SwitchCase
            /*switch (pM.currentPlayState)
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
            }*/
            #endregion
            GameControls();
            checkForBlock();
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
                    Attack att = getAttackFromType(LastInput.type, this.pM.currentPlayState);
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
            if (onCharacter == false)
            {
                attackpoint.gameObject.SetActive(false);
                StartCoroutine(LAttack(0.2f));
            }
            
            input = new ComboInput(AttackType.light, pM.GetInput(), pM.currentPlayState); Debug.Log("Light attack");
        }
        if (player.GetButtonDown("MediumAttack"))
        {
            StartCoroutine(mediumAttack(0.6f));
            input = new ComboInput(AttackType.medium, pM.GetInput(), pM.currentPlayState); Debug.Log("medium attack ");
        }
        if (player.GetButtonDown("HeavyAttack"))
        {
            StartCoroutine(heavyAttack(1f));
            input = new ComboInput(AttackType.heavy, pM.GetInput(), pM.currentPlayState); Debug.Log("Heavy attack ");
        }

        if (pM.currentPlayState == PlayerMovement.playerState.Grounded ^ pM.currentPlayState == PlayerMovement.playerState.Crouch)
        {
            if (player.GetButtonDown("UniqueAttack"))
            {
                StartCoroutine(uniqueAttack(1.2f));
                input = new ComboInput(AttackType.unique, pM.GetInput(), pM.currentPlayState); Debug.Log("Unique attack ");

            }
            if (player.GetButtonDown("Throw"))
            {
                input = new ComboInput(AttackType.throwAction, pM.GetInput(), pM.currentPlayState); Debug.Log("Throw");

            }
            if (player.GetButtonDown("Use Assist"))
            {

                input = new ComboInput(AttackType.Assist, pM.GetInput(), pM.currentPlayState); Debug.Log("Use Assist ");
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
                input = new ComboInput(AttackType.Lifeline, pM.GetInput(), pM.currentPlayState); Debug.Log("LifeLine Assist Used ");
            }
            if (player.GetButtonDown("SwitchAnim"))
            {
                if (onCharacter == false)
                {
                    SetAnimator(this.GetComponent<Animator>());
                    onCharacter = true;
                }
                else if (onCharacter == true)
                {
                    SetAnimator(AnimObject.GetComponent<Animator>());
                    onCharacter = false;
                }

            }

            Vector2 movement = Vector2.zero;
            if (pM.InputDownX1())
            {
                DirectionLook dlook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
                if (playerID == 0)
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
                if (playerID == 1)
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
            Attack att = getAttackFromType(input.type, this.pM.currentPlayState);
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
    public void checkForBlock()
    {
        DirectionLook dlook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
        if (playerID == 0)
        {
            if (dlook.isFlipped == false)
            {
                if ((-pM.x1 >= 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            pM.isBlockingHigh = true;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(8);
                            if ((-pM.y1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                
                                this.contactFilter.SetLayerMask(9);

                            }
                        }
                        if (pM.otherPlayer.isAttacking == false)
                        {
                            pM.isBlockingHigh = false;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(7);
                        }
                    }
                    if (dlook.blockAvailable == false)
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(7);
                    }
                }
                else
                {
                    pM.isBlockingHigh = false;
                    pM.isBlockingLow = false;
                    contactFilter.SetLayerMask(7);
                }
                if ((-pM.y1 > 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            if ((-pM.x1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(9);
                            }
                        }
                    }
                    if (dlook.blockAvailable == false)
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(7);
                    }
                }

            }
            if (dlook.isFlipped == true)
            {
                if ((pM.x1 >= 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            pM.isBlockingHigh = true;
                            pM.isBlockingLow = false;
                            this.gameObject.layer = 8;
                            if ((-pM.y1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(9);
                            }
                        }
                        else
                        {
                            pM.isBlockingHigh = false;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(7);
                        }
                    }
                    else
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(7);
                    }
                }
                else
                {
                    pM.isBlockingHigh = false;
                    pM.isBlockingLow = false;
                    contactFilter.SetLayerMask(7);
                }
                if ((-pM.y1 > 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            if ((pM.x1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(9);
                            }
                        }
                    }
                    if (dlook.blockAvailable == false)
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(7);
                    }
                }

            }
        }
        if (playerID == 1)
        {
            if (dlook.isFlipped == true)
            {
                if ((-pM.x1 >= 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            pM.isBlockingHigh = true;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(11);
                            if ((-pM.y1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(12);
                            }
                        }
                        if (pM.otherPlayer.isAttacking == false)
                        {
                            pM.isBlockingHigh = false;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(10);
                        }
                    }
                    if (dlook.blockAvailable == false)
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(10);
                    }
                }
                else
                {
                    pM.isBlockingHigh = false;
                    pM.isBlockingLow = false;
                    contactFilter.SetLayerMask(10);
                }
                if ((-pM.y1 > 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            if ((-pM.x1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(11);
                            }
                        }
                    }
                    if (dlook.blockAvailable == false)
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(10);
                    }
                }

            }
            if (dlook.isFlipped == false)
            {
                if ((pM.x1 >= 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            pM.isBlockingHigh = true;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(12);
                            if ((-pM.y1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(11);
                            }
                        }
                        else
                        {
                            pM.isBlockingHigh = false;
                            pM.isBlockingLow = false;
                            contactFilter.SetLayerMask(10);
                        }
                    }
                    else
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(10);
                    }
                }
                else
                {
                    pM.isBlockingHigh = false;
                    pM.isBlockingLow = false;
                    contactFilter.SetLayerMask(10);
                }
                if ((-pM.y1 > 0.5))
                {
                    if (dlook.blockAvailable == true)
                    {
                        if (pM.otherPlayer.isAttacking == true)
                        {
                            if ((pM.x1 >= 0.3))
                            {
                                pM.isBlockingHigh = false;
                                pM.isBlockingLow = true;
                                contactFilter.SetLayerMask(11);
                            }
                        }
                    }
                    if (dlook.blockAvailable == false)
                    {
                        pM.isBlockingHigh = false;
                        pM.isBlockingLow = false;
                        contactFilter.SetLayerMask(10);
                    }
                }
            }
        }
    }


    public void KnockBack(float knockbackForceX, float knockbackForceY, Rigidbody2D playerRB)
    {
        DirectionLook dLook = GameObject.Find("GameManager").GetComponent<DirectionLook>();
        if (dLook.isFlipped == true)
        {
            playerRB.AddRelativeForce(Vector3.right * knockbackForceX, ForceMode2D.Impulse);
        }
        if (dLook.isFlipped == false)
        {
            playerRB.AddRelativeForce(Vector3.left * knockbackForceX, ForceMode2D.Impulse);
        }
        playerRB.AddRelativeForce(Vector3.up * knockbackForceY, ForceMode2D.Impulse);
    }

    public void SetIsAttackingTrue()
    {
        isAttacking = true;
    }

    public void DisableThenEnable()
    {
        StartCoroutine(DisableThenEnable(1f));
    }

    public void SetIsAttackingFalse()
    {
       //isAttacking = false;
    }

    public void Attack(Attack A)
    {
        curAttack = A;
        timer = A.length;
        myAnim.Play(A.name, 0, 0); //playAnimation in code and resets anim
        Debug.Log(A.name + ": successfuly inputted");
        StartCoroutine(RunAnimForLength(1f));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInChildren<AnimAttack>().AttackPlayer(enemyHealth, enemyMovement);

        }
        if (pM.currentPlayState == PlayerMovement.playerState.Grounded)
        {
            SetPlayerStateGrounded();
        }
        else if (pM.currentPlayState == PlayerMovement.playerState.Crouch)
        {
            SetPlayerStateCrouch();
        }
        else if (pM.currentPlayState == PlayerMovement.playerState.Jump)
        {
            SetPlayerStateJump();
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackpoint.position, attackRange);
    }
    IEnumerator RunAnimForLength(float time)
    {
        isAttacking = true;
        if (isAttacking == true)
        {
            attackpoint.gameObject.SetActive(true);
            //myAnim.SetTrigger("Attack");
            //SetPlayerStateImmobile();
        }
        yield return new WaitForSeconds(timer);
        isAttacking = false;
        if (isAttacking == false)
        {
            attackpoint.gameObject.SetActive(false);
        }
    }

    Attack getAttackFromType(AttackType t, PlayerMovement.playerState currState)
    {
        if (t == AttackType.light && currState == pM.currentPlayState)
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
        if (player.GetButtonDown("Pause"))
        {
            if (gMM.isMultiActive == true)
            {
                rM.PauseGame();
                rM.currentState = RoundManager.roundState.roundPaused;
            }
            if (gMM.isMultiActive == false)
            {
                
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
    IEnumerator DisableThenEnable(float time)
    {
        SetPlayerStateImmobile();
        yield return new WaitForSeconds(1.2f);
        SetPlayerStateGrounded();
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
        //pM.currentPlayState = PlayerMovement.playerState.Immobile;
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(false);
        hitboxes[2].gameObject.SetActive(false);
        hitboxes[3].gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hitboxes[3].gameObject.SetActive(false);
      //  StopCoroutine(uniqueAttack(1f));
       // pM.currentPlayState = PlayerMovement.playerState.Grounded;
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
    public PlayerMovement.playerState thisCurrentState;


    public ComboInput(AttackType e, Vector2 m , PlayerMovement.playerState currentState)
    {
        type = e;
        mType = Vector2.zero;
        mType = m;
        thisCurrentState = currentState;
    }
    public ComboInput(Vector2 e)
    {
        type = AttackType.movement;
        mType = e;
    }

    public bool IsSameAs(ComboInput J)
    {
        bool valid = validMovemment(J.mType);
        return ((type == AttackType.movement) ? (valid) : (type == J.type && valid && thisCurrentState == J.thisCurrentState)); 
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
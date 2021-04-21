using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class temp_char_controller : MonoBehaviour
{
    Animator tempAnimator;
    Rigidbody2D tempRB2D;
    SpriteRenderer tempSR;

    bool isGrounded;
    bool isCrouched;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    private float tempMvmtSpd;

    public int testMaxHealth = 100;
    public int testCurrentHealth;
    public temp_health_script testCharHealth;

    public int testCharMaxAssistMeter = 100;
    public int testCharCurrentAssistMeter;
    public temp_assistMeter_script testAssistMeter;

    public int testCharMaxSuperMeter = 100;
    public int testCharCurrentSuperMeter;
    public temp_superMeter_script testSuperMeter;

    bool isAttacking;

    [SerializeField]
    private float attackDelay;

    // Start is called before the first frame update
    void Start()
    {
        tempAnimator = GetComponent<Animator>();
        tempRB2D = GetComponent<Rigidbody2D>();
        tempSR = GetComponent<SpriteRenderer>();

        testCurrentHealth = testMaxHealth;
        testCharHealth.tempSetMaxHealth(testMaxHealth);

        testCharCurrentAssistMeter = testCharMaxAssistMeter;
        testAssistMeter.tempSetMaxAssist(testCharMaxAssistMeter);

        testCharCurrentSuperMeter = testCharMaxSuperMeter;
        testSuperMeter.tempSetSuperMeter(testCharMaxSuperMeter);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))) {

            isGrounded = true;
        
        }//Is the player grounded?
        else {

            isGrounded = false;
        
        }//The player is not on the ground

        if (Input.GetKey("d") || Input.GetKey("right"))
        {

            tempRB2D.velocity = new Vector2(tempMvmtSpd, tempRB2D.velocity.y);

            if (isGrounded) { 
                tempAnimator.Play("char_temp_moving_forward");
                isAttacking = false;
            }

            tempSR.flipX = false;
            isCrouched = false;
        }//moving right
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {

            tempRB2D.velocity = new Vector2(-tempMvmtSpd, tempRB2D.velocity.y);

            if (isGrounded) { 
                tempAnimator.Play("char_temp_moving_back");
                isAttacking = false;
            }
            isCrouched = false;
        }//moving left
        else if (Input.GetKey("s") || Input.GetKey("down"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_crouch");
                isCrouched = true;
                isAttacking = false;
            }
        }//crouching
        else
        {

            if (isGrounded && !isAttacking) { 
                tempAnimator.Play("char_temp_idle");
            }

            tempRB2D.velocity = new Vector2(0, tempRB2D.velocity.y);
            isCrouched = false;
            isAttacking = false;
        }//idle

        if (Input.GetKey("w") && isGrounded || Input.GetKey("up") && isGrounded)
        {

            tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, 9);
            tempAnimator.Play("char_temp_jump");
            isCrouched = false;
            isAttacking = false;

        }//jumping

        if (Input.GetKey("i"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_punch_light");
                isCrouched = false;
                isAttacking = true;

                if (isCrouched || Input.GetKey("s") || Input.GetKey("down"))
                {
                    tempRB2D.velocity = new Vector2(0, 0);
                    tempAnimator.Play("char_temp_crouch_punch_light");
                    isCrouched = true;
                }
            }
            else if (!isGrounded) {

                tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, tempRB2D.velocity.y);
                tempAnimator.Play("char_temp_air_punch_light");
                isCrouched = false;
            }
        }//light punch (standing, air, and crouching)
        else if (Input.GetKey("o"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_punch_medium");
                isCrouched = false;
                isAttacking = true;

                if (isCrouched || Input.GetKey("s") || Input.GetKey("down"))
                {
                    tempRB2D.velocity = new Vector2(0, 0);
                    tempAnimator.Play("char_temp_crouch_punch_medium");
                    isCrouched = true;
                }
            }
            else if (!isGrounded) {

                tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, tempRB2D.velocity.y);
                tempAnimator.Play("char_temp_air_punch_medium");
                isCrouched = false;
            }
        }//medium punch (standing, air, and crouching)
        else if (Input.GetKey("p"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_punch_heavy");
                isCrouched = false;
                isAttacking = true;

                if (isCrouched || Input.GetKey("s") || Input.GetKey("down"))
                {
                    tempRB2D.velocity = new Vector2(0, 0);
                    tempAnimator.Play("char_temp_crouch_punch_heavy");
                    isCrouched = true;
                }
            }
            else if (!isGrounded) {

                tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, tempRB2D.velocity.y);
                tempAnimator.Play("char_temp_air_punch_heavy");
                isCrouched = false;
            }
        }//heavy punch (standing, air, and crouching)

        if (Input.GetKey("j"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_kick_light");
                isCrouched = false;
                isAttacking = true;

                if (isCrouched || Input.GetKey("s") || Input.GetKey("down"))
                {

                    tempRB2D.velocity = new Vector2(0, 0);
                    tempAnimator.Play("char_temp_crouch_kick_light");
                    isCrouched = true;
                }
            }
            else if (!isGrounded) {

                tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, tempRB2D.velocity.y);
                tempAnimator.Play("char_temp_air_kick_light");
                isCrouched = false;
            }
        }//light kick (standing, air, and crouching)
        else if (Input.GetKey("k"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_kick_medium");
                isAttacking = true;
                isCrouched = false;

                if (isCrouched || Input.GetKey("s") || Input.GetKey("down"))
                {

                    tempRB2D.velocity = new Vector2(0, 0);
                    tempAnimator.Play("char_temp_crouch_kick_medium");
                    isCrouched = true;
                }
            }
            else if (!isGrounded) {

                tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, tempRB2D.velocity.y);
                tempAnimator.Play("char_temp_air_kick_medium");
                isCrouched = false;
            }
        }//medium kick (standing, air, and crouching)
        else if (Input.GetKey("l"))
        {
            if (isGrounded)
            {
                tempRB2D.velocity = new Vector2(0, 0);
                tempAnimator.Play("char_temp_kick_heavy");
                isCrouched = false;
                isAttacking = true;

                if (isCrouched || Input.GetKey("s") || Input.GetKey("down"))
                {

                    tempRB2D.velocity = new Vector2(0, 0);
                    tempAnimator.Play("char_temp_crouch_kick_heavy");
                    isCrouched = true;
                }
            }
            else if (!isGrounded) {

                tempRB2D.velocity = new Vector2(tempRB2D.velocity.x, tempRB2D.velocity.y);
                tempAnimator.Play("char_temp_air_kick_heavy");
                isCrouched = false;
            }
        }//heavy kick (standing, air, and crouching)

        if (Input.GetKey("u") && isGrounded && !isCrouched)
        {

            tempRB2D.velocity = new Vector2(0, 0);
            tempAnimator.Play("char_temp_grab");
            isCrouched = false;
            isAttacking = true;

        }//grab

        if (Input.GetKey("h") && isGrounded)
        {

            tempRB2D.velocity = new Vector2(0, 0);
            tempAnimator.Play("char_temp_assist");
            testUseAssistMeter(10);
            isCrouched = false;
            isAttacking = true;

        }//assist

        if (Input.GetKey("y") && isGrounded)
        {

            tempRB2D.velocity = new Vector2(0, 0);
            tempAnimator.Play("char_temp_SUPER");
            isCrouched = false;
            isAttacking = true;
            testUseSuperMeter(20);

        }//SUPER MOVE (DOUBLE TRIGGER)

        if (Input.GetKey("1")) {

            testDamage(10);
        
        }//Testing damage intake
    }

    void testDamage(int testDamageTaken) {

        testCurrentHealth -= testDamageTaken;
        testCharHealth.tempSetCurrentHealth(testCurrentHealth);


    }

    void testUseAssistMeter(int testAssistPointsTaken)
    {

        testCharCurrentAssistMeter -= testAssistPointsTaken;
        testAssistMeter.tempSetCurrentAssistMeter(testCharCurrentAssistMeter);

    }

    void testUseSuperMeter(int testSuperPointsTaken)
    {

        testCharCurrentSuperMeter -= testSuperPointsTaken;
        testSuperMeter.tempSetCurrentSuperMeter(testCharCurrentSuperMeter);

    }

    void AttackFin()
    {
        isAttacking = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_WeaponFunction : MonoBehaviour
{
    public itemObject B_Weapon;
    public float lifeTime;
    public bool isGrounded;

    public void Awake()
    {
        lifeTime = B_Weapon.itemLifeTime;//preset in the ScriptableObject
    }

    public void Update()
    {
        if (isGrounded)
        {
            tickDown();//calls tickDown Method while it is on the ground
        }
        if (lifeTime <= 0)
        {
            DestroySelf();//calls DestroySelf Method after lieftime is up  
        }
    }

    public void tickDown()
    {
        lifeTime -= Time.deltaTime;//ticks down lifetime counter as
    }
    private void DestroySelf()
    {
        Destroy(gameObject);//destroys self
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            Debug.Log(other.gameObject.name + " picked up " + this.gameObject.name);//temporary
            other.gameObject.GetComponent<Bweap_Timer>().ResetTimer();//finds blunt weapon timer script on interacting player and resets it to max time
            other.gameObject.GetComponent<Bweap_Timer>().hasWeapon = true;//sets bool on b-weapon script to true which activates the countdown
            DestroySelf();//temporary until full function is completed
        }
    }
}

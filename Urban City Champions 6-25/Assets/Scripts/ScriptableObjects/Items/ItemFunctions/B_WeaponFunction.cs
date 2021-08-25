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
            tickDown();//calls tickDown Method
        }
        if (lifeTime <= 0)
        {
            DestroySelf();//calls DestroySelf Method
        }
    }

    public void tickDown()
    {
        lifeTime -= Time.deltaTime;//ticks down lifetime counter
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
            other.gameObject.GetComponent<Bweap_Timer>().ResetTimer(); // finds bweap timer on interacting player and resets timer
            other.gameObject.GetComponent<Bweap_Timer>().hasWeapon = true; // sets bool on bweap timer true
            DestroySelf();//temporary until full function is completed
        }
    }
}

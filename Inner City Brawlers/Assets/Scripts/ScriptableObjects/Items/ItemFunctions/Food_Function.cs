using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Function : MonoBehaviour
{
    public itemObject FoodItem;
    public float lifeTime;
    public bool isGrounded;

    public void Awake()
    {
        lifeTime = FoodItem.itemLifeTime;//preset in the ScriptableObject
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
        if (col.gameObject.tag == "Ground")
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
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(other.gameObject.name + " picked up " + this.gameObject.name);//temporary
            DestroySelf();//temporary until full function is completed
        }
    }
}

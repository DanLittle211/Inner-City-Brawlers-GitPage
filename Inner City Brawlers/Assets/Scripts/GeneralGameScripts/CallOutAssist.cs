using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CallOutAssist : MonoBehaviour
    
{
    [Header("Script References")]
    public ItemLists itemList;
    [Header("Bool Controllers")]
    public bool AssistAllowed;//Tracks if the option is toggled On or Off
    public bool oneTime;//Allows spawning of item when true
    [Header("For Choosing Items to Instantiate")]
    public float R_itemType;//for choosing which random item
    public float R_Item;//For choosing which item is actually spawned based on the type list
    public int spawningItem;//For converting float to int
    [Header("For Spawning the item")]
    public GameObject itemToSpawn;
    public Vector2 randomSpawnPoint, leftMax, rightMax;
    public float randomSpawnX;//random X-axis number that is between the LeftMax.x and rightMax.x
   
    //temporary for testing
    private void Awake()
    {
        AssistAllowed = true;//Temporary for testing, makes the assist being on true
        itemList = GameObject.Find("GameManager").GetComponent<ItemLists>();//Reference for itemLists, found on GameManager object
    }
    void Update()
    {
        //If the Assist is set to be ON the E-key will be available to instantiate the random item to assist the player
        if (AssistAllowed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                setOneTime();//calls method that sets the spawing bool to true
            }
        }
        //what happens when the spawning bool is set true
        if (oneTime)
        {
            setSpawnPoint();
            spawnItem(whichItem(whichItemType()));//Spawning random item
        }
    }
    public void setOneTime()
    {
        oneTime = true;
    }
    //Generates random number for choosing which type of item will be spawned, returns number(Float) which is chosen
    public float whichItemType()
    {
       R_itemType = Mathf.Round(Random.Range(0, 3));//chooses random number 0-2, Mathf.Rounded to make Whole numbers
       return R_itemType;
    }
    //ganerates random number for choosing which item from the choosen item list will spawn, returns the gameObject to spawn
    public Vector2 setSpawnPoint()
    {
        randomSpawnX = Mathf.Round(Random.Range(leftMax.x + 1, rightMax.x - 1));
        randomSpawnPoint.x = randomSpawnX;
        return randomSpawnPoint;
    }
    public GameObject whichItem(float itemType)
    {
        //For spawning a random food item
        if(itemType == 0)
        {
            R_Item = Mathf.Round(Random.Range(0, itemList.Food_List.Count));//largest number is based on assossiated list length.count
            itemToSpawn = itemList.Food_List[(int) R_Item].gameObject;//spawning items based on the random number
        }
        //for spawning a random Blunt Weapon
        if(itemType == 1)
        {
            R_Item = Mathf.Round(Random.Range(0, itemList.BluntWeapon_List.Count));//largest number is based on assossiated list length.count
            itemToSpawn = itemList.BluntWeapon_List[(int) R_Item].gameObject;//spawning items based on the random number
        }
        //for spawning a random Projectile Weapon
        if (itemType == 2)
        {
            Mathf.Round(Random.Range(0, itemList.ProjectileWeapon_List.Count));//largest number is based on assossiated list length.count
            itemToSpawn = itemList.ProjectileWeapon_List[(int)R_Item].gameObject;//spawning items based on the random number
        }
        return itemToSpawn;
    }
    //spawns actual the actual item, takes a gameObject as a parameter
    public void spawnItem(GameObject itemToSpawn)
    {
        Instantiate(itemToSpawn, randomSpawnPoint, Quaternion.identity);//Spawns the random opject chosen at a random SpawnPoint in range
        oneTime = false;//turns spawning abilitly to false
    }

}

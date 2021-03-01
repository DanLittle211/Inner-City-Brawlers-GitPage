using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CallOutAssist : MonoBehaviour
    
{
    [Header("Script References")]
    public ItemLists itemList;//reference for the list of items, used for selecting which item is spawned
    [Header("UI Elements")]
    public Slider p1AssistMeter;//reference for p1's callout assist meter
    public Slider p2AssistMeter;//reference for p2'2 callout assist meter
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
    [Header("Tracking Spawning timer")]
    public bool p1CanSpawn;//flag for when player 1 can spawn an item when the assist is allowed in settings
    public bool p2CanSpawn;//flag for when player 2 can spawn an item when the assist is allowed in settings
    public float p1Timer;//timer amount for when player 1 can spawn an item
    public float p2Timer;//timer amount for when player 2 can spawn an item
    public float time2Spawn;//The number that the timers need to reach to enable spawning for each player

    private void Awake()
    {
        AssistAllowed = true;//Temporary for testing, makes the assist being on true
        itemList = GameObject.Find("GameManager").GetComponent<ItemLists>();//Reference for itemLists, found on GameManager object
    }
    void Update()
    {
        //limiting each timer's upper limit to the time2Spawn number
        if (p1Timer >= time2Spawn) p1Timer = time2Spawn;
        if (p2Timer >= time2Spawn) p2Timer = time2Spawn;

        //If the Assist is set to be ON the N-key(Player1) or M-Key(Player2) will be available to instantiate the random item to assist the player
        if (AssistAllowed)
        {
            //updating the assist meters with player timers
            p1AssistMeter.value = p1Timer;//setting slider value to current counting time for p1
            p1AssistMeter.maxValue = time2Spawn;//sets p1's meter max value to time2Spawn
            p2AssistMeter.value = p2Timer;//setting slider value to current counting time for p2
            p2AssistMeter.maxValue = time2Spawn;//sets p2's meter max value to time2Spawn

            timerTickUp();//calls method that increases each player's timer for item spawns
            //for player 1
            if (p1CanSpawn)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    setOneTime();//calls method that sets the spawing bool to true
                    p1Timer = 0;//sets p1's timer back to 0, turning p1CanSpawn false
                }
            }
            //for player 2
            if (p2CanSpawn)
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    setOneTime();//calls method that sets the spawing bool to true
                    p2Timer = 0;//sets p2's timer back to 0, turning p2CanSpawn false
                }
            }
        }
        //what happens when the spawning bool is set true
        if (oneTime)
        {
            setSpawnPoint(); //calls method that sets the point where the item will be spawned
            spawnItem(whichItem(whichItemType()));//calls methods that spawn a random item
        }
        //for spawning timers
        //setting p1CanSpawn to true if equal to or greater than the set number
        if (p1Timer >= time2Spawn)
        {
            p1CanSpawn = true;
        }
        else p1CanSpawn = false;
        //setting p2CanSpawn to true if equal to or greater than the set number
        if (p2Timer >= time2Spawn)
        {
            p2CanSpawn = true;
        }
        else p2CanSpawn = false;
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
        randomSpawnX = Mathf.Round(Random.Range(leftMax.x + 1, rightMax.x - 1));//choosing a random number between the two point's x-axis
        randomSpawnPoint.x = randomSpawnX;//sets the X-Axis of the spawn point to the random number
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
        Debug.Log("Spawned: " + itemToSpawn.name);
        oneTime = false;//turns spawning abilitly to false
    }
    public void timerTickUp()
    {
        p1Timer += Time.deltaTime;
        p2Timer += Time.deltaTime;
    }
}

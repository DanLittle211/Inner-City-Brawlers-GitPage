using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject tempPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(tempPlayer.transform.position.x, tempPlayer.transform.position.y, -10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]private Vector2 parrallaxMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCamPos;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("FightViewCamera").transform;
        lastCamPos = cameraTransform.position;
    }

    private void Update()
    {

    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCamPos;
        
        transform.position += new Vector3( deltaMovement.x * parrallaxMultiplier.x, deltaMovement.y * parrallaxMultiplier.y);
        lastCamPos = cameraTransform.position;
    }
}

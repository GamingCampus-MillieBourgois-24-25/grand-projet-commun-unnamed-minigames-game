using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for moving the tile
 */

public class MovingTile : MonoBehaviour
{

    public float speed = 15f;
    public GameObject startPoint; /*position where the tile will be set to once it reaches the endPoint*/
    

    void Update()
    {
        
        transform.position += new Vector3(0, 0, -speed*Time.deltaTime);
    }
    

    void TeleportToPointZero()
    {
        transform.position += new Vector3(0, 0, 150);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            //TeleportToStartPoint();
            TeleportToPointZero();  
            Debug.Log("Collision entre "+gameObject.name+" et "+other.gameObject.name);
            //Debug.Log("Position : "+transform.position+" - "+startPoint.transform.position);
        }
    }
    public float getSpeed()
    {
        return speed;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}

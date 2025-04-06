using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for moving the tile
 */

public class MovingTile : MonoBehaviour
{

    public float speed = 1f;
    public GameObject startPoint; /*position where the tile will be set to once it reaches the endPoint*/


    // Start is called before the first frame update
    void Start()
    {
        check();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, -speed*Time.deltaTime);
    }

    void check()
    {
        if (startPoint)
        {
            return;
        }
        else
        {
            startPoint = GameObject.Find("StartPoint");
            if (!startPoint)
            {
                Debug.Log("MovingTile.cs : variable startPoint non trouvé dans la hiérarchie");
            }
        }
    }

    /*Petit probleme : teleport a coté du start poit et non dessus*/
    void TeleportToStartPoint()
    {
        transform.position = startPoint.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            TeleportToStartPoint();
            Debug.Log("Collision entre "+gameObject.name+" et "+other.gameObject.name);
            Debug.Log("Position : "+transform.position+" - "+startPoint.transform.position);
        }
    }

}

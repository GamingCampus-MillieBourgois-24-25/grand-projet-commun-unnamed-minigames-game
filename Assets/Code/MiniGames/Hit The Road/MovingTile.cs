using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for moving the tile
 */

public class MovingTile : MonoBehaviour
{

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, -speed*Time.deltaTime);
    }
}

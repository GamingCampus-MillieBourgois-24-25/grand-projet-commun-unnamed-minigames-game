using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for generating tile and moving them at start position when colliding with endpoint
 */

public class GenerateTiles : MonoBehaviour
{
    int tileScale = 5;
    public float tileSpeed = 40f;

    public GameObject tile;
    public GameObject startPoint; /*position where the tile will be set to once it reaches the endPoint*/
    public GameObject endPoint; 
    /*Position for the front tile, the tile rendered by the camera, and the last tile*/
    Vector3 frontPosition;
    Vector3 mainPosition;
    Vector3 backPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (tile) 
        {
            check();
            //SettingUpTheScene2();
            SettingUpTheDesertScene();
        }

        else
        {
            Debug.Log("Erreur variable Tile non assigné !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void check()
    {
        if (tile)
        {
            if (startPoint)
            {
                if (endPoint)
                {
                    return;
                }
                else
                {
                    Debug.Log("GenerateTile.cs : variable endPoint non assigné");
                }
            }
            else
            {
                //Debug.Log("GenerateTile.cs : variable startPoint non assigné");
                startPoint = GameObject.Find("StartPoint");
            }
        }
        else
        {
            Debug.Log("GenerateTile.cs : variable tilenon assigné");
        }
    }

    void SettingUpTheDesertScene()
    {
        mainPosition.z = 50f;
        Instantiate(tile, mainPosition, Quaternion.identity);

        mainPosition = new Vector3(0, 0, 0);
        Instantiate(tile, mainPosition, Quaternion.identity);

        mainPosition.z = -50f;
        Instantiate(tile, mainPosition, Quaternion.identity);
        
        tile.GetComponent<MovingTile>().setSpeed(tileSpeed);
    }
 
}

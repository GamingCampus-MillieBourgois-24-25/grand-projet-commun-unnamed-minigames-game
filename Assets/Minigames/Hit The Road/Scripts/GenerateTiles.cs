using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using AxoLoop.Minigames.HitTheRoad;

/*
 * Script for generating tile and moving them at start position when colliding with endpoint
 */

public class GenerateTiles : MonoBehaviour
{
    int tileScale = 5;
    public float tileSpeed = 40f;
  
    public GameObject[] tile;
    public GameObject startPoint; /*position where the tile will be set to once it reaches the endPoint*/
    public GameObject endPoint; 
    public GameObject emptyObject; //parent des objets généré dynamiquement par Instantiate
    private int index;
    /*Position for the front tile, the tile rendered by the camera, and the last tile*/
    Vector3 frontPosition;
    Vector3 mainPosition;
    Vector3 backPosition;

    // Start is called before the first frame update
    //void Start()
    //{

    //    check();
    //    SettingUpTheScene();
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void check()
    {
       if(tile.Length == 0)
        {
            Debug.Log("Tableau de tuile vide !");
            return;
        }
    }

    public void SettingUpTheScene()
    {
        index = Random.Range(0, tile.Length);

        mainPosition.z = 50f;
        Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        mainPosition = new Vector3(0, 0, 0);
        Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        mainPosition.z = -50f;
        Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        tile[index].GetComponent<MovingTile>().setSpeed(tileSpeed);
        
        Debug.Log(tile[index].GetComponent<MovingTile>().getName() + " index :"+ index);
        Debug.Log(" length :" + (tile.Length-1));
    }
    
    public void setIndex(int index)
    {
        this.index = index;
    }
}

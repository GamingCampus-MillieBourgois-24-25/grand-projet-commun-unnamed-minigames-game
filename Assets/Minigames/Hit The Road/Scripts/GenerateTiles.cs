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
    public GameObject CamPosition1; /*position where the tile will be set to once it reaches the endPoint*/
    public GameObject CamPosition2; 
    public GameObject emptyObject; //parent des objets généré dynamiquement par Instantiate
    public GameObject Light;
    private int index;
    Vector3 mainPosition;

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
        /* Choix aleatoire de la tuile */
        index = Random.Range(0, tile.Length);

        mainPosition.z = 50f;
        Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        mainPosition = new Vector3(0, 0, 0);
        Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        mainPosition.z = -50f;
        Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        tile[index].GetComponent<MovingTile>().setSpeed(tileSpeed);

        /* Choix aleatoire de la position de la camera*/
        int camPos = Random.Range(0, 2);
        if (camPos == 0)
        {
            Camera.main.transform.position = CamPosition1.transform.position;
            Camera.main.transform.rotation = CamPosition1.transform.rotation;
        }
        if (camPos == 1)
        {
            Camera.main.transform.position = CamPosition2.transform.position;
            Camera.main.transform.rotation = CamPosition2.transform.rotation;
        }

        /* Choix aleatoire de la rotation de la light*/
        Quaternion lightRotation = Light.transform.rotation;
        lightRotation.x = Random.Range(25.5f, 149.4f);
        lightRotation.x = Random.Range(-644.9f,-385.4f);
        Light.transform.rotation = lightRotation;


        Debug.Log(tile[index].GetComponent<MovingTile>().getName() + " index :"+ index);
        Debug.Log(" length :" + (tile.Length-1));
        Debug.Log("Camera pos " + Camera.main.transform.position + "campos = "+camPos);
        Debug.Log("light rotation : " + Light.transform.rotation);
    }
    
    public void setIndex(int index)
    {
        this.index = index;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using AxoLoop.Minigames.HitTheRoad;
using Axoloop.Global;

/*
 * Script for generating tile and moving them at start position when colliding with endpoint
 */

public class GenerateTiles : SingletonMB<GenerateTiles>
{
    public float tileSpeed = 40f;
  
    public GameObject[] tile;
    public CameraPosition CamPosition1; /*position where the tile will be set to once it reaches the endPoint*/
    public CameraPosition CamPosition2; 
    public CameraPosition CamPosition3; 
    public CameraPosition CamPosition4;
    public Onomatopea activeOnomatopea;
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

    public void ShowOnomatopea()
    {
        activeOnomatopea.enabled = true;
    }

    public void SettingUpTheScene()
    {
        /* Choix aleatoire de la tuile */
        index = Random.Range(0, tile.Length);

        mainPosition.z = 25f;
        var tileI = Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);
        tileI.GetComponent<MovingTile>().setSpeed(tileSpeed);

        mainPosition = new Vector3(0, 0, -25);
        tileI = Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);
        tileI.GetComponent<MovingTile>().setSpeed(tileSpeed);

        //mainPosition.z = -50f;
        //Instantiate(tile[index], mainPosition, Quaternion.identity, emptyObject.transform);

        tile[index].GetComponent<MovingTile>().setSpeed(tileSpeed);

        /* Choix aleatoire de la position de la camera*/
        int camPos = Random.Range(0, 4);
        Transform position;
        switch (camPos)
        {
            case 0:
                position = CamPosition1.transform;
                activeOnomatopea = CamPosition1.GetOnomatopea;
                break;
            case 1:
                position = CamPosition2.transform;
                activeOnomatopea = CamPosition2.GetOnomatopea;
                break;
            case 2:
                position = CamPosition3.transform;
                activeOnomatopea = CamPosition3.GetOnomatopea;
                HitTheRoadController.Instance.invertButtons = true;
                break;
            case 3:
            default:
                position = CamPosition4.transform;
                activeOnomatopea = CamPosition4.GetOnomatopea;
                HitTheRoadController.Instance.invertButtons = true;
                break;
        }

        Camera.main.transform.position = position.position;
        Camera.main.transform.rotation = position.rotation;


        /* Choix aleatoire de la rotation de la light*/
        float randomX = Random.Range(25.5f, 149.4f);     // Inclinaison verticale
        float randomY = Random.Range(-644.9f, -385.4f);  // Orientation horizontale

        Light.transform.rotation = Quaternion.Euler(randomX, randomY, 0f);


       
        Debug.Log(" length :" + (tile.Length-1));
        Debug.Log("Camera pos " + Camera.main.transform.position + "campos = "+camPos);
        Debug.Log("light rotation : " + Light.transform.eulerAngles);
    }
    
    public void setIndex(int index)
    {
        this.index = index;
    }
}

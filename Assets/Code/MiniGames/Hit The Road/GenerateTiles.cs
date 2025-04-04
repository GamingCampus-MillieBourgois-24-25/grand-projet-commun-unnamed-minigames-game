using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTiles : MonoBehaviour
{
    public GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        if (tile) 
        {
            Instantiate(tile, new Vector3(0,0,0), Quaternion.identity);    
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
}

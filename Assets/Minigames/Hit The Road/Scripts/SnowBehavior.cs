using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Place la neige devant la camera 
 */
public class SnowBehavior : MonoBehaviour
{
    GameObject SnowParticles; 
    // Start is called before the first frame update
    void Start()
    {
        SnowParticles = GameObject.Find("SnowParticle");
        if (SnowParticles)
        {
            SnowParticles.transform.position = Camera.main.transform.position + Camera.main.transform.forward*1f ;
        }
        else
        {
            Debug.Log("Particule de neige non trouvé !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

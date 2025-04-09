using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoeType
{
    Liquid,
    Fire,
    Food,
    Wind
}
public class Foe : MonoBehaviour
{
    public string Name {  get; set; }
    public Sprite AliveSprite { get; set; }
    public Sprite DeadSprite { get; set; }

    public FoeType FoeType { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Attack()
    {

    }

    void Die()
    {

    }
}

using System;
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

[RequireComponent(typeof(SpriteRenderer))]
public class Foe : MonoBehaviour
{
    public string Name {  get; private set; }
    [SerializeField] protected Sprite AliveSprite;
    //public Sprite DeadSprite { get; private set; }
    [SerializeField] protected FoeType foeType;

    public FoeType FoeType => foeType;

    protected SpriteRenderer spriteRenderer;


    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = AliveSprite;
    }

    protected virtual void Attack(Action callBack)
    {
        callBack?.Invoke();
    }

    protected virtual void Die(Action callBack)
    {
        callBack?.Invoke();
    }



}
